import { Product } from '../product/product';
import { Order } from './order';

export interface OrderDetails {
  position: number;
  order: Order;
  product: Product;
  pricePerPiece: number;
  quantityPurchased: number;
  discount: number;
  total: number;
  billDate: Date;
}
