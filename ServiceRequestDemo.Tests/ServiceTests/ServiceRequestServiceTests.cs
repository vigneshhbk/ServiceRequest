using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceRequestDemo.AutoMapper;
using ServiceRequestDemo.DatabaseAccess.TableClasses;
using ServiceRequestDemo.Models;
using ServiceRequestDemo.Repository.Interfaces;
using ServiceRequestDemo.Service;
using ServiceRequestDemo.Service.Interfaces;
using System.Xml.Linq;

namespace ServiceRequestDemo.Tests.ServiceTests
{
    public class ServiceRequestServiceTests
    {
        private readonly Mock<IServiceRequestRepository> _repositoryMock;
        private readonly IServiceRequestService _serviceRequestService;
        private readonly Mock<Microsoft.Extensions.Logging.ILogger<ServiceRequestService>> _logger;
        private readonly IMapper _mapper;

        AutoMapperProfile myProfile = new AutoMapperProfile();
        

        public ServiceRequestServiceTests()
        {
            _repositoryMock = new Mock<IServiceRequestRepository>();
            _logger = new Mock<Microsoft.Extensions.Logging.ILogger<ServiceRequestService>>();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);
            _serviceRequestService = new ServiceRequestService(_repositoryMock.Object, _mapper, _logger.Object);
        }

        [Fact]
        public void GetServiceRequests_ReturnList()
        {
            Guid id = Guid.NewGuid();
            var serviceRequestsList = new List<ServiceRequest> { new ServiceRequest {
                Id = id,
                Description = "Please turn off the AC",
                BuildingCode = "COH",
                CurrentStatus = 1,
                CreatedBy = "Test User",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Test User",
                LastModifiedDate = DateTime.Now
            } };

            _repositoryMock.Setup(repo =>  repo.GetAll()).Returns(serviceRequestsList).Verifiable();

            List<Models.ServiceRequestDTO>? result = _serviceRequestService.GetServiceRequests();

            _repositoryMock.Verify();
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(id, result.First().Id);
        }

        [Fact]
        public void GetServiceRequests_ReturnEmptyList()
        {
            _repositoryMock.Setup(repo => repo.GetAll()).Returns((List<ServiceRequest>)null);

            List<Models.ServiceRequestDTO>? response = _serviceRequestService.GetServiceRequests();
            Assert.Null(response);
        }

        [Fact]
        public void GetServiceRequestsById_ReturnsRequest_IfRequestExists()
        {
            Guid id = Guid.NewGuid();
            var serviceRequest = new ServiceRequest {
                Id = id,
                Description = "Please turn off the AC",
                BuildingCode = "COH",
                CurrentStatus = 1,
                CreatedBy = "Test User",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Test User",
                LastModifiedDate = DateTime.Now
            };

            _repositoryMock.Setup(repo => repo.GetById(id)).Returns(serviceRequest);

            Models.ServiceRequestDTO? result = _serviceRequestService.GetServiceRequestById(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public void GetServiceRequestsById_ReturnsNull_IfRequestDoesntExist()
        {
            Guid id = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetById(id)).Returns((ServiceRequest)null);

            Models.ServiceRequestDTO? response = _serviceRequestService.GetServiceRequestById(id);
            Assert.Null(response);
        }

        [Fact]
        public void DeleteServiceRequest_ReturnsTrue_IfRequestDeleted()
        {
            Guid id = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.Delete(id)).Returns(true);

            bool response = _serviceRequestService.DeleteServiceRequest(id);
            Assert.True(response);
        }

        [Fact]
        public void DeleteServiceRequest_ReturnsFalse_IfRequestNotDeleted()
        {
            Guid id = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.Delete(id)).Returns(false);

            bool response = _serviceRequestService.DeleteServiceRequest(id);
            Assert.False(response);
        }

        [Fact]
        public void AddServiceRequest_AddsRequest_ReturnsRequest()
        {
            Guid id = Guid.NewGuid();
            var sr = new ServiceRequestDTO
            {
                Id = id,
                Description = "Please turn off the AC",
                BuildingCode = "COH",
                CurrentStatus = Common.CurrentStatus.Created,
                CreatedBy = "Test User",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Test User",
                LastModifiedDate = DateTime.Now
            };

            _repositoryMock.Setup(repo => repo.Add(_mapper.Map<ServiceRequest>(sr))).Returns(_mapper.Map<ServiceRequest>(sr));

            Models.ServiceRequestDTO? result = _serviceRequestService.AddServiceRequest(sr);
            Assert.NotNull(result);
            Assert.Equal(sr.Id, result.Id);
        }

        [Fact]
        public void UpdateServiceRequest_WhenUpdateFails_ReturnsNull()
        {
            Guid id = Guid.NewGuid();
            var sr = new ServiceRequestDTO
            {
                Id = id,
                Description = "Please turn off the AC",
                BuildingCode = "COH",
                CurrentStatus = Common.CurrentStatus.Created,
                CreatedBy = "Test User",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Test User",
                LastModifiedDate = DateTime.Now
            };

            _repositoryMock.Setup(repo => repo.Update(_mapper.Map<ServiceRequest>(sr))).Returns((ServiceRequest)null);

            Models.ServiceRequestDTO? result = _serviceRequestService.UpdateServiceRequest(sr);
            Assert.Null(result);
        }
    }
}
