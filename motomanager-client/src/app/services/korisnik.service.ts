import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Korisnik {
  id: string;
  imePrezime: string;
  userName: string;
  sektorId: number;
  sektorNaziv: string;
}

@Injectable({
  providedIn: 'root'
})
export class KorisnikService {
  private apiUrl = `${environment.apiUrl}/korisnici`;
  private isRegisteredSubject = new BehaviorSubject<boolean | null>(null);
  public isRegistered$ = this.isRegisteredSubject.asObservable();

  constructor(private http: HttpClient) {}

  checkIfRegistered(userId: string): Observable<{ exists: boolean }> {
    return this.http.get<{ exists: boolean }>(`${this.apiUrl}/exists/${userId}`);
  }

  setRegistered(value: boolean | null) {
    this.isRegisteredSubject.next(value);
  }

  getAll(): Observable<Korisnik[]> {
    return this.http.get<Korisnik[]>(this.apiUrl);
  }

  getById(id: string): Observable<Korisnik> {
    return this.http.get<Korisnik>(`${this.apiUrl}/${id}`);
  }
}
