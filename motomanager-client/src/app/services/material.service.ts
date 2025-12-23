import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Material, CreateMaterialRequest, UpdateMaterialRequest, PagedResult } from '../models/material.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MaterialService {
  private apiUrl = `${environment.apiUrl}/materials`;

  constructor(private http: HttpClient) { }

  getAllMaterials(): Observable<Material[]> {
    return this.http.get<Material[]>(this.apiUrl);
  }

  getMaterialsPaged(pageNumber: number = 1, pageSize: number = 20): Observable<PagedResult<Material>> {
    let params: any = {
      pageNumber: pageNumber.toString(),
      pageSize: pageSize.toString(),
      _t: new Date().getTime()
    };
    return this.http.get<PagedResult<Material>>(`${this.apiUrl}/paged`, { params });
  }

  getMaterialById(id: number): Observable<Material> {
    return this.http.get<Material>(`${this.apiUrl}/${id}`);
  }

  createMaterial(request: CreateMaterialRequest): Observable<Material> {
    return this.http.post<Material>(this.apiUrl, request);
  }

  updateMaterial(id: number, request: UpdateMaterialRequest): Observable<Material> {
    return this.http.put<Material>(`${this.apiUrl}/${id}`, request);
  }

  deleteMaterial(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
