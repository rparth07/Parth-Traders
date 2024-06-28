export class Customer {
    email: string;
    first_name: string;
    last_name: string;
    token: string;

    constructor(email: string, first_name: string, last_name: string, token: string) {
        this.email = email;
        this.first_name = first_name;
        this.last_name = last_name;
        this.token = token;
    }
}

export enum PaymentType {
    General,
    Cash,
    Month_end,
    CashOnDelivery,
}