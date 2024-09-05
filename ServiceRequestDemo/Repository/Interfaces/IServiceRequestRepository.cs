using ServiceRequestDemo.DatabaseAccess.TableClasses;

namespace ServiceRequestDemo.Repository.Interfaces
{
    public interface IServiceRequestRepository
    {
        List<ServiceRequest>? GetAll();
        ServiceRequest? GetById(Guid id);
        ServiceRequest Add(ServiceRequest serviceRequest);
        ServiceRequest? Update(ServiceRequest serviceRequest);
        bool Delete(Guid id);
    }
}
