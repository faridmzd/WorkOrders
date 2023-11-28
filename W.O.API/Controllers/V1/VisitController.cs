using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using W.O.API.Contracts.V1;
using W.O.API.Contracts.V1.Visits;
using W.O.API.Contracts.V1.WorkOrders;
using W.O.API.Data.Repositories.Abstract;
using W.O.API.Domain;
using W.O.API.Domain.Common.Helpers;

namespace W.O.API.Controllers.V1
{
    public class VisitController : BaseController
    {
        private readonly IVisitRepository _visitRepo;
        private readonly IWorkOrderRepository _workOrderRepo;
        public VisitController(IVisitRepository visitRepo, IWorkOrderRepository workOrderRepo)
        {
            _visitRepo = visitRepo;
            _workOrderRepo = workOrderRepo;
        }

        [HttpGet(ApiRoutes.Visits.GetAll)]
        public async Task<IActionResult> GetVisitsAsync()
        {
            var visit = await _visitRepo.GetAllAsync();

            return Ok(visit.Select(v => (GetVisitResponse)v));
        }

        [HttpGet(ApiRoutes.Visits.Get)]
        public async Task<IActionResult> GetVisitByIdAsync([FromRoute] Guid id)
        {
            var visit = await _visitRepo.GetByIdAsync(id);

            return visit != null
                 ? Ok((GetVisitResponse)visit)
                 : NotFound($"Visit with given id: {id} does not exist!");
        }

        [HttpPost(ApiRoutes.Visits.Add)]
        public async Task<IActionResult> AddVisitAsync([FromBody] CreateVisitRequest request,
            [FromServices] IValidator<CreateVisitRequest>? validator)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(validator));

            ValidationResult validationResult = validator!.Validate(request);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);

                return ValidationProblem();
            }

            var currentWorkOrderVisitCount = (await _workOrderRepo.GetByIdAsync(request.workOrderId))?.TotalVisits;

            if (currentWorkOrderVisitCount is null)
            {
                return NotFound($"Work order with given id : {request.workOrderId} does not exist!");
            }
            if (currentWorkOrderVisitCount >= 3 )
            {
                return Conflict("Work order can have max 3 visits.");
            }
            
            var newVisit = (Visit)request;

            var addedVisit = await _visitRepo.AddAsync(newVisit);

            return Ok((CreateVisitResponse)addedVisit);
        }

        [HttpDelete(ApiRoutes.Visits.Delete)]
        public async Task<IActionResult> DeleteVisitAsync([FromRoute] Guid id)
        {
            var deletedRows = await _visitRepo.DeleteAsync(id);

            if (deletedRows == 0) return NotFound($"Visit with given id: {id} does not exist!");

            return NoContent();
        }

        [HttpPut(ApiRoutes.Visits.Update)]
        public async Task<IActionResult> UpdateVisitAsync([FromRoute] Guid id, [FromBody] UpdateVisitRequest request,
            [FromServices] IValidator<UpdateVisitRequest>? validator)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(validator));

            ValidationResult validationResult = validator!.Validate(request);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);

                return ValidationProblem();
            }

            var visit = await _visitRepo.GetByIdAsync(id);

            if (visit == null) return NotFound($"Visit with given id: {id} does not exist!");

            var visitToUpdate = visit.Update(request.assigneeFullName,
                DateTime.TryParse(request.assignedFrom, out DateTime sResult) == true ? sResult : null);

            await _visitRepo.UpdateAsync(visitToUpdate);

            return NoContent();

        }
    }
}
