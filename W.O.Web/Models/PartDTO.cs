using W.O.Web.Models.Requests.Create;

namespace W.O.Web.Models
{
	public class PartDTO
	{
		public Guid Id { get; set; }
		public Guid VisitId { get; set; }
		public string Description { get; set; }
		public decimal Amount { get; set; }
		public string Currency { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPrice { get; set; }

	}
}
