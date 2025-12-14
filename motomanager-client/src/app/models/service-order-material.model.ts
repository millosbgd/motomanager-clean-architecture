export interface ServiceOrderMaterial {
  id: number;
  serviceOrderId: number;
  materialId: number;
  materialNaziv: string;
  kolicina: number;
  jedinicnaCena: number;
  ukupnaCena: number;
}

export interface CreateServiceOrderMaterialRequest {
  serviceOrderId: number;
  materialId: number;
  kolicina: number;
  jedinicnaCena: number;
  ukupnaCena: number;
}

export interface UpdateServiceOrderMaterialRequest {
  id: number;
  serviceOrderId: number;
  materialId: number;
  kolicina: number;
  jedinicnaCena: number;
  ukupnaCena: number;
}
