using Microsoft.AspNetCore.Identity;
using ProjectManagement.API.Models;

namespace ProjectManagement.API.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<Employee> _passwordHasher;

        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<Employee>();
        }

        public string HashPassword(Employee employee, string password)
        {
            return _passwordHasher.HashPassword(employee, password);
        }

        public bool VerifyPassword(Employee employee, string password)
        {
            if (string.IsNullOrWhiteSpace(employee.PasswordHash))
                return false;

            var result = _passwordHasher.VerifyHashedPassword(
                employee,
                employee.PasswordHash,
                password);

            return result != PasswordVerificationResult.Failed;
        }
    }
}