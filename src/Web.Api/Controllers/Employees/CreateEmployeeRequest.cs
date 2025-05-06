namespace Web.Api.Controllers.Employees;

public class CreateEmployeeRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime HireDate { get; set; } = DateTime.UtcNow;
}