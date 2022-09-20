import { Order } from '../shared/order';

export interface Customer {
  position: number;
  customerName: string;
  createdDate: Date;
  customerEmail: string;
  customerPhoneNumber: string;
  customerAddress: string;
  paymentType: PaymentType;
  orders: Order[];
}

export enum PaymentType {
  General,
  Cash,
  Month_end,
}
