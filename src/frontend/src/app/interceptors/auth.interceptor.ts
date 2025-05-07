import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();
  
  console.log(`Request to ${req.url}, token present: ${!!token}`);
  
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    console.log(`Authorization header set for request to ${req.url}`);
  } else {
    console.warn(`No auth token available for request to ${req.url}`);
  }
  
  return next(req);
};