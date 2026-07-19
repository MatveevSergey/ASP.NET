using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Mapping;
using PromoCodeFactory.WebHost.Models.PromoCodes;

namespace PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Промокоды
/// </summary>
public class PromoCodesController(
    IRepository<PromoCode> promoCodeRepository,
    IRepository<Employee> employeeRepository,
    IRepository<Preference> preferenceRepository,
    IRepository<Customer> customerRepository,
    IRepository<CustomerPromoCode> customerPromoCodeRepository)
    : BaseController
{
    /// <summary>
    /// Получить все промокоды
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PromoCodeShortResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PromoCodeShortResponse>>> Get(CancellationToken ct)
    {
        var promoCodes = await promoCodeRepository.GetAll(withIncludes: true, ct: ct);

        var promoCodesModels = promoCodes.Select(PromoCodesMapper.ToPromoCodeShortResponse).ToList();

        return Ok(promoCodesModels);
    }

    /// <summary>
    /// Получить промокод по id
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PromoCodeShortResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PromoCodeShortResponse>> GetById(Guid id, CancellationToken ct)
    {
        var promoCode = await promoCodeRepository.GetById(id, withIncludes: true, ct: ct);
        if (promoCode is null)
            return NotFound(new ProblemDetails
            {
                Title = "Promo code not found",
                Detail = $"Promo code with id '{id}' was not found."
            });

        return Ok(PromoCodesMapper.ToPromoCodeShortResponse(promoCode));
    }

    /// <summary>
    /// Создать промокод и выдать его клиентам с указанным предпочтением
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PromoCodeShortResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PromoCodeShortResponse>> Create(PromoCodeCreateRequest request, CancellationToken ct)
    {
        var partnerManager = await employeeRepository.GetById(request.PartnerManagerId, ct: ct);
        if (partnerManager is null)
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid partner manager",
                Detail = $"Employee with Id {request.PartnerManagerId} not found."
            });

        var preference = await preferenceRepository.GetById(request.PreferenceId, ct: ct);
        if (preference is null)
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid preference",
                Detail = $"Preference with Id {request.PreferenceId} not found."
            });

        var customers = await customerRepository.GetWhere(
            c => c.Preferences.Any(p => p.Id == request.PreferenceId),
            ct: ct);

        var promoCode = PromoCodesMapper.ToPromoCode(request, partnerManager, preference, customers);
        await promoCodeRepository.Add(promoCode, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = promoCode.Id },
            PromoCodesMapper.ToPromoCodeShortResponse(promoCode));
    }

    /// <summary>
    /// Применить промокод (отметить, что клиент использовал промокод)
    /// </summary>
    [HttpPost("{id:guid}/apply")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Apply(
        [FromRoute] Guid id,
        [FromBody] PromoCodeApplyRequest request,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
