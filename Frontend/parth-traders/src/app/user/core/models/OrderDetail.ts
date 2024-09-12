import { Product } from "./Product";

export class OrderDetail {
    productSku: string;
    price: number;
    quantity: number;
    discount: number;
    totalPrice: number;

    constructor(product: Product, quantity: number, totalPrice: number) {
        this.productSku = product.productSku;
        this.price = product.unitPrice;
        this.discount = product.discount;
        this.quantity = quantity;
        this.totalPrice = totalPrice;
    }

    incrementProductCount() {
        this.quantity += 1;
        this.totalPrice += this.price;
    }

    decrementProductCountFrom() {
        this.quantity -= 1;
        this.totalPrice -= this.price;
    }

    getProductQuantity() {
        return this.quantity;
    }

    getTotalPrice() {
        return this.totalPrice;
    }
}