import { Customer, PaymentType } from "./Customer";
import { OrderDetail } from "./OrderDetail";

export class Order {
    transactionId: string;
    customer: Customer;
    orderDetails: OrderDetail[];
    grandTotal: number;
    orderDate: Date;
    paymentType: PaymentType;
    orderStatus: OrderStatus;
    shippingAddress: string;
    billingAddress: string;

    constructor(customer: Customer, orderDetails: OrderDetail[]) {
        this.transactionId = '';
        this.customer = customer;
        this.orderDetails = orderDetails;
        this.grandTotal = 0;
        this.orderDate = new Date();
        this.paymentType = PaymentType.CashOnDelivery;
        this.orderStatus = OrderStatus.Pending;
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

    getTotalOrderPrice() {
        return this.orderDetails.reduce((acc, currentOrderDetail) => acc + currentOrderDetail.getTotalPrice(), 0);
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
