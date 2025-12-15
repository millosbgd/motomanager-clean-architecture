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
}

export interface CreatePurchaseInvoiceRequest {
  brojRacuna: string;
  datum: Date;
  dobavljacId: number;
  voziloId: number | null;
  iznosNeto: number;
  iznosPDV: number;
  iznosBruto: number;
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
}
