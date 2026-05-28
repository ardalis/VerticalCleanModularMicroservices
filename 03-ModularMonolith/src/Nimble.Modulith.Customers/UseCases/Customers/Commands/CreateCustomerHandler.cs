using Ardalis.Result;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Nimble.Modulith.Customers.Domain.CustomerAggregate;
using Nimble.Modulith.Customers.Domain.Interfaces;
using Nimble.Modulith.Email.Contracts;
using Nimble.Modulith.Users.Contracts;

namespace Nimble.Modulith.Customers.UseCases.Customers.Commands;

public class CreateCustomerHandler(
    IRepository<Customer> repository, 
    IMediator mediator,
    UserManager<IdentityUser> userManager) 
    : ICommandHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    public async ValueTask<Result<CustomerDto>> Handle(CreateCustomerCommand command, CancellationToken ct)
    {
        // Check if user already exists
        var existingUser = await userManager.FindByEmailAsync(command.Email);
        string? temporaryPassword = null;

        if (existingUser == null)
        {
            // Generate a random password
            temporaryPassword = Guid.NewGuid().ToString("N")[..12]; // First 12 chars of GUID

            // Create Identity user
            var createUserCommand = new CreateUserCommand(command.Email, temporaryPassword);
            var userResult = await mediator.Send(createUserCommand, ct);

            if (!userResult.IsSuccess)
            {
                return Result<CustomerDto>.Error($"Failed to create user account: {userResult.Errors.FirstOrDefault()}");
            }
        }

        // Create customer record
        var customer = new Customer
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            Address = new Address
            {
                Street = command.Street,
                City = command.City,
                State = command.State,
                PostalCode = command.PostalCode,
                Country = command.Country
            }
        };

        await repository.AddAsync(customer, ct);
        await repository.SaveChangesAsync(ct);

        // Send welcome email - only include password if a new user was created
        if (temporaryPassword != null)
        {
            var emailBody = $@"
Welcome to our service!

Your account has been created successfully.

Email: {command.Email}
Temporary Password: {temporaryPassword}

Please log in and change your password as soon as possible.

Best regards,
The Team
";

            var emailCommand = new SendEmailCommand(
                command.Email,
                "Welcome - Your Account Has Been Created",
                emailBody
            );

            await mediator.Send(emailCommand, ct);
        }
        else
        {
            var emailBody = $@"
Welcome back!

A customer profile has been created for your existing account.

Email: {command.Email}

You can continue using your existing password to access our services.

Best regards,
The Team
";

            var emailCommand = new SendEmailCommand(
                command.Email,
                "Customer Profile Created",
                emailBody
            );

            await mediator.Send(emailCommand, ct);
        }

        var dto = new CustomerDto(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.PhoneNumber,
            new AddressDto(
                customer.Address.Street,
                customer.Address.City,
                customer.Address.State,
                customer.Address.PostalCode,
                customer.Address.Country
            ),
            customer.CreatedAt,
            customer.UpdatedAt
        );

        return Result<CustomerDto>.Success(dto);
    }
}
