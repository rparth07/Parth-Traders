import { Customer, PaymentType } from "./Customer";
import { OrderDetail } from "./OrderDetail";

export interface Order {
    position: number;
    customer: Customer;
    orderDetails: OrderDetail[];
    paymentType: PaymentType;
    orderDate: Date;
    grandTotal: number;
    orderStatus: OrderStatus;
}

export enum OrderStatus {
    Pending,
    Approved,
    Processing,
    CanceledByCustomer,
    CanceledByOwner,
    Completed,
}
