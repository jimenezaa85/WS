using System;

namespace TaxRay.Model
{
    public class TaxReturn
    {
        public int? Id { get; set; }
        public string AtlasId { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }
        public string TaxPayer { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public int Year { get; set; }
        public int? AssignedToId { get; set; }
        public virtual User AssignedTo { get; set; }
        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
    }
}