import { Order } from '../shared/order';

export interface Customer {
  position: number;
  userName: string;
  email: string;
  phoneNumber: string;
  customerAddress: string;
  paymentType: PaymentType;
  orders: Order[];
}

export enum PaymentType {
  General,
  Cash,
  Month_end,
}
