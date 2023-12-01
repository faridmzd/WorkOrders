using FluentResults;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.ObjectModel;
using W.O.Web.Models;
using W.O.Web.Models.Requests.Create;
using W.O.Web.Pages.Visits;
using W.O.Web.Pages.WorkOrders;
using W.O.Web.Services.Abstract;
using W.O.Web.Services.Concrete;

namespace W.O.Web.Pages
{
    public class WorkOrderDetailsBase : ComponentBase
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        protected ISnackbar Snackbar { get; set; }

        [Inject]
        public IWorkOrdersService _workOrdersService { get; set; }

        [Inject]
        public IVisitsService _visitsService { get; set; }

        [Inject]
        public IPartsService _partsService { get; set; }

        [Inject]
        protected IDialogService _dialogService { get; set; }

        protected ObservableCollection<VisitDTO> Visits = new ObservableCollection<VisitDTO>();

        protected Result<WorkOrderDetailsDTO> Result { get; set; } = new Result<WorkOrderDetailsDTO>();

        protected WorkOrderDetailsDTO WorkOrderDetails {  get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // parallel task

            Result = await _workOrdersService.GetByIdAsync(Id);

            if (Result.IsSuccess) 
            {
                this.Visits = new ObservableCollection<VisitDTO>(Result.Value.Visits);

                //Visits.SelectMany(v => v.Parts = v.Parts.ToList());
            }
        }

        protected async Task CommittedVisitChanges(VisitDTO item)
        {
            await UpdateVisitAsync(item);
        }

        private async Task UpdateVisitAsync(VisitDTO item)
        {
            var response = await _visitsService.UpdateAsync(item);

            if (response.IsSuccess)
            {
                Snackbar.Add("Visit updated!", Severity.Success);

                var updatedResponse = await _visitsService.GetByIdAsync(item.Id);

                if (updatedResponse.IsSuccess)
                {
                    Visits.Remove(item);
                    Visits.Insert(0, updatedResponse.Value);
                }
            }
            else
            {
                Snackbar.Add("Couldn't update the visit, please try again!", Severity.Error);
                Snackbar.Add(response.Errors.Select(x => x.Message + " <br />").ToString(), Severity.Warning);
            }
        }

        protected async Task CommittedPartChanges(PartDTO item)
        {
            await UpdatePartAsync(item);
        }

        private async Task UpdatePartAsync(PartDTO item)
        {
            var response = await _partsService.UpdateAsync(item);

            if (response.IsSuccess)
            {
                Snackbar.Add("Part updated!", Severity.Success);

                var updatedResponse = await _partsService.GetByIdAsync(item.Id);

                if (updatedResponse.IsSuccess)
                {
                    var parts = (List<PartDTO>)Visits.FirstOrDefault(v => v.Id == item.VisitId)!.Parts;
                    parts.Remove(item);
                    parts.Insert(0, updatedResponse.Value);
                }
            }
            else
            {
                Snackbar.Add("Couldn't update the work order, please try again!", Severity.Error);
                Snackbar.Add(response.Errors.Select(x => x.Message + " <br />").ToString(), Severity.Warning);
            }
        }

        protected async Task DeleteVisitButtonClickedAsync(VisitDTO item)
        {
            bool? result = await _dialogService.ShowMessageBox(
            "Warning",
            "Sure about Deleting the Visit ?",
            yesText: "Delete",
            cancelText: "Cancel",
            options: new DialogOptions { FullWidth = true, MaxWidth = MaxWidth.ExtraSmall }); ;

            if (result is true)
            {
                await DeleteVisitAsync(item);
            }
        }

        private async Task DeleteVisitAsync(VisitDTO item)
        {
            var response = await _visitsService.DeleteAsync(item.Id);

            if (response.IsSuccess)
            {
                Visits.Remove(item);
                Snackbar.Add($"The Visit removed!", Severity.Info);
            }
            else
            {
                Snackbar.Add("Unable to remove the Visit!", Severity.Error);
                Snackbar.Add(response.Errors.Select(x => x.Message + " <br />").ToString(), Severity.Warning);

            }
        }

        protected async Task DeletePartButtonClickedAsync(PartDTO item)
        {
            bool? result = await _dialogService.ShowMessageBox(
            "Warning",
            "Sure about Deleting the Part ?",
            yesText: "Delete",
            cancelText: "Cancel",
            options: new DialogOptions { FullWidth = true, MaxWidth = MaxWidth.ExtraSmall }); ;

            if (result is true)
            {
                await DeletePartAsync(item);
            }
        }

        private async Task DeletePartAsync(PartDTO item)
        {
            var response = await _partsService.DeleteAsync(item.Id);

            if (response.IsSuccess)
            {
                var parts = (List<PartDTO>)Visits.FirstOrDefault(v => v.Id == item.VisitId)!.Parts;
                parts.Remove(item);
                Snackbar.Add($"The Part removed!", Severity.Info);
            }
            else
            {
                Snackbar.Add("Unable to remove the Part!", Severity.Error);
                Snackbar.Add(response.Errors.Select(x => x.Message + " <br />").ToString(), Severity.Warning);

            }
        }

        protected async Task AddVisit()
        {
            var parameters = new DialogParameters();
            var dialogresult = _dialogService.Show<PostVisitForm>("New Visit", parameters);

            var result = await dialogresult.Result;

            if (!result.Canceled)
            {
                var request = (CreateVisitRequest)result.Data;
                request.WorkOrderId = Id;

				var response = await _visitsService.AddAsync(request);

                if (response.IsSuccess)
                {
                    if (Visits is null) { Visits = new ObservableCollection<VisitDTO>(); }

                    Visits.Insert(0, response.Value);
                    Snackbar.Add("Visit added!", Severity.Success);

                }
				else
				{
					Snackbar.Add("Couldn't Add the Visit, please try again!", Severity.Error);
					Snackbar.Add(response.Errors.Select(x => x.Message + " <br />").ToString(), Severity.Warning);
				}
			}

        }

		protected async Task AddPart(Guid visitId)
		{
			var parameters = new DialogParameters();
			var dialogresult = _dialogService.Show<PostVisitForm>("New Part", parameters);

			var result = await dialogresult.Result;

			if (!result.Canceled)
			{
				var request = (CreatePartRequest)result.Data;
				request.VisitId = visitId;

				var response = await _partsService.AddAsync(request);

				if (response.IsSuccess)
				{
					if (Visits is null) { Visits = new ObservableCollection<VisitDTO>(); }

                    var parts = (List<PartDTO>)Visits.FirstOrDefault(v => v.Id == visitId)!.Parts;
                    parts.Add(response.Value);
					Snackbar.Add("Part added!", Severity.Success);

				}
				else
				{
					Snackbar.Add("Couldn't Add the Part, please try again!", Severity.Error);
					Snackbar.Add(response.Errors.Select(x => x.Message + " <br />").ToString(), Severity.Warning);
				}
			}

		}

	}
}
