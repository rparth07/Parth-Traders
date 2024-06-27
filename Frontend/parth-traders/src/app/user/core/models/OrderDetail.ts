import { Order } from "./Order";
import { Product } from "./Product";


export interface OrderDetail {
    position: number;
    order: Order;
    product: Product;
    productSize: string,
    pricePerPiece: number;
    quantityPurchased: number;
    discount: number;
    total: number;
    billDate: Date;
}
