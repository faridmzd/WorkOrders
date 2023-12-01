using FluentValidation;

namespace W.O.Web.Models.Requests.Create
{
    public class CreateWorkOrderRequest
    {
		public string Title { get; set; }
        public string Description { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
        public DateTime? StartAt { get; set; } = default(DateTime);
        public DateTime? FinishAt { get; set; } = default(DateTime);
	}

    public class CreateVisitRequest
    {
        public Guid WorkOrderId { get; set; }
        public string AssigneeFullName { get; set; }
        public DateTime? AssignedFrom { get; set; } = default(DateTime);
        public ICollection<CreatePartRequest> Parts { get; set; } = new List<CreatePartRequest>();
    }


    public class CreatePartRequest
    {
        public Guid VisitId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public string Currency {  get; set; }
    }
}
