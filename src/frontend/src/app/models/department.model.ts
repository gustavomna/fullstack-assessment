export interface DepartmentResponse {
    id: string;
    name: string;
    description: string;
}
  
export interface CreateDepartmentRequest {
    name: string;
    description: string;
}
  
export interface UpdateDepartmentRequest {
    name: string;
    description: string;
}