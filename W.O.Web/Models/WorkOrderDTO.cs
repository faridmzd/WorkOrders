namespace W.O.Web.Models
{
    public class WorkOrderDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime FinishAt { get; set; }
        public int TotalVisits { get; set; }
        public int TotalParts { get; set; }

        public static explicit operator WorkOrderDTO(WorkOrderDetailsDTO workOrderDetailsDTO)
        {
            var totalVisits = workOrderDetailsDTO.Visits?.Count() ?? 0;
            var totalParts = workOrderDetailsDTO.Visits?.Sum(x => x.Parts?.Count() ?? 0) ?? 0;

                return new WorkOrderDTO 
           { 
               Id = workOrderDetailsDTO.Id,
               Title = workOrderDetailsDTO.Title,
               Description = workOrderDetailsDTO.Description,
               Phone = workOrderDetailsDTO.Phone,
               Email = workOrderDetailsDTO.Email,
               TotalParts = totalParts,
               TotalVisits = totalVisits,
               StartAt = workOrderDetailsDTO.StartAt,
               FinishAt = workOrderDetailsDTO.FinishAt,
           };
        }
    }
}
