using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;

namespace CarStockManagementAPI.Dtos
{
    public class AddCarRequest
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int Stock { get; set; }
    }
    public class AddCarRequestValidator : Validator<AddCarRequest>
    {
        public AddCarRequestValidator()
        {
            RuleFor(x => x.Make)
                .NotEmpty()
                .WithMessage("Make is required.");

            RuleFor(x => x.Model)
                .NotEmpty()
                .WithMessage("Model is required.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1886, DateTime.Now.Year)
                .WithMessage($"Year must be between 1886 and {DateTime.Now.Year}.");

            RuleFor(x => x.Color)
                .NotEmpty()
                .WithMessage("Color is required.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock must be a non-negative integer.");
        }
    }
}