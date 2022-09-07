using Domain.Models.Customer;
using FluentValidation;

namespace Application.Validations.Customers
{
    public class CreateCustomerValidation : AbstractValidator<CreateCustomer>
    {
        public CreateCustomerValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be null or empty.");
            RuleFor(x => x.Age)
                .InclusiveBetween(1, 150)
                .WithMessage("Age must between 1 to 150.");
        }
    }
}