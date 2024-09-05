using Microsoft.EntityFrameworkCore;
using ServiceRequestDemo.DatabaseAccess;
using ServiceRequestDemo.Repository.Interfaces;
using ServiceRequestDemo.DatabaseAccess.TableClasses;

namespace ServiceRequestDemo.Repository
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly ServiceRequestContext _dbContext;

        public ServiceRequestRepository(ServiceRequestContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ServiceRequest>? GetAll()
        {
            return _dbContext.ServiceRequests.ToList();
        }

        public ServiceRequest? GetById(Guid id)
        {
            return _dbContext.ServiceRequests.FirstOrDefault(x => x.Id == id);
        }

        public ServiceRequest Add(ServiceRequest serviceRequest)
        {
            _dbContext.ServiceRequests.Add(serviceRequest);
            _dbContext.SaveChanges();

            return serviceRequest;
        }

        public ServiceRequest? Update(ServiceRequest serviceRequest)
        {
            ServiceRequest? sr = _dbContext.ServiceRequests.FirstOrDefault(x => x.Id == serviceRequest.Id);
            if(sr != null)
            {
                sr.CurrentStatus = serviceRequest.CurrentStatus;
                sr.BuildingCode = serviceRequest.BuildingCode;
                sr.Description = serviceRequest.Description;
                sr.LastModifiedBy = serviceRequest.LastModifiedBy;
                sr.LastModifiedDate = serviceRequest.LastModifiedDate;
                _dbContext.SaveChanges();
            }

            return sr;
        }

        public bool Delete(Guid id)
        {
            ServiceRequest? sr = _dbContext.ServiceRequests.FirstOrDefault(x => x.Id == id);

            if (sr != null)
            {
                _dbContext.ServiceRequests.Remove(sr);
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
