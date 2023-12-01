using Microsoft.AspNetCore.Components;
using MudBlazor;
using W.O.Web.Models;
using W.O.Web.Models.Requests.Create;
using W.O.Web.Validators;

namespace W.O.Web.Pages.Parts
{
	public class PostPartFormBase : ComponentBase
	{
		[CascadingParameter]
		protected MudDialogInstance MudDialog { get; set; }

		protected MudForm form;

		protected CreatePartRequestValidator partValidator = new();

		protected CreatePartRequest part = new ();

		protected async Task Submit()
		{
			await form.Validate();

			if (form.IsValid)
			{
				MudDialog.Close(DialogResult.Ok(part));
			}
		}

		protected void Cancel()
		{
			MudDialog.Cancel();
		}
	}
}
