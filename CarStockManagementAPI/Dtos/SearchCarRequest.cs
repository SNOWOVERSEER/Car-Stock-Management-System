#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;

namespace CarStockManagementAPI.Dtos
{
    public class SearchCarRequest
    {
        public required string Make { get; set; }
        public string? Model { get; set; }
    }
    public class SearchCarRequestValidator : Validator<SearchCarRequest>
    {
        public SearchCarRequestValidator()
        {
            RuleFor(x => x.Make)
                .NotEmpty().WithMessage("Make is required")
                .Length(1, 50).WithMessage("Make must be between 1 and 50 characters");

            RuleFor(x => x.Model)
                .Length(0, 50).WithMessage("Model must be up to 50 characters");
        }
    }
}