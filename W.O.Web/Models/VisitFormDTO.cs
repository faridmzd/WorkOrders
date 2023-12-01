using W.O.Web.Models.Requests.Create;

namespace W.O.Web.Models
{
	public class VisitFormDTO
	{
		public string AssigneeFullName { get; set; }
		public DateTime? AssignedFrom { get; set; } = default(DateTime);

        public CreatePartRequest Part { get; set; } = new CreatePartRequest();

		public static explicit operator CreateVisitRequest(VisitFormDTO dto)
		{
			var request =  new CreateVisitRequest
			{
				AssigneeFullName = dto.AssigneeFullName,
				AssignedFrom = dto.AssignedFrom,
			};

			request.Parts.Add(dto.Part);

			return request;
		} 
    }
}
