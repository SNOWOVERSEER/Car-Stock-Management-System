using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;

namespace CarStockManagementAPI.Dtos
{
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequestValidator : Validator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}