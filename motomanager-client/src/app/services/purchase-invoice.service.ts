import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PurchaseInvoice, CreatePurchaseInvoiceRequest, UpdatePurchaseInvoiceRequest } from '../models/purchase-invoice.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PurchaseInvoiceService {
  private apiUrl = `${environment.apiUrl}/purchaseinvoices`;

  constructor(private http: HttpClient) { }

  getAllPurchaseInvoices(): Observable<PurchaseInvoice[]> {
    return this.http.get<PurchaseInvoice[]>(this.apiUrl);
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
}
