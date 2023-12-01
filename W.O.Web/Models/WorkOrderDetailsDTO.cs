namespace W.O.Web.Models
{
    public class WorkOrderDetailsDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime FinishAt { get; set; }
        public IEnumerable<VisitDTO> Visits { get; set; } = new List<VisitDTO>();
    }
}
