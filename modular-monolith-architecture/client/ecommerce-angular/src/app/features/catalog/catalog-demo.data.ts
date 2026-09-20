import { Brand, Category, PagedResult, Product } from './catalog.models';

export const demoCategories: Category[] = [
  { id: 'category-electronics', name: 'Electronics', parentCategoryId: null, isActive: true, sortOrder: 1 },
  { id: 'category-home', name: 'Home & Living', parentCategoryId: null, isActive: true, sortOrder: 2 },
  { id: 'category-travel', name: 'Travel', parentCategoryId: null, isActive: true, sortOrder: 3 }
];

export const demoBrands: Brand[] = [
  { id: 'brand-elevate', name: 'Elevate', isActive: true },
  { id: 'brand-northstar', name: 'Northstar', isActive: true },
  { id: 'brand-studio', name: 'Studio Goods', isActive: true }
];

export const demoProducts: Product[] = [
  { id: '10000000-0000-0000-0000-000000000001', sku: 'ELV-AUDIO-001', name: 'Elevate Wireless Headphones', description: 'Comfortable noise-isolating headphones for focused work and relaxed listening.', price: 129.99, status: 'Active' },
  { id: '10000000-0000-0000-0000-000000000002', sku: 'ELV-DESK-002', name: 'Minimal Desk Lamp', description: 'A warm, adjustable desk lamp designed for calm evening workspaces.', price: 64.5, status: 'Active' },
  { id: '10000000-0000-0000-0000-000000000003', sku: 'ELV-TRVL-003', name: 'Everyday Carry Backpack', description: 'A durable everyday backpack with a padded laptop sleeve and smart storage.', price: 89, status: 'Active' },
  { id: '10000000-0000-0000-0000-000000000004', sku: 'ELV-HOME-004', name: 'Stoneware Coffee Mug', description: 'Hand-finished stoneware mug with a comfortable matte grip.', price: 24.95, status: 'Active' },
  { id: '10000000-0000-0000-0000-000000000005', sku: 'ELV-OFFICE-005', name: 'Cloud Notes Notebook', description: 'Premium dotted pages for planning, sketching and daily notes.', price: 18.75, status: 'Active' },
  { id: '10000000-0000-0000-0000-000000000006', sku: 'ELV-TRVL-006', name: 'Weekender Travel Tote', description: 'A lightweight carryall for short trips, gym days and busy commutes.', price: 72, status: 'Active' },
  { id: '10000000-0000-0000-0000-000000000007', sku: 'ELV-HOME-007', name: 'Linen Throw Blanket', description: 'A soft textured throw that adds warmth and character to any room.', price: 58.25, status: 'Active' },
  { id: '10000000-0000-0000-0000-000000000008', sku: 'ELV-OFFICE-008', name: 'Focus Timer', description: 'A simple tactile timer for distraction-free work sessions.', price: 31.4, status: 'Active' }
];

export function demoProductPage(search = '', pageNumber = 1, pageSize = 12): PagedResult<Product> {
  const term = search.trim().toLowerCase();
  const filtered = term
    ? demoProducts.filter(product => `${product.name} ${product.sku}`.toLowerCase().includes(term))
    : demoProducts;
  const start = (pageNumber - 1) * pageSize;
  const items = filtered.slice(start, start + pageSize);
  return { items, pageNumber, pageSize, totalCount: filtered.length, totalPages: Math.max(1, Math.ceil(filtered.length / pageSize)), hasPrevious: pageNumber > 1, hasNext: start + pageSize < filtered.length };
}
