export interface ServiceOrder {
  id: number;
  brojNaloga: string;
  datum: string;
  clientId: number;
  clientNaziv: string;
  vehicleId: number;
  vehicleModel: string;
  vehiclePlate: string;
  opisRada: string;
  kilometraza: number;
}

export interface CreateServiceOrderRequest {
  brojNaloga: string;
  datum: string;
  clientId: number;
  vehicleId: number;
  opisRada: string;
  kilometraza: number;
}

export interface UpdateServiceOrderRequest {
  id: number;
  brojNaloga: string;
  datum: string;
  clientId: number;
  vehicleId: number;
  opisRada: string;
  kilometraza: number;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  currentPage: number;
  pageSize: number;
  totalPages: number;
}
