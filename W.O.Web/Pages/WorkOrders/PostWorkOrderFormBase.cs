using Microsoft.AspNetCore.Components;
using MudBlazor;
using W.O.Web.Models.Requests.Create;
using W.O.Web.Validators;

namespace W.O.Web.Pages.WorkOrders
{
	public class PostWorkOrderFormBase : ComponentBase
	{
		[CascadingParameter]
		protected MudDialogInstance MudDialog {  get; set; }

		protected MudForm form;

		protected CreateWorkOrderRequestValidator orderValidator = new();

		protected CreateWorkOrderRequest model = new();

		protected async Task Submit()
		{
			await form.Validate();

			if (form.IsValid)
			{
		    MudDialog.Close(DialogResult.Ok(model));
			}
		}

		protected void Cancel()
		{
		MudDialog.Cancel();
		}

	}
}
