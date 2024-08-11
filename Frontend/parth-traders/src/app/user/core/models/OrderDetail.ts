import { Order } from "./Order";
import { Product } from "./Product";


export class OrderDetail {
    product: Product;
    productSize: string;
    quantity: number;
    totalPrice: number;

    constructor(product: Product, productSize: string, quantity: number, totalPrice: number) {
        this.product = product;
        this.productSize = productSize;
        this.quantity = quantity;
        this.totalPrice = totalPrice;
    }

    incrementProductCount() {
        this.quantity += 1;
        this.totalPrice += this.product.price;
    }

    decrementProductCountFrom() {
        this.quantity -= 1;
        this.totalPrice -= this.product.price;
    }

    getProductQuantity() {
        return this.quantity;
    }
}