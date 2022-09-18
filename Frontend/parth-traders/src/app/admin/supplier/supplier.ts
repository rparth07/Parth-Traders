import { Product } from '../product/product';

export interface Supplier {
  position: number;
  supplierName: string;
  supplierAddress: string;
  supplierEmail: string;
  supplierPhoneNumber: string;
  products: Product[];
}
