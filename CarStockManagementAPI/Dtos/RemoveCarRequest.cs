using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;

namespace CarStockManagementAPI.Dtos
{
    public class RemoveCarRequest
    {
        public int CarId { get; set; }
    }
    public class RemoveCarRequestValidator : Validator<RemoveCarRequest>
    {
        public RemoveCarRequestValidator()
        {
            RuleFor(x => x.CarId)
                .GreaterThan(0)
                .WithMessage("Car ID must be a positive integer.");
        }
    }
}