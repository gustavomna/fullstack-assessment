import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { DepartmentResponse } from '../models/department.model';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private apiUrl = `${environment.apiUrl}/Departments`;

  constructor(private http: HttpClient) {}

  getDepartments(): Observable<DepartmentResponse[]> {
    console.log('Fetching departments');
    return this.http.get<DepartmentResponse[]>(this.apiUrl).pipe(
      tap(response => console.log('Departments API response:', response)),
      catchError(error => {
        console.error('Error in getDepartments service call:', error);
        throw error;
      })
    );
  }

  getDepartment(id: string): Observable<DepartmentResponse> {
    return this.http.get<DepartmentResponse>(`${this.apiUrl}/${id}`);
  }
}