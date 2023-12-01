namespace W.O.Web.Models
{
    public class VisitDTO
    {
        public Guid Id { get; set; }
        public Guid WorkOrderId { get; set; } 
        public string AssigneeFullName { get; set; }
        public DateTime AssignedFrom { get; set; } 
        public IEnumerable<PartDTO> Parts { get; set; } = new List<PartDTO>();
    }
}
