import { Customer, PaymentType } from '../customer/customer';
import { OrderDetails } from './order-details';

export interface Order {
  position: number;
  customer: Customer;
  orderDetails: OrderDetails[];
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
