export interface PurchaseInvoice {
  id: number;
  brojRacuna: string;
  datum: Date;
  dobavljacId: number;
  dobavljacNaziv: string;
  voziloId: number | null;
  voziloModel: string | null;
  voziloPlate: string | null;
  iznosNeto: number;
  iznosPDV: number;
  iznosBruto: number;
  sektorId: number | null;
  sektorNaziv: string | null;
}

export interface PagedResult<T> {
  data: T[];
  totalCount: number;
  currentPage: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface CreatePurchaseInvoiceRequest {
  brojRacuna: string;
  datum: Date;
  dobavljacId: number;
  voziloId: number | null;
  iznosNeto: number;
  iznosPDV: number;
  iznosBruto: number;
  sektorId: number | null;
}

export interface UpdatePurchaseInvoiceRequest {
  id: number;
  brojRacuna: string;
  datum: Date;
  dobavljacId: number;
  voziloId: number | null;
  iznosNeto: number;
  iznosPDV: number;
  iznosBruto: number;
  sektorId: number | null;
}
