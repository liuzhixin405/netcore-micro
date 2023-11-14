using System.Globalization;
using Carts.Api.Requests;
using Carts.ShoppingCarts;
using Carts.ShoppingCarts.GettingCartById;
using Carts.ShoppingCarts.Products;
using FluentAssertions;
using Ogooreck.API;
using Xunit;
using static Ogooreck.API.ApiSpecification;

namespace Carts.Api.Tests.ShoppingCarts.RemovingProduct;

public class RemoveProductFixture: ApiSpecification<Program>, IAsyncLifetime
{
    public Guid ShoppingCartId { get; private set; }

    public readonly Guid ClientId = Guid.NewGuid();

    public readonly ProductItemRequest ProductItem = new(Guid.NewGuid(), 10);

    public decimal UnitPrice;

    public async Task InitializeAsync()
    {
        var cartDetails = await Given()
            .When(POST, URI("/api/ShoppingCarts"), BODY(new OpenShoppingCartRequest(ClientId)))
            .Then(CREATED_WITH_DEFAULT_HEADERS(eTag: 1))
            .And()
            .When(
                POST,
                URI(ctx => $"/api/ShoppingCarts/{ctx.GetCreatedId()}/products"),
                BODY(new AddProductRequest(ProductItem)),
                HEADERS(IF_MATCH(1))
            )
            .Then(OK)
            .And()
            .When(GET, URI(ctx => $"/api/ShoppingCarts/{ctx.GetCreatedId()}"))
            .Until(RESPONSE_ETAG_IS(2))
            .Then(OK)
            .GetResponseBody<ShoppingCartDetails>();

        ShoppingCartId = cartDetails.Id;
        UnitPrice = cartDetails.ProductItems.Single().UnitPrice;
    }

    public Task DisposeAsync() => Task.CompletedTask;
}

public class RemoveProductTests: IClassFixture<RemoveProductFixture>
{
    private readonly RemoveProductFixture API;

    public RemoveProductTests(RemoveProductFixture api) => API = api;

    [Fact]
    [Trait("Category", "Acceptance")]
    public Task Delete_Should_Return_OK_And_Cancel_Shopping_Cart() =>
        API
            .Given()
            .When(
                DELETE,
                URI(
                    $"/api/ShoppingCarts/{API.ShoppingCartId}/products/{API.ProductItem.ProductId}?quantity={RemovedCount}&unitPrice={API.UnitPrice.ToString(CultureInfo.InvariantCulture)}"),
                HEADERS(IF_MATCH(2))
            )
            .Then(NO_CONTENT)
            .And()
            .When(GET, URI($"/api/ShoppingCarts/{API.ShoppingCartId}"))
            .Until(RESPONSE_ETAG_IS(3))
            .Then(
                OK,
                RESPONSE_BODY<ShoppingCartDetails>(details =>
                {
                    details.Id.Should().Be(API.ShoppingCartId);
                    details.Status.Should().Be(ShoppingCartStatus.Pending);
                    details.ProductItems.Should().HaveCount(1);
                    var productItem = details.ProductItems.Single();
                    productItem.Should().BeEquivalentTo(
                        PricedProductItem.Create(
                            ProductItem.From
                            (
                                API.ProductItem.ProductId!.Value,
                                API.ProductItem.Quantity!.Value - RemovedCount
                            ),
                            API.UnitPrice
                        ));
                    details.ClientId.Should().Be(API.ClientId);
                    details.Version.Should().Be(3);
                }));

    private readonly int RemovedCount = 5;
}
