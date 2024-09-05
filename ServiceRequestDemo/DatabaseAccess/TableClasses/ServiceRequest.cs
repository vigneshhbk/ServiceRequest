using ServiceRequestDemo.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceRequestDemo.DatabaseAccess.TableClasses
{
    [Table("ServiceRequest")]
    public class ServiceRequest
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? BuildingCode { get; set; }
        [Required]
        public string? Description { get; set; }
        public int CurrentStatus { get; set; }
        [Required]
        public string? CreatedBy { get; set; }
        [Required]
        public DateTime? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
