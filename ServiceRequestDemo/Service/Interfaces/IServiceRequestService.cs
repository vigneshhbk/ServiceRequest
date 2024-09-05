using ServiceRequestDemo.Models;

namespace ServiceRequestDemo.Service.Interfaces
{
    public interface IServiceRequestService
    {
        List<ServiceRequestDTO>? GetServiceRequests();
        ServiceRequestDTO? GetServiceRequestById(Guid id);
        bool DeleteServiceRequest(Guid id);
        ServiceRequestDTO? AddServiceRequest(ServiceRequestDTO serviceRequestDTO);
        ServiceRequestDTO? UpdateServiceRequest(ServiceRequestDTO serviceRequestDTO);
    }
}
