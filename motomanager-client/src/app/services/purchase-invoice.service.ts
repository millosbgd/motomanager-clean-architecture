import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PurchaseInvoice, CreatePurchaseInvoiceRequest, UpdatePurchaseInvoiceRequest, PagedResult } from '../models/purchase-invoice.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PurchaseInvoiceService {
  private apiUrl = `${environment.apiUrl}/purchaseinvoices`;

  constructor(private http: HttpClient) { }

  getAllPurchaseInvoices(
    datumOd?: string, 
    datumDo?: string, 
    dobavljacId?: number | null, 
    voziloId?: number | null,
    sektorId?: number | null,
    pageNumber: number = 1,
    pageSize: number = 20
  ): Observable<PagedResult<PurchaseInvoice>> {
    let params: any = {};
    
    if (datumOd) {
      params.datumOd = datumOd;
    }
    if (datumDo) {
      params.datumDo = datumDo;
    }
    if (dobavljacId && dobavljacId > 0) {
      params.dobavljacId = dobavljacId.toString();
    }
    if (voziloId && voziloId > 0) {
      params.voziloId = voziloId.toString();
    }
    if (sektorId && sektorId > 0) {
      params.sektorId = sektorId.toString();
    }
    
    params.pageNumber = pageNumber.toString();
    params.pageSize = pageSize.toString();
    
    // Add cache busting parameter
    params._t = new Date().getTime();
    
    return this.http.get<PagedResult<PurchaseInvoice>>(this.apiUrl, { params });
  }

  getPurchaseInvoiceById(id: number): Observable<PurchaseInvoice> {
    return this.http.get<PurchaseInvoice>(`${this.apiUrl}/${id}`);
  }

  createPurchaseInvoice(request: CreatePurchaseInvoiceRequest): Observable<PurchaseInvoice> {
    return this.http.post<PurchaseInvoice>(this.apiUrl, request);
  }

  updatePurchaseInvoice(id: number, request: UpdatePurchaseInvoiceRequest): Observable<PurchaseInvoice> {
    return this.http.put<PurchaseInvoice>(`${this.apiUrl}/${id}`, request);
  }

  deletePurchaseInvoice(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  exportToExcel(
    datumOd?: string, 
    datumDo?: string, 
    dobavljacId?: number | null, 
    voziloId?: number | null
  ): Observable<Blob> {
    let params: any = {};
    
    if (datumOd) {
      params.datumOd = datumOd;
    }
    if (datumDo) {
      params.datumDo = datumDo;
    }
    if (dobavljacId && dobavljacId > 0) {
      params.dobavljacId = dobavljacId.toString();
    }
    if (voziloId && voziloId > 0) {
      params.voziloId = voziloId.toString();
    }
    
    return this.http.get(`${this.apiUrl}/export-excel`, { 
      params, 
      responseType: 'blob' 
    });
  }
}
