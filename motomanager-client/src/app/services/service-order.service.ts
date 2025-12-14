import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceOrder, CreateServiceOrderRequest, UpdateServiceOrderRequest } from '../models/service-order.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ServiceOrderService {
  private apiUrl = `${environment.apiUrl}/serviceorders`;

  constructor(private http: HttpClient) { }

  getAllServiceOrders(): Observable<ServiceOrder[]> {
    return this.http.get<ServiceOrder[]>(this.apiUrl);
  }

  getServiceOrderById(id: number): Observable<ServiceOrder> {
    return this.http.get<ServiceOrder>(`${this.apiUrl}/${id}`);
  }

  createServiceOrder(request: CreateServiceOrderRequest): Observable<ServiceOrder> {
    return this.http.post<ServiceOrder>(this.apiUrl, request);
  }

  updateServiceOrder(id: number, request: UpdateServiceOrderRequest): Observable<ServiceOrder> {
    return this.http.put<ServiceOrder>(`${this.apiUrl}/${id}`, request);
  }

  deleteServiceOrder(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
