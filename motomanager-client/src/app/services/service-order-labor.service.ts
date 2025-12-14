import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceOrderLabor, CreateServiceOrderLaborRequest, UpdateServiceOrderLaborRequest } from '../models/service-order-labor.model';

@Injectable({
  providedIn: 'root'
})
export class ServiceOrderLaborService {
  private apiUrl = 'http://localhost:10582/api/serviceorderlabors';

  constructor(private http: HttpClient) { }

  getLaborsByServiceOrderId(serviceOrderId: number): Observable<ServiceOrderLabor[]> {
    return this.http.get<ServiceOrderLabor[]>(`${this.apiUrl}/service-order/${serviceOrderId}`);
  }

  getLaborById(id: number): Observable<ServiceOrderLabor> {
    return this.http.get<ServiceOrderLabor>(`${this.apiUrl}/${id}`);
  }

  createLabor(request: CreateServiceOrderLaborRequest): Observable<ServiceOrderLabor> {
    return this.http.post<ServiceOrderLabor>(this.apiUrl, request);
  }

  updateLabor(id: number, request: UpdateServiceOrderLaborRequest): Observable<ServiceOrderLabor> {
    return this.http.put<ServiceOrderLabor>(`${this.apiUrl}/${id}`, request);
  }

  deleteLabor(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
