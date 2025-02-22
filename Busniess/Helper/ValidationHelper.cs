

using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Busniess.Dtos;
using Busniess.Interface;
using Busniess.Services;
using Data.Entities;

namespace Busniess.Helper;

public class ValidationHelper
{
    private static readonly Regex EmailRegex = new(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled);
    private static readonly Regex PhoneRegex = new(@"^\+?[0-9]{7,15}$", RegexOptions.Compiled);

    public static async Task<List<string>> ValidateEmployee(EmployeeDto employee, IEmployeeService employeeService)
    {
        var errors = new List<string>();
        if (employee == null)
        {
            errors.Add("Employee data is missing.");
            return errors;
        }

        if (string.IsNullOrWhiteSpace(employee.FirstName))
            errors.Add("First name is required.");

        if (string.IsNullOrWhiteSpace(employee.LastName))
            errors.Add("Last name is required.");

        if (string.IsNullOrWhiteSpace(employee.Email) || !EmailRegex.IsMatch(employee.Email))
            errors.Add("Valid email is required.");

        if (string.IsNullOrWhiteSpace(employee.PhoneNumber) || !PhoneRegex.IsMatch(employee.PhoneNumber))
            errors.Add("Valid phone number is required.");

        if (employee.RoleId <= 0)
            errors.Add("A valid role must be selected.");

        var existingEmployees = await employeeService.GetAllEmployeesAsync();
        if (existingEmployees.Any(e => e.Email.ToLower() == employee.Email.ToLower() && e.Id != employee.Id))
        {
            errors.Add("This email is already in use by another employee.");
        }


        return errors; ;

    }
    public static async Task<List<String>> ValidateCustomer(CustomerDto customer, ICustomerService customerService)
    {
        var errors = new List<string>();
        if (customer == null)
        {
            errors.Add("Customer data is missing.");
            return errors;
        }

        if (string.IsNullOrWhiteSpace(customer.FirstName))
            errors.Add("First name is required.");

        if (string.IsNullOrWhiteSpace(customer.LastName))
            errors.Add("Last name is required.");

        if (string.IsNullOrWhiteSpace(customer.Email) || !EmailRegex.IsMatch(customer.Email))
            errors.Add("Valid email is required.");

        if (string.IsNullOrWhiteSpace(customer.PhoneNumber) || !PhoneRegex.IsMatch(customer.PhoneNumber))
            errors.Add("Valid phone number is required.");

        var existingCustomer = await customerService.GetAllCustomersAsync();
        if (existingCustomer.Any(e => e.Email.ToLower() == customer.Email.ToLower() && e.Id != customer.Id))
        {
            errors.Add("This email is already in use by another employee.");
        }

        return errors;
    }
    public static List<String> ValidateProject(ProjectDto project)
    {
        var errors = new List<string>();
        if (project == null)
        {
            errors.Add("Project data is missing.");
            return errors;
        }

        if (string.IsNullOrWhiteSpace(project.Name))
            errors.Add("Project name is required.");

        if (string.IsNullOrWhiteSpace(project.Description))
            errors.Add("Project description is required.");

        if (project.StartDate == default)
            errors.Add("Start date is required.");

        if (project.EndDate == default || project.EndDate < project.StartDate)
            errors.Add("End date must be after start date.");

        if (project.CustomerId <= 0)
            errors.Add("A valid customer must be selected.");

        if (project.EmployeeId <= 0)
            errors.Add("A valid employee must be selected.");

        if (project.ServiceId <= 0)
            errors.Add("A valid service must be selected.");

        return errors;
    }
}

