import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { EmployeeListComponent } from './components/employees/employee-list/employee-list.component';
import { EmployeeDetailComponent } from './components/employees/employee-detail/employee-detail.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'employees', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { 
    path: 'employees', 
    component: EmployeeListComponent, 
    canActivate: [authGuard] 
  },
  { 
    path: 'employees/:id', 
    component: EmployeeDetailComponent, 
    canActivate: [authGuard] 
  },
];