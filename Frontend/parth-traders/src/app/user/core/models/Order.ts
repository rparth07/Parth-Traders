import { Customer, PaymentType } from "./Customer";
import { OrderDetail } from "./OrderDetail";
import { Product } from "./Product";

export class Order {
    customer: Customer;
    orderDetails: OrderDetail[];
    grandTotal: number;
    orderDate: Date;
    paymentType: PaymentType;
    orderStatus: OrderStatus;
    shippingAddress: string;
    billingAddress: string;

    constructor(customer: Customer) {
        this.customer = customer;
        this.orderDetails = new Array<OrderDetail>();
        this.grandTotal = 0;
        this.orderDate = new Date();
        this.paymentType = PaymentType.CashOnDelivery;
        this.orderStatus = OrderStatus.Approved;
        this.shippingAddress = '';
        this.billingAddress = '';
    }

    updateShippingAddress(shippingAddress: string) {
        this.shippingAddress = shippingAddress;
    }

    updateBillingAddress(billingAddress: string) {
        this.billingAddress = billingAddress;
    }

    getOrderDetails() {
        return this.orderDetails;
    }

    hasProduct(product: Product): Boolean {
        return this.orderDetails.findIndex(_ => _.product == product) > -1;
    }

    addNewProduct(product: Product, productSize: string) {
        this.orderDetails.push(new OrderDetail(product, productSize, 1, product.price));
    }

    updateProductCount(product: Product) {
        this.orderDetails[this.orderDetails.findIndex(_ => _.product == product)].incrementProductCount();
    }

    removeOrderItemAt(index: number) {
        this.orderDetails.splice(index, 1);
    }

    incrementProductCountOf(orderDetail: OrderDetail) {
        const index = this.orderDetails.findIndex(_ => _.product == orderDetail.product);
        this.orderDetails[index].incrementProductCount();
    }

    decrementProductCountFrom(orderDetail: OrderDetail) {
        const index = this.orderDetails.findIndex(_ => _.product == orderDetail.product);
        if (this.orderDetails[index].getProductQuantity() == 1) {
            this.orderDetails.splice(index, 1);
        } else {
            this.orderDetails[index].decrementProductCountFrom();
        }
    }
}

export enum OrderStatus {
    Pending,
    Approved,
    Processing,
    CanceledByCustomer,
    CanceledByOwner,
    Completed,
}
