import { Customer, PaymentType } from "./Customer";
import { OrderDetail } from "./OrderDetail";

export class Order {
    transactionId: string;
    customerName: string;
    orderDetails: OrderDetail[];
    paymentType: PaymentType;
    orderDate: Date;
    grandTotal: number;
    orderStatus: OrderStatus;

    constructor(customer: Customer, orderDetails: OrderDetail[]) {
        this.transactionId = '';
        this.customerName = customer?.userName;
        this.orderDetails = orderDetails;
        this.grandTotal = 0;
        this.orderDate = new Date();
        this.paymentType = PaymentType.CashOnDelivery;
        this.orderStatus = OrderStatus.Pending;
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
