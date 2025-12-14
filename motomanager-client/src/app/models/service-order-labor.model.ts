export interface ServiceOrderLabor {
  id: number;
  serviceOrderId: number;
  opisRadova: string;
  ukupnoVreme: number;
  cena: number;
}

export interface CreateServiceOrderLaborRequest {
  serviceOrderId: number;
  opisRadova: string;
  ukupnoVreme: number;
  cena: number;
}

export interface UpdateServiceOrderLaborRequest {
  id: number;
  serviceOrderId: number;
  opisRadova: string;
  ukupnoVreme: number;
  cena: number;
}
