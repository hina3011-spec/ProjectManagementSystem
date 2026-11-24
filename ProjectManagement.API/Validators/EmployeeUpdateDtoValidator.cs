using FluentValidation;
using ProjectManagement.API.DTOs;

namespace ProjectManagement.API.Validators
{
    public class EmployeeUpdateDtoValidator
        : AbstractValidator<EmployeeUpdateDto>
    {
        public EmployeeUpdateDtoValidator()
        {
            RuleFor(x => x.EmployeeName)
                .NotEmpty()
                .WithMessage("Employee name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .WithMessage("Please select a valid department.");

            RuleFor(x => x.Designation)
                .NotEmpty()
                .WithMessage("Designation is required.");
        }
    }
}