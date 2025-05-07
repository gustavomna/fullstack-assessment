import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateUtilsService {
  formatHireDate(dateString: string): string {
    const hireDate = new Date(dateString);
    const now = new Date();
    
    // Format the date part (May 2, 2021)
    const options: Intl.DateTimeFormatOptions = { 
      year: 'numeric', 
      month: 'long', 
      day: 'numeric' 
    };
    const formattedDate = hireDate.toLocaleDateString('en-US', options);
    
    // Calculate years, months, days since hire
    let years = now.getFullYear() - hireDate.getFullYear();
    let months = now.getMonth() - hireDate.getMonth();
    let days = now.getDate() - hireDate.getDate();
    
    if (days < 0) {
      const lastMonth = new Date(now.getFullYear(), now.getMonth() - 1, 0);
      days += lastMonth.getDate();
      months--;
    }
    
    if (months < 0) {
      months += 12;
      years--;
    }
    
    // Format the duration part (2y - 1m - 4d)
    const duration = `${years}y - ${months}m - ${days}d`;
    
    return `${formattedDate} (${duration})`;
  }
}