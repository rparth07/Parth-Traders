import { Product } from '../product/product';

export interface Category {
  position: number;
  categoryName: string;
  categoryDescription: string;
  products: Product[];
}
