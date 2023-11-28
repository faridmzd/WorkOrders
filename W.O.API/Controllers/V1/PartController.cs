using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using W.O.API.Contracts.V1;
using W.O.API.Contracts.V1.Parts;
using W.O.API.Data.Repositories.Abstract;
using W.O.API.Domain;
using W.O.API.Domain.Common.Helpers;

namespace W.O.API.Controllers.V1
{
    public class PartController : BaseController
    {
        private readonly IPartRepository _partRepo;
        private readonly IVisitRepository _visitRepo;

        public PartController(IPartRepository partRepo, IVisitRepository visitRepo)
        {
            _partRepo = partRepo;
            _visitRepo = visitRepo;
        }

        [HttpGet(ApiRoutes.Currencies.GetAll)]
        public IActionResult GetCurrencies()
        {
            return Ok(CurrencyHelper.GetCurrencyNames());
        }

        [HttpGet(ApiRoutes.Parts.GetAll)]
        public async Task<IActionResult> GetPartsAsync()
        {
            var part = await _partRepo.GetAllAsync();
            return Ok(part.Select(p => (GetPartResponse)p));
        }

        [HttpGet(ApiRoutes.Parts.Get)]
        public async Task<IActionResult> GetPartByIdAsync([FromRoute] Guid id)
        {
            var part = await _partRepo.GetByIdAsync(id);

            return part != null
                 ? Ok((GetPartResponse)part)
                 : NotFound($"Part with given id: {id} does not exist!");
        }

        [HttpPost(ApiRoutes.Parts.Add)]
        public async Task<IActionResult> AddPartAsync([FromBody] CreatePartRequest request)
        {
            var currentVisitPartCount = (await _visitRepo.GetByIdAsync(request.visitId))?.TotalParts;

            if (currentVisitPartCount is null)
            {
                return NotFound($"Visit with given id : {request.visitId} does not exist!");
            }
            if (currentVisitPartCount >= 5)
            {
                return Conflict("Visit can have max 5 parts.");
            }

            var newPart = (Part)request;

            var addedPart = await _partRepo.AddAsync(newPart);

            return Ok((CreatePartResponse)addedPart);
        }

        [HttpDelete(ApiRoutes.Parts.Delete)]
        public async Task<IActionResult> DeletePartAsync([FromRoute] Guid id)
        {
            var deletedRows = await _partRepo.DeleteAsync(id);

            if (deletedRows == 0) return NotFound($"Part with given id: {id} does not exists!");

            return NoContent();
        }

        [HttpPut(ApiRoutes.Parts.Update)]
        public async Task<IActionResult> UpdatePartAsync([FromRoute] Guid id, [FromBody] UpdatePartRequest request)
        {
            var part = await _partRepo.GetByIdAsync(id);

            if (part == null) return NotFound($"Part with given id: {id} does not exists!");

            var partToUpdate = part.Update(request.description, request.amount, request.currency, request.quantity);

            await _partRepo.UpdateAsync(partToUpdate);

            return NoContent();
        }
    }
}
