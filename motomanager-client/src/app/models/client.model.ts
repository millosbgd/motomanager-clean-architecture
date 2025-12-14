export interface Client {
  id: number;
  naziv: string;
  adresa: string;
  grad: string;
  pib: string | null;
  telefon: string;
  email: string;
}

export interface CreateClientRequest {
  naziv: string;
  adresa: string;
  grad: string;
  pib: string | null;
  telefon: string;
  email: string;
}

export interface UpdateClientRequest {
  id: number;
  naziv: string;
  adresa: string;
  grad: string;
  pib: string | null;
  telefon: string;
  email: string;
}
