using FluentValidation;
using ProjectManagement.API.DTOs;

namespace ProjectManagement.API.Validators
{
    public class EmployeeCreateDtoValidator
        : AbstractValidator<EmployeeCreateDto>
    {
        public EmployeeCreateDtoValidator()
        {
            RuleFor(x => x.EmployeeName)
                .NotEmpty()
                .WithMessage("Employee name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .WithMessage("Please select a valid department.");

            RuleFor(x => x.Designation)
                .NotEmpty()
                .WithMessage("Designation is required.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("Password must contain at least 8 characters.");
        }
    }
}