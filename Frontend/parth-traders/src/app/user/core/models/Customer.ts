export interface Customer {
    id: string;
    email: string;
    first_name: string;
    last_name: string;
    token: string;
}

export enum PaymentType {
    General,
    Cash,
    Month_end,
}