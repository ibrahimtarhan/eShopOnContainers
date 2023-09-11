namespace UnitTest.Ordering.Application;

using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Queries;
using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

public class OrdersWebApiTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IOrderQueries> _orderQueriesMock;
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly Mock<ILogger<OrdersController>> _loggerMock;

    public OrdersWebApiTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _orderQueriesMock = new Mock<IOrderQueries>();
        _identityServiceMock = new Mock<IIdentityService>();
        _loggerMock = new Mock<ILogger<OrdersController>>();
    }

    [Fact]
    public async Task Cancel_order_with_requestId_success()
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CancelOrderCommand, bool>>(), default))
            .Returns(Task.FromResult(true));

        //Act
        var orderController = new OrdersController(_mediatorMock.Object, _orderQueriesMock.Object, _identityServiceMock.Object, _loggerMock.Object);
        var actionResult = await orderController.CancelOrderAsync(new CancelOrderCommand(1), Guid.NewGuid().ToString()) as OkResult;

        //Assert
        Assert.Equal((int)System.Net.HttpStatusCode.OK, actionResult.StatusCode);

    }

    [Fact]
    public async Task Cancel_order_bad_request()
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CancelOrderCommand, bool>>(), default))
            .Returns(Task.FromResult(true));

        //Act
        var orderController = new OrdersController(_mediatorMock.Object, _orderQueriesMock.Object, _identityServiceMock.Object, _loggerMock.Object);
        var actionResult = await orderController.CancelOrderAsync(new CancelOrderCommand(1), string.Empty) as BadRequestResult;

        //Assert
        Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, actionResult.StatusCode);
    }

    [Fact]
    public async Task Ship_order_with_requestId_success()
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<ShipOrderCommand, bool>>(), default))
            .Returns(Task.FromResult(true));

        //Act
        var orderController = new OrdersController(_mediatorMock.Object, _orderQueriesMock.Object, _identityServiceMock.Object, _loggerMock.Object);
        var actionResult = await orderController.ShipOrderAsync(new ShipOrderCommand(1), Guid.NewGuid().ToString()) as OkResult;

        //Assert
        Assert.Equal((int)System.Net.HttpStatusCode.OK, actionResult.StatusCode);

    }

    [Fact]
    public async Task Ship_order_bad_request()
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CreateOrderCommand, bool>>(), default))
            .Returns(Task.FromResult(true));

        //Act
        var orderController = new OrdersController(_mediatorMock.Object, _orderQueriesMock.Object, _identityServiceMock.Object, _loggerMock.Object);
        var actionResult = await orderController.ShipOrderAsync(new ShipOrderCommand(1), string.Empty) as BadRequestResult;

        //Assert
        Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, actionResult.StatusCode);
    }

    [Fact]
    public async Task Get_orders_success()
    {
        //Arrange
        var fakeDynamicResult = Enumerable.Empty<OrderSummary>();

        _identityServiceMock.Setup(x => x.GetUserIdentity())
            .Returns(Guid.NewGuid().ToString());

        _orderQueriesMock.Setup(x => x.GetOrdersFromUserAsync(Guid.NewGuid()))
            .Returns(Task.FromResult(fakeDynamicResult));

        //Act
        var orderController = new OrdersController(_mediatorMock.Object, _orderQueriesMock.Object, _identityServiceMock.Object, _loggerMock.Object);
        var actionResult = await orderController.GetOrdersAsync();

        //Assert
        Assert.Equal((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_order_success()
    {
        //Arrange
        var fakeOrderId = 123;
        var fakeDynamicResult = new Microsoft.eShopOnContainers.Services.Ordering.API.Application.Queries.Order();
        _orderQueriesMock.Setup(x => x.GetOrderAsync(It.IsAny<int>()))
            .Returns(Task.FromResult(fakeDynamicResult));

        //Act
        var orderController = new OrdersController(_mediatorMock.Object, _orderQueriesMock.Object, _identityServiceMock.Object, _loggerMock.Object);
        var actionResult = await orderController.GetOrderAsync(fakeOrderId);

        //Assert
        Assert.Same(actionResult.Value, fakeDynamicResult);
    }

    [Fact]
    public async Task Get_cardTypes_success()
    {
        //Arrange
        var fakeDynamicResult = Enumerable.Empty<CardType>();
        _orderQueriesMock.Setup(x => x.GetCardTypesAsync())
            .Returns(Task.FromResult(fakeDynamicResult));

        //Act
        var orderController = new OrdersController(_mediatorMock.Object, _orderQueriesMock.Object, _identityServiceMock.Object, _loggerMock.Object);
        var actionResult = await orderController.GetCardTypesAsync();

        //Assert
        Assert.Equal((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Set_complete_statu_order_succes()
    {
        //Arrange
        
        var fakeDynamicResult = Enumerable.Empty<OrderSummary>();
        _orderQueriesMock.Setup(x => x.GetAllOrdersAsync())
            .Returns(Task.FromResult(fakeDynamicResult));
        var regularOrder = fakeDynamicResult.FirstOrDefault(c => c.status.Equals(OrderStatus.Shipped.Name));

        _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CompleteOrderCommand, bool>>(), default))
            .Returns(Task.FromResult(true));

        //Act
        var orderController = new OrdersController(_mediatorMock.Object, _orderQueriesMock.Object, _identityServiceMock.Object, _loggerMock.Object);
        var actionResult = await orderController.CompleteOrderAsync(new CompleteOrderCommand(regularOrder.ordernumber), Guid.NewGuid().ToString());

        //Assert
        Assert.True(actionResult.Value); //Expecting True

    }

    [Fact]
    public async Task Set_complete_statu_order_fail()
    {
        //Arrange

        var fakeDynamicResult = Enumerable.Empty<OrderSummary>();
        _orderQueriesMock.Setup(x => x.GetAllOrdersAsync())
            .Returns(Task.FromResult(fakeDynamicResult));
        var irregularOrder = fakeDynamicResult.FirstOrDefault(c => c.status == null || c.status !=(OrderStatus.Shipped.Name));

        _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CompleteOrderCommand, bool>>(), default))
           .Returns(Task.FromResult(true));

        //Act
        var orderController = new OrdersController(_mediatorMock.Object, _orderQueriesMock.Object, _identityServiceMock.Object, _loggerMock.Object);
        //var actionResult = await orderController.CompleteOrderAsync(new CompleteOrderCommand(irregularOrder.ordernumber), Guid.NewGuid().ToString());

        //Assert
        var exception =await Assert.ThrowsAsync<OrderingDomainException>(() => orderController.CompleteOrderAsync(new CompleteOrderCommand(irregularOrder.ordernumber), Guid.NewGuid().ToString()));
        

    }
    [Fact]
    public async Task Set_complete_statu_order_false()
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CompleteOrderCommand, bool>>(), default))
            .Returns(Task.FromResult(true));

        //Act
        var orderController = new OrdersController(_mediatorMock.Object, _orderQueriesMock.Object, _identityServiceMock.Object, _loggerMock.Object);
        var actionResult = await orderController.CompleteOrderAsync(new CompleteOrderCommand(1), string.Empty);

        //Assert
        Assert.False(actionResult.Value);
    }
}
