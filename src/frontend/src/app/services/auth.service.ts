import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { LoginRequest, RegisterRequest, UserResponse } from '../models/auth.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/Users`;
  private currentUserSubject = new BehaviorSubject<UserResponse | null>(null);
  private platformId = inject(PLATFORM_ID);
  
  public currentUser$ = this.currentUserSubject.asObservable();
  
  constructor(private http: HttpClient) {
    if (isPlatformBrowser(this.platformId)) {
      this.loadStoredUser();
    }
  }
  
  public get currentUserValue(): UserResponse | null {
    return this.currentUserSubject.value;
  }
  
  public get isAuthenticated(): boolean {
    return !!this.getToken();
  }
  
  login(credentials: LoginRequest): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/login`, credentials, {
      responseType: 'text' as 'json'
    }).pipe(
      tap(token => {
        if (token) {
          this.storeToken(token);
          this.loadUserDetails();
        }
      })
    );
  }
  
  register(userData: RegisterRequest): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/register`, userData);
  }
  
  loadUserDetails(): void {
    const token = this.getToken();
    if (token) {
      const userId = this.getUserIdFromToken(token);
      
      if (userId) {
        this.http.get<UserResponse>(`${this.apiUrl}/${userId}`)
          .subscribe({
            next: (user) => {
              this.currentUserSubject.next(user);
              this.storeUserData(user);
            },
            error: (error) => {
              console.error('Error in getting user data', error);
              this.logout();
            }
          });
      }
    }
  }
  
  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('auth_token');
      localStorage.removeItem('user_data');
    }
    this.currentUserSubject.next(null);
  }
  
  private storeToken(token: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('auth_token', token);
    }
  }
  
  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem('auth_token');
    }
    return null;
  }
  
  private storeUserData(user: UserResponse): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('user_data', JSON.stringify(user));
    }
  }
  
  private loadStoredUser(): void {
    if (isPlatformBrowser(this.platformId)) {
      const userData = localStorage.getItem('user_data');
      if (userData) {
        try {
          const user = JSON.parse(userData) as UserResponse;
          this.currentUserSubject.next(user);
        } catch (error) {
          console.error('Erro ao carregar dados do usuário do localStorage', error);
          this.logout();
        }
      }
    }
  }
  
  private getUserIdFromToken(token: string): string | null {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      
      return payload.sub || payload.userId || payload.id;
    } catch (error) {
      console.error('Erro ao decodificar token', error);
      return null;
    }
  }
}