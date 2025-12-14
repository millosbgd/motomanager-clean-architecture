export interface Vehicle {
  id: number;
  model: string;
  plate: string;
  clientId: number;
  clientNaziv: string;
}

export interface CreateVehicleRequest {
  model: string;
  plate: string;
  clientId: number;
}

export interface UpdateVehicleRequest {
  model: string;
  plate: string;
  clientId: number;
}
