import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceOrderMaterial, CreateServiceOrderMaterialRequest, UpdateServiceOrderMaterialRequest } from '../models/service-order-material.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ServiceOrderMaterialService {
  private apiUrl = `${environment.apiUrl}/serviceordermaterials`;

  constructor(private http: HttpClient) { }

  getMaterialsByServiceOrderId(serviceOrderId: number): Observable<ServiceOrderMaterial[]> {
    return this.http.get<ServiceOrderMaterial[]>(`${this.apiUrl}/service-order/${serviceOrderId}`);
  }

  getMaterialById(id: number): Observable<ServiceOrderMaterial> {
    return this.http.get<ServiceOrderMaterial>(`${this.apiUrl}/${id}`);
  }

  createMaterial(request: CreateServiceOrderMaterialRequest): Observable<ServiceOrderMaterial> {
    return this.http.post<ServiceOrderMaterial>(this.apiUrl, request);
  }

  updateMaterial(id: number, request: UpdateServiceOrderMaterialRequest): Observable<ServiceOrderMaterial> {
    return this.http.put<ServiceOrderMaterial>(`${this.apiUrl}/${id}`, request);
  }

  deleteMaterial(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
