using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Controllers;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.PromoCodes;

public class CreateTests
{
    private readonly Mock<IRepository<PromoCode>> _promoCodesRepositoryMock;
    private readonly Mock<IRepository<Customer>> _customersRepositoryMock;
    private readonly Mock<IRepository<CustomerPromoCode>> _customerPromoCodesRepositoryMock;
    private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
    private readonly Mock<IRepository<Preference>> _preferencesRepositoryMock;
    private readonly PromoCodesController _sut;

    public CreateTests()
    {
        _promoCodesRepositoryMock = new Mock<IRepository<PromoCode>>();
        _customersRepositoryMock = new Mock<IRepository<Customer>>();
        _customerPromoCodesRepositoryMock = new Mock<IRepository<CustomerPromoCode>>();
        _partnersRepositoryMock = new Mock<IRepository<Partner>>();
        _preferencesRepositoryMock = new Mock<IRepository<Preference>>();
        _sut = new PromoCodesController(
            _promoCodesRepositoryMock.Object,
            _customersRepositoryMock.Object,
            _customerPromoCodesRepositoryMock.Object,
            _partnersRepositoryMock.Object,
            _preferencesRepositoryMock.Object);
    }

    [Fact]
    public async Task Create_WhenPartnerNotFound_ReturnsNotFound()
    {
    }

    [Fact]
    public async Task Create_WhenPreferenceNotFound_ReturnsNotFound()
    {
    }

    [Fact]
    public async Task Create_WhenNoActiveLimit_ReturnsUnprocessableEntity()
    {
    }

    [Fact]
    public async Task Create_WhenLimitExceeded_ReturnsUnprocessableEntity()
    {
    }

    [Fact]
    public async Task Create_WhenValidRequest_ReturnsCreatedAndIncrementsIssuedCount()
    {
    }
}
