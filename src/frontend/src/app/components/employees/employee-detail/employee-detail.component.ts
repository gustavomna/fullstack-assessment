import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { EmployeeResponse, UpdateEmployeeRequest } from '../../../models/employee.model';
import { DepartmentResponse } from '../../../models/department.model';
import { EmployeeService } from '../../../services/employee.service';
import { DepartmentService } from '../../../services/department.service';
import { catchError, of, Subscription, timeout } from 'rxjs';
import { AuthService } from '../../../services/auth.service';
import { DateUtilsService } from '../../../utils/date-utils.service';

@Component({
  selector: 'app-employee-detail',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.scss']
})
export class EmployeeDetailComponent implements OnInit, OnDestroy {
  employee: EmployeeResponse | null = null;
  departments: DepartmentResponse[] = [];
  loading = true;
  updateForm!: FormGroup;
  updateSuccess = false;
  errorMessage = '';
  private subscriptions = new Subscription();
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private authService: AuthService,
    public dateUtils: DateUtilsService,
    private fb: FormBuilder
  ) {
    this.updateForm = this.fb.group({
      department: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    // Force loading to end after 10 seconds
    setTimeout(() => {
      if (this.loading) {
        this.loading = false;
        this.errorMessage = 'Request timed out. Please try again.';
        console.error('Loading timeout occurred');
      }
    }, 10000);
    
    // Check authentication
    if (!this.authService.isAuthenticated) {
      this.loading = false;
      this.errorMessage = 'You must be logged in to view this page';
      this.router.navigate(['/login'], { 
        queryParams: { returnUrl: this.router.url } 
      });
      return;
    }

    const paramsSub = this.route.paramMap.subscribe(params => {
      const employeeId = params.get('id');
      if (employeeId) {
        this.loadEmployeeAndDepartments(employeeId);
      } else {
        this.loading = false;
        this.router.navigate(['/employees']);
      }
    });
    
    this.subscriptions.add(paramsSub);
  }
  
  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  loadEmployeeAndDepartments(id: string): void {
    console.log('Loading employee and departments for ID:', id);
    this.loading = true;
    
    // Attempt to load employee details first
    const employeeSub = this.employeeService.getEmployee(id).pipe(
      timeout(8000),
      catchError(error => {
        console.error('Error fetching employee details', error);
        this.loading = false;
        if (error.name === 'TimeoutError') {
          this.errorMessage = 'Request timed out. Please try again.';
        } else if (error.status === 401) {
          this.errorMessage = 'Your session has expired. Please log in again.';
          this.redirectToLogin();
        } else {
          this.errorMessage = 'Could not load employee details. Please try again.';
        }
        return of(null);
      })
    ).subscribe(employee => {
      if (employee) {
        this.employee = employee;
        console.log('Employee loaded:', employee);
        // Now load departments
        this.loadDepartments();
      } else {
        this.loading = false;
      }
    });
    
    this.subscriptions.add(employeeSub);
  }
  
  loadDepartments(): void {
    console.log('Loading departments');
    const deptSub = this.departmentService.getDepartments().pipe(
      timeout(8000),
      catchError(error => {
        console.error('Error fetching departments', error);
        this.loading = false;
        if (error.name === 'TimeoutError') {
          this.errorMessage = 'Request for departments timed out. Please try again.';
        } else if (error.status === 401) {
          this.errorMessage = 'Your session has expired. Please log in again.';
          this.redirectToLogin();
        } else {
          this.errorMessage = 'Could not load departments. Please try again.';
        }
        return of([]);
      })
    ).subscribe(departments => {
      this.departments = departments;
      console.log('Departments loaded:', departments.length);
      
      // Set form value now that both are loaded
      if (this.employee && departments.length > 0) {
        console.log('Setting department value:', this.employee.departmentName);
        this.updateForm.patchValue({
          department: this.employee.departmentName
        });
      }
      
      this.loading = false;
    });
    
    this.subscriptions.add(deptSub);
  }

  redirectToLogin(): void {
    setTimeout(() => {
      this.authService.logout();
      this.router.navigate(['/login'], { 
        queryParams: { returnUrl: this.router.url } 
      });
    }, 2000);
  }

  onSubmit(): void {
    if (this.updateForm.invalid || !this.employee) {
      return;
    }
    
    const updateRequest: UpdateEmployeeRequest = {
      firstName: this.employee.firstName,
      lastName: this.employee.lastName,
      phone: this.employee.phone,
      address: this.employee.address,
      departmentName: this.updateForm.get('department')?.value
    };
    
    this.employeeService.updateEmployee(this.employee.id, updateRequest).subscribe({
      next: () => {
        this.updateSuccess = true;
        setTimeout(() => {
          this.updateSuccess = false;
        }, 3000);
        
        // Update the employee object to reflect the change
        if (this.employee) {
          this.employee.departmentName = this.updateForm.get('department')?.value;
        }
      },
      error: (error) => {
        console.error('Error updating employee', error);
        
        if (error.status === 401) {
          this.errorMessage = 'Your session has expired. Please log in again.';
          this.redirectToLogin();
        } else {
          this.errorMessage = 'Error updating employee. Please try again.';
          setTimeout(() => {
            this.errorMessage = '';
          }, 3000);
        }
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/employees']);
  }

  getDepartmentName(departmentId: string): string {
    const department = this.departments.find(d => d.id === departmentId);
    return department ? department.name : 'Unknown Department';
  }
}