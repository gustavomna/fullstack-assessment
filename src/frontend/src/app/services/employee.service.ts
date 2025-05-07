// src/app/services/employee.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { EmployeeResponse, CreateEmployeeRequest, UpdateEmployeeRequest } from '../models/employee.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = `${environment.apiUrl}/Employees`;

  constructor(private http: HttpClient) {}

  getEmployees(): Observable<EmployeeResponse[]> {
    return this.http.get<EmployeeResponse[]>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Error fetching employees', error);
        return of([]);
      })
    );
  }

  getEmployee(id: string): Observable<EmployeeResponse> {
    console.log(`Fetching employee with ID: ${id}`);
    return this.http.get<EmployeeResponse>(`${this.apiUrl}/${id}`).pipe(
      tap(response => console.log('Employee API response:', response)),
      catchError(error => {
        console.error('Error in getEmployee service call:', error);
        throw error;
      })
    );
  }

  createEmployee(employee: CreateEmployeeRequest): Observable<string> {
    return this.http.post<string>(this.apiUrl, employee, {
      responseType: 'text' as 'json'
    }).pipe(
      map(response => {
        return response.replace(/"/g, '');
      })
    );
  }

  updateEmployee(id: string, employee: UpdateEmployeeRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, employee);
  }

  deleteEmployee(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}