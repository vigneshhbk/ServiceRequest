using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceRequestDemo.Controllers;
using ServiceRequestDemo.Service.Interfaces;
using Xunit;
using Moq;
using ServiceRequestDemo.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceRequestDemo.DatabaseAccess.TableClasses;
using Microsoft.AspNetCore.Http;

namespace ServiceRequestDemo.Tests.ControllerTests
{
    public class ServiceRequestHandlerTests
    {
        private readonly ServiceRequestController _controller;
        private readonly Mock<IServiceRequestService> _serviceRequestService;
        private readonly Mock<Microsoft.Extensions.Logging.ILogger<ServiceRequestController>> _logger;

        public ServiceRequestHandlerTests()
        {
            _serviceRequestService = new Mock<IServiceRequestService>();
            _logger = new Mock<Microsoft.Extensions.Logging.ILogger<ServiceRequestController>>();
            _controller = new ServiceRequestController(_serviceRequestService.Object, _logger.Object);
        }

        [Fact]
        public void Get_ShouldReturnRequests_IfRequestsExists()
        {
            Guid id = Guid.NewGuid();
            var serviceRequestsList = new List<ServiceRequestDTO> { new ServiceRequestDTO {
                Id = id,
                Description = "Please turn off the AC",
                BuildingCode = "COH",
                CurrentStatus = Common.CurrentStatus.Created,
                CreatedBy = "Test User",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Test User",
                LastModifiedDate = DateTime.Now
            } };

            _serviceRequestService.Setup(service => service.GetServiceRequests()).Returns(serviceRequestsList);

            var response = _controller.Get();
            var result = response.Result as OkObjectResult;

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ServiceRequestDTO>>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Single(returnValue);
            Assert.Equal(id, returnValue.First().Id);
        }

        [Fact]
        public void Get_ShouldReturnEmptyContent_IfRequestsIsEmpty()
        {
            _serviceRequestService.Setup(service => service.GetServiceRequests()).Returns((List<ServiceRequestDTO>)null);

            var response = _controller.Get();
            var result = response.Result as NoContentResult;

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public void GetById_ShouldReturnRequest_IfRequestExists()
        {
            Guid id = Guid.NewGuid();
            var serviceRequestsList = new List<ServiceRequestDTO> { new ServiceRequestDTO {
                Id = id,
                Description = "Please turn off the AC",
                BuildingCode = "COH",
                CurrentStatus = Common.CurrentStatus.Created,
                CreatedBy = "Test User",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Test User",
                LastModifiedDate = DateTime.Now
            } };

            _serviceRequestService.Setup(service => service.GetServiceRequestById(id)).Returns(serviceRequestsList.Where(x => x.Id == id).FirstOrDefault());

            var response = _controller.GetById(id);
            var result = response.Result as OkObjectResult;

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ServiceRequestDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public void GetById_ShouldReturnEmptyContent_IfRequestNotFound()
        {
            Guid id = Guid.NewGuid();
            _serviceRequestService.Setup(service => service.GetServiceRequestById(id)).Returns((ServiceRequestDTO)null);

            var response = _controller.GetById(id);
            var result = response.Result as NotFoundResult;

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public void AddServiceRequest_AddsRequest_ReturnsRequest()
        {
            Guid id = Guid.NewGuid();
            var serviceRequest = new ServiceRequestDTO {
                Id = id,
                Description = "Please turn off the AC",
                BuildingCode = "COH",
                CurrentStatus = Common.CurrentStatus.Created,
                CreatedBy = "Test User",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Test User",
                LastModifiedDate = DateTime.Now
            };

            _serviceRequestService.Setup(service => service.AddServiceRequest(serviceRequest)).Returns(serviceRequest);

            var response = _controller.AddServiceRequest(serviceRequest);
            var result = response as CreatedAtActionResult;

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<ServiceRequestDTO>(createdResult.Value);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public void AddServiceRequest_WhenModelInvalid_ReturnsBadRequest()
        {
            Guid id = Guid.NewGuid();
            ServiceRequestDTO serviceRequest = null;

            _serviceRequestService.Setup(service => service.AddServiceRequest(serviceRequest)).Returns((ServiceRequestDTO)null);

            var response = _controller.AddServiceRequest(serviceRequest);
            var result = response as BadRequestObjectResult;

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateServiceRequest_ReturnsSuccess_IfValidData()
        {
            Guid id = Guid.NewGuid();
            var serviceRequest = new ServiceRequestDTO
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

            _serviceRequestService.Setup(service => service.UpdateServiceRequest(serviceRequest)).Returns(serviceRequest);

            var response = _controller.UpdateServiceRequest(id, serviceRequest);
            var result = response as OkObjectResult;

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ServiceRequestDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async Task UpdateServiceRequest_ReturnsBadRequest_IfInvalidData()
        {
            Guid id = Guid.NewGuid();
            Guid dummyId = Guid.NewGuid();
            var serviceRequest = new ServiceRequestDTO
            {
                Id = dummyId,
                Description = "Please turn off the AC",
                BuildingCode = "COH",
                CurrentStatus = Common.CurrentStatus.Created,
                CreatedBy = "Test User",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Test User",
                LastModifiedDate = DateTime.Now
            };

            _serviceRequestService.Setup(service => service.UpdateServiceRequest(serviceRequest)).Returns(serviceRequest);

            var response = _controller.UpdateServiceRequest(id, serviceRequest);
            var result = response as BadRequestResult;

            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task UpdateServiceRequest_ReturnsNotFound_IfRequestNotFound()
        {
            Guid id = Guid.NewGuid();
            var serviceRequest = new ServiceRequestDTO
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

            _serviceRequestService.Setup(service => service.UpdateServiceRequest(serviceRequest)).Returns((ServiceRequestDTO)null);

            var response = _controller.UpdateServiceRequest(id, serviceRequest);
            var result = response as NotFoundResult;

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteServiceRequest_ReturnsTrue_IfDeleted()
        {
            Guid id = Guid.NewGuid();
            _serviceRequestService.Setup(service => service.DeleteServiceRequest(id)).Returns(true);

            var response = _controller.DeleteServiceRequest(id);
            var result = response as OkResult;
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteServiceRequest_ReturnsFalse_IfRequestNotFound()
        {
            Guid id = Guid.NewGuid();
            _serviceRequestService.Setup(service => service.DeleteServiceRequest(id)).Returns(false);

            var response = _controller.DeleteServiceRequest(id);
            var result = response as NotFoundResult;

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
