import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Sektor } from '../models/sektor.model';

@Injectable({
  providedIn: 'root'
})
export class SektorService {
  private apiUrl = `${environment.apiUrl}/sektori`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Sektor[]> {
    return this.http.get<Sektor[]>(this.apiUrl);
  }

  getById(id: number): Observable<Sektor> {
    return this.http.get<Sektor>(`${this.apiUrl}/${id}`);
  }
}
