using Microsoft.AspNetCore.Components;
using MudBlazor;
using W.O.Web.Models;
using W.O.Web.Models.Requests.Create;
using W.O.Web.Validators;

namespace W.O.Web.Pages.Visits
{
    public class PostVisitFormBase : ComponentBase
    {
        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; }

        protected MudForm form;

        protected CreateVisitRequestValidator visitValidator = new();

        protected VisitFormDTO visit = new VisitFormDTO();

		protected async Task Submit()
        {
            await form.Validate();

            if (form.IsValid)
            {
                MudDialog.Close(DialogResult.Ok((CreateVisitRequest)visit));
            }
        }

        protected void Cancel()
        {
            MudDialog.Cancel();
        }

    }
}
