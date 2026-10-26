import { Product } from "./Product";
import { discountedUnitPrice } from "../../../shared/pricing";

export class OrderDetail {
    productSku: string;
    productName?: string;
    unitsInStock?: number;
    price: number;
    quantity: number;
    discount: number;
    totalPrice: number;

    constructor(product: Product, quantity: number, totalPrice: number) {
        this.productSku = product.productSku;
        this.productName = product.productName;
        this.unitsInStock = product.unitsInStock;
        this.price = product.unitPrice;
        this.discount = product.discount;
        this.quantity = quantity;
        this.totalPrice = Math.round(quantity * discountedUnitPrice(this.price, this.discount) * 100) / 100;
    }

    incrementProductCount() {
        this.quantity += 1;
        this.totalPrice = Math.round(this.quantity * discountedUnitPrice(this.price, this.discount) * 100) / 100;
    }

    decrementProductCountFrom() {
        this.quantity -= 1;
        this.totalPrice = Math.round(this.quantity * discountedUnitPrice(this.price, this.discount) * 100) / 100;
    }

    getProductQuantity() {
        return this.quantity;
    }

    getTotalPrice() {
        return this.totalPrice;
    }
}
