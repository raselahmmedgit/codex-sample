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
  status: string | number;
  imageUrl?: string | null;
}

const productStatuses = ['Draft', 'Active', 'Inactive', 'Out of stock', 'Archived'];

export function productStatusLabel(status: string | number): string {
  if (typeof status === 'number' || /^\d+$/.test(status)) {
    return productStatuses[Number(status)] ?? 'Unknown';
  }

  const normalized = status.replace(/([a-z])([A-Z])/g, '$1 $2').toLowerCase();
  return productStatuses.find(label => label.toLowerCase() === normalized)?.toString() ?? status;
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
