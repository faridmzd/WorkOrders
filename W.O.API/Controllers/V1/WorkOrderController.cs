using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using W.O.API.Contracts.V1;
using W.O.API.Contracts.V1.WorkOrders;
using W.O.API.Data.Repositories.Abstract;
using W.O.API.Domain;
using FluentValidation.Results;
using W.O.API.Domain.Common.Helpers;

namespace W.O.API.Controllers.V1
{
    public class WorkOrderController : BaseController
    {
        private readonly IWorkOrderRepository _workOrderRepo;
        public WorkOrderController(IWorkOrderRepository workOrderRepo)
        {
            _workOrderRepo = workOrderRepo;
        }

        [HttpGet(ApiRoutes.WorkOrders.GetAll)]
        public async Task<IActionResult> GetWorkOrdersAsync()
        {
            var order = await _workOrderRepo.GetAllAsync();
            return Ok(order.Select(o => (GetWorkOrderResponse)o));
        }

        [HttpGet(ApiRoutes.WorkOrders.Get)]
        public async Task<IActionResult> GetWorkOrderByIdAsync([FromRoute] Guid id)
        {
            var order = await _workOrderRepo.GetByIdAsync(id);

            return order != null
                 ? Ok((GetWorkOrderResponse)order)
                 : Problem($"Work order with given id: {id} does not exist!");
        }

        [HttpPost(ApiRoutes.WorkOrders.Add)]
        public async Task<IActionResult> AddWorkOrderAsync([FromBody] CreateWorkOrderRequest request,
            [FromServices] IValidator<CreateWorkOrderRequest>? validator)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(validator));

            ValidationResult validationResult = validator!.Validate(request);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);

                return ValidationProblem();
            }

            var newWorkOrder = (WorkOrder)request;

            var addedWorkOrder = await _workOrderRepo.AddAsync(newWorkOrder);

            return Ok((CreateWorkOrderResponse)addedWorkOrder);
        }

        [HttpDelete(ApiRoutes.WorkOrders.Delete)]
        public async Task<IActionResult> DeleteWorkOrderAsync([FromRoute] Guid id)
        {
            var deletedRows = await _workOrderRepo.DeleteAsync(id);

            if (deletedRows == 0) return NotFound($"Work order with given id: {id} does not exists!");

            return NoContent();
        }

        [HttpPut(ApiRoutes.WorkOrders.Update)]
        public async Task<IActionResult> UpdateWorkOrderAsync([FromRoute] Guid id, [FromBody] UpdateWorkOrderRequest request,
            [FromServices] IValidator<UpdateWorkOrderRequest>? validator)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(validator));

            ValidationResult validationResult = validator!.Validate(request);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);

                return ValidationProblem();
            }

            var order = await _workOrderRepo.GetByIdAsync(id);

            if (order == null) return NotFound($"Work order with given id: {id} does not exists!");

            var orderToUpdate = order.Update(
                request.title, 
                request.description, 
                request.phone, 
                request.email,
                DateTime.TryParse(request.startAt, out DateTime sResult) == true ? sResult : null,
                DateTime.TryParse(request.finishAt, out DateTime fResult) == true ? fResult : null);

            await _workOrderRepo.UpdateAsync(orderToUpdate);

            return NoContent();

        }
    }
}
