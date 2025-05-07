import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { EmployeeResponse, CreateEmployeeRequest } from '../../../models/employee.model';
import { DepartmentResponse } from '../../../models/department.model';
import { EmployeeService } from '../../../services/employee.service';
import { DepartmentService } from '../../../services/department.service';
import { Router } from '@angular/router';
import { DateUtilsService } from '../../../utils/date-utils.service';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent implements OnInit {
  employees: EmployeeResponse[] = [];
  departments: DepartmentResponse[] = [];
  loading = false;
  loadingDepartments = false;
  employeeForm!: FormGroup;
  showModal = false;
  
  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    public dateUtils: DateUtilsService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
    this.loadDepartments();
    this.initForm();
  }

  loadEmployees(): void {
    this.loading = true;
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching employees', error);
        this.loading = false;
      }
    });
  }

  loadDepartments(): void {
    this.loadingDepartments = true;
    this.departmentService.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
        this.loadingDepartments = false;
      },
      error: (error) => {
        console.error('Error fetching departments', error);
        this.loadingDepartments = false;
      }
    });
  }

  initForm(): void {
    this.employeeForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      phone: ['', Validators.required],
      address: ['', Validators.required],
      department: ['', Validators.required],
      hireDate: [new Date().toISOString().split('T')[0], Validators.required]
    });
  }

  openNewEmployeeModal(): void {
    this.employeeForm.reset({
      hireDate: new Date().toISOString().split('T')[0]
    });
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onSubmit(): void {
    if (this.employeeForm.invalid) {
      return;
    }
    
    const newEmployee: CreateEmployeeRequest = this.employeeForm.value;
    
    this.employeeService.createEmployee(newEmployee).subscribe({
      next: (employeeId) => {
        this.showModal = false;
        this.loadEmployees();
      },
      error: (error) => {
        console.error('Error creating employee', error);
      }
    });
  }

  deleteEmployee(id: string): void {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.deleteEmployee(id).subscribe({
        next: () => {
          this.loadEmployees();
        },
        error: (error) => {
          console.error('Error deleting employee', error);
        }
      });
    }
  }

  viewEmployeeDetails(id: string): void {
    this.router.navigate(['/employees', id]);
  }

  get f() {
    return this.employeeForm.controls;
  }

  getFullName(employee: EmployeeResponse): string {
    return `${employee.firstName} ${employee.lastName}`;
  }

  getDepartmentName(departmentId: string): string {
    console.log('Department ID:', departmentId);
    const department = this.departments.find(d => d.name === departmentId);
    return department ? department.name : 'Unknown Department';
  }
}