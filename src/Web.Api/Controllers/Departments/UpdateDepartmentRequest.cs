﻿namespace Web.Api.Controllers.Departments;

public class UpdateDepartmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}