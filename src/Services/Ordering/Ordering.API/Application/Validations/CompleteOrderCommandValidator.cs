using Microsoft.Extensions.Logging;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.Validations
{
    public class CompleteOrderCommandValidator : AbstractValidator<CompleteOrderCommand>
    {
        public CompleteOrderCommandValidator(ILogger<CompleteOrderCommandValidator> logger) {
            RuleFor(order => order.OrderNumber).NotEmpty().WithMessage("order id cannot be empty");

            logger.LogTrace("INSTANCE CREATED - {ClassName}", GetType().Name);
        }  
    }
}
