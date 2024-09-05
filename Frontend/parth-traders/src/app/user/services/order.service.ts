import { EventEmitter, Injectable } from '@angular/core';
import { Order } from '../core/models/Order';
import { AuthService } from './auth.service';
import { CartService } from './cart.service';
import { OrderDetail } from '../core/models/OrderDetail';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private order!: Order;

  constructor(private authService: AuthService, private cartService: CartService) {
  }

  createOrder() {
    this.order = new Order(this.authService.getCustomer(), this.cartService.getCartItems());
  }

  getOrderItems(): OrderDetail[] {
    return this.order?.orderDetails;
  }

  getTotalOrderPrice(): number {
    if (this.order) {
      return this.order.orderDetails.reduce((acc, currentOrderDetail) => acc + currentOrderDetail.getTotalPrice(), 0);
    }
    return 0;
  }

  setTransactionId(transactionId: string) {
    if (this.order)
      this.order.transactionId = transactionId;
  }

  getTransactionId(): string {
    return this.order?.transactionId;
  }
}
