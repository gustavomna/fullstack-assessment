export interface Employee {
    id: string;
    firstName: string;
    lastName: string;
    departmentName: string;
    hireDate: string;
    avatarUrl: string;
    email?: string;
    phone?: string;
    position?: string;
  }

  export interface EmployeeResponse {
    id: string;
    firstName: string;
    lastName: string;
    phone: string;
    address: string;
    departmentName: string;
    hireDate: string; 
  }

  export interface CreateEmployeeRequest {
    firstName: string;
    lastName: string;
    phone: string;
    address: string;
    departmentName: string;
    hireDate: string;
  }
  
  export interface UpdateEmployeeRequest {
    firstName: string;
    lastName: string;
    phone: string;
    address: string;
    departmentName: string;
  }