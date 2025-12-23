export interface Material {
  id: number;
  naziv: string;
  jedinicnaCena: number;
}

export interface CreateMaterialRequest {
  naziv: string;
  jedinicnaCena: number;
}

export interface UpdateMaterialRequest {
  id: number;
  naziv: string;
  jedinicnaCena: number;
}

export interface PagedResult<T> {
  data: T[];
  totalCount: number;
  currentPage: number;
  pageSize: number;
  totalPages: number;
}
