using AutoMapper;
using ServiceRequestDemo.DatabaseAccess.TableClasses;
using ServiceRequestDemo.Models;
using ServiceRequestDemo.Repository.Interfaces;
using ServiceRequestDemo.Service.Interfaces;

namespace ServiceRequestDemo.Service
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceRequestService> _logger;

        public ServiceRequestService(IServiceRequestRepository serviceRequestRepository, IMapper mapper, ILogger<ServiceRequestService> logger)
        {
            _serviceRequestRepository = serviceRequestRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public List<ServiceRequestDTO>? GetServiceRequests()
        {
            try
            {
                List<ServiceRequest>? requests = _serviceRequestRepository.GetAll();
                if (requests == null || requests.Count == 0)
                    return null;

                return _mapper.Map<List<ServiceRequestDTO>>(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public ServiceRequestDTO? GetServiceRequestById(Guid id)
        {
            try
            {
                ServiceRequest? request = _serviceRequestRepository.GetById(id);
                if (request == null)
                    return null;

                return _mapper.Map<ServiceRequestDTO>(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public bool DeleteServiceRequest(Guid id)
        {
            try
            {
                return _serviceRequestRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public ServiceRequestDTO? AddServiceRequest(ServiceRequestDTO serviceRequestDTO)
        {
            try
            {
                if(serviceRequestDTO.Id == null)
                    serviceRequestDTO.Id = Guid.NewGuid();

                serviceRequestDTO.CurrentStatus = Common.CurrentStatus.Created;
                serviceRequestDTO.CreatedDate = DateTime.Now;
                serviceRequestDTO.LastModifiedBy = serviceRequestDTO.CreatedBy;
                serviceRequestDTO.LastModifiedDate = DateTime.Now;

                ServiceRequest req = _mapper.Map<ServiceRequest>(serviceRequestDTO);

                _serviceRequestRepository.Add(req);
                return serviceRequestDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
            
        }

        public ServiceRequestDTO? UpdateServiceRequest(ServiceRequestDTO serviceRequestDTO)
        {
            try
            {
                serviceRequestDTO.LastModifiedDate = DateTime.Now;
                ServiceRequest req = _mapper.Map<ServiceRequest>(serviceRequestDTO);
                ServiceRequest? request = _serviceRequestRepository.Update(req);
                if (request == null)
                    return null;

                return serviceRequestDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
