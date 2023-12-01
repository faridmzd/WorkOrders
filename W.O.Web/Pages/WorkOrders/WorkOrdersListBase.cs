using FluentResults;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.ObjectModel;
using W.O.Web.Models;
using W.O.Web.Models.Requests.Create;
using W.O.Web.Pages.WorkOrders;
using W.O.Web.Services.Abstract;

namespace W.O.Web.Pages
{
    public class WorkOrdersListBase : ComponentBase
    {
        [Inject] 
        protected ISnackbar Snackbar { get; set; }

        [Inject]
        protected IWorkOrdersService _workOrdersService { get; set; }

        [Inject]
		protected IDialogService _dialogService { get; set; }

        [Inject]
        NavigationManager _navigationManager { get; set; }

        protected ObservableCollection<WorkOrderDTO> Orders = new ObservableCollection<WorkOrderDTO>();

        protected string ErrorMessage = string.Empty;

        protected Result<IEnumerable<WorkOrderDTO>> Result { get; set; } = new Result<IEnumerable<WorkOrderDTO>>();

        protected bool _readOnly = true;

        protected override async Task OnInitializedAsync()
        {
            Result = await _workOrdersService.GetAllAsync();

            if(Result.IsSuccess) Orders = new ObservableCollection<WorkOrderDTO>(Result.Value);
        }

        protected void RowClickedAsync(DataGridRowClickEventArgs<WorkOrderDTO> args)
        {
            _navigationManager.NavigateTo($"/workOrderDetails/{args.Item.Id}");
        }

        protected async Task CommittedItemChanges(WorkOrderDTO item)
        {
            await UpdateWorkOrderAsync(item);
        }

        protected async Task DeleteButtonClickedAsync(WorkOrderDTO item)
        {
            bool? result = await _dialogService.ShowMessageBox(
            "Warning",
            $"Sure about Deleting '{item.Title}' ?",
            yesText: "Delete", 
            cancelText: "Cancel",
            options: new DialogOptions { FullWidth = true, MaxWidth = MaxWidth.ExtraSmall });;

            if (result is true)
            {
                await DeleteWorkOrderAsync(item);
            }
        }

        private async Task DeleteWorkOrderAsync(WorkOrderDTO item)
        {
            var response = await _workOrdersService.DeleteAsync(item.Id);

            if (response.IsSuccess)
            {
               Orders.Remove(item);
               Snackbar.Add($"Work order '{item.Title}' removed!", Severity.Info);
            }
            else
            {
               Snackbar.Add($"Unable to remove work order '{item.Title}'!", Severity.Error);
               Snackbar.Add(response.Errors.Select(x => x.Message + " <br />").ToString(), Severity.Warning);

            }
        }


        private async Task UpdateWorkOrderAsync(WorkOrderDTO item)
        {
            var response = await _workOrdersService.UpdateAsync(item);

            if (response.IsSuccess)
            {
                Snackbar.Add("Work order updated!", Severity.Success);

                var updatedResponse = await _workOrdersService.GetByIdAsync(item.Id);

                if (updatedResponse.IsSuccess)
                {
                    Orders.Remove(item);
                    Orders.Insert(0, (WorkOrderDTO)updatedResponse.Value);
                }
            }
            else
            {
                Snackbar.Add("Couldn't update the work order, please try again!", Severity.Error);
            }
        }

        protected async Task AddWorkOrder()
		{
			var parameters = new DialogParameters();
			var dialogresult = _dialogService.Show<PostWorkOrderForm>("New Work order",parameters);

			var result = await dialogresult.Result;

			if (!result.Canceled)
            {
                var response = await _workOrdersService.AddAsync((CreateWorkOrderRequest)result.Data);

                if (response.IsSuccess) 
                {
                    if(Orders is null) { Orders = new ObservableCollection<WorkOrderDTO>();}

                    Orders.Insert(0,response.Value);
                    Snackbar.Add($"Work order with title: '{response.Value.Title}' added!",Severity.Success);
                    
                }
                else
                {
					Snackbar.Add("Couldn't Add the work order, please try again!", Severity.Error);
					Snackbar.Add(response.Errors.Select(x => x.Message + " <br />").ToString(), Severity.Warning);
				}
			}

		}
    }
}
