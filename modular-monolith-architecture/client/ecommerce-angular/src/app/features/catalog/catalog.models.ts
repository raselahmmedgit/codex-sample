export interface ApiResult<T> {
  succeeded: boolean;
  data: T | null;
  message: string;
  errors: string[];
}

export interface Product {
  id: string;
  sku: string;
  name: string;
  description: string | null;
  price: number;
  status: string;
  imageUrl?: string | null;
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface Category {
  id: string;
  name: string;
  parentCategoryId: string | null;
  isActive: boolean;
  sortOrder: number;
}

export interface Brand {
  id: string;
  name: string;
  isActive: boolean;
}
