import { Order } from "./Order";

export class Customer {
    id: number;
    userName: string;
    email: string;
    phoneNumber: string;
    first_name: string;
    last_name: string;
    token: string;
    customerAddress: string;
    paymentType: PaymentType;
    orders: Order[];

    constructor(
        id: number,
        userName: string,
        email: string,
        phoneNumber: string,
        first_name: string,
        last_name: string,
        token: string,
        customerAddress: string,
        paymentType: PaymentType,
        orders: Order[]) {
        this.id = id;
        this.userName = userName;
        this.email = email;
        this.phoneNumber = phoneNumber;
        this.first_name = first_name;
        this.last_name = last_name;
        this.token = token;
        this.customerAddress = customerAddress;
        this.paymentType = paymentType;
        this.orders = orders;
    }
}

export enum PaymentType {
    General,
    Cash,
    Month_end,
    CashOnDelivery,
}