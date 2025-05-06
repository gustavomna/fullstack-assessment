﻿using Application.Abstractions.Messaging;

namespace Application.Departments.Create;

public sealed record CreateDepartmentCommand(
    string Name,
    string Description) : ICommand<Guid>;
