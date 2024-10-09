using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;

namespace CarStockManagementAPI.Dtos
{
    public class UpdateCarStockRequest
    {
        public int CarId { get; set; }
        public int NewStock { get; set; }
    }

    public class UpdateCarStockRequestValidator : Validator<UpdateCarStockRequest>
    {
        public UpdateCarStockRequestValidator()
        {
            RuleFor(x => x.CarId)
                .GreaterThan(0).WithMessage("CarId must be a positive integer");

            RuleFor(x => x.NewStock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock must be zero or greater");
        }
    }
}