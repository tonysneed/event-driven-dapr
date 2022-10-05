using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Domain.OrderAggregate;
using OrderService.Integration.EventHandlers;
using OrderService.Repositories;
using OrderService.Tests.Helpers;
using Xunit;
// TODO: Using directives
// using Common.Integration.Events;
// using EventDriven.EventBus.Abstractions;
// using Address = Common.Integration.Models.Address;

namespace OrderService.Tests.Integration.EventHandlers;

public class CustomerAddressUpdatedEventHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<ILogger<CustomerAddressUpdatedEventHandler>> _logger;
    private readonly IMapper _mapper;

    private readonly Mock<IOrderRepository> _repositoryMoq;

    public CustomerAddressUpdatedEventHandlerTests()
    {
        _repositoryMoq = new Mock<IOrderRepository>();
        _mapper = MappingHelper.GetMapper();
        _logger = new Mock<ILogger<CustomerAddressUpdatedEventHandler>>();
    }

    [Fact]
    public void WhenInstantiated_ThenShouldBeOfCorrectType()
    {
        var handler = new CustomerAddressUpdatedEventHandler(_repositoryMoq.Object, _mapper, _logger.Object);

        Assert.NotNull(handler);
        // TODO: Uncomment
        // Assert.IsAssignableFrom<IntegrationEventHandler<CustomerAddressUpdated>>(handler);
        Assert.IsType<CustomerAddressUpdatedEventHandler>(handler);
    }

    [Fact]
    public /* async Task */ void WhenEventIsHandled_ThenOrderAddressShouldGetUpdated()
    {
        // TODO: Uncomment
        var address = _fixture.Create<Address>();
        // var updatedEvent = new CustomerAddressUpdated(Guid.NewGuid(), address);
        var addressWasUpdated = false;
        _repositoryMoq.Setup(x => x.GetByCustomerAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new[] { _fixture.Create<Order>() });
        _repositoryMoq.Setup(x => x.UpdateAddressAsync(It.IsAny<Guid>(), It.IsAny<OrderService.Domain.OrderAggregate.Address>()))
            .Callback<Guid, OrderService.Domain.OrderAggregate.Address>((_, _) => { addressWasUpdated = true; });
        var handler = new CustomerAddressUpdatedEventHandler(_repositoryMoq.Object, _mapper, _logger.Object);

        // TODO: Uncomment
        // await handler.HandleAsync(updatedEvent);

        Assert.True(addressWasUpdated);
    }
}