import { Injectable } from '@angular/core';
import { Order } from '../core/models/Order';
import { AuthService } from './auth.service';
import { CartService } from './cart.service';
import { OrderDetail } from '../core/models/OrderDetail';
import { AppSetting } from 'src/app/shared/app-settings';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private order!: Order;
  private completedTransactionId = '';

  constructor(private authService: AuthService, private cartService: CartService, private http: HttpClient) {
  }

  initializeOrder() {
    this.completedTransactionId = '';
    this.order = new Order(this.authService.getCustomer(), this.cartService.getCartItems());
  }

  createOrder(transactionId: string) {
    this.setTransactionId(transactionId);
    return this.http.post(AppSetting.API_URL + 'API/orders', this.order);
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
    this.completedTransactionId = transactionId;
    if (this.order) this.order.transactionId = transactionId;
  }

  getTransactionId(): string {
    return this.completedTransactionId || this.order?.transactionId;
  }

  getOrders() {
    return this.http.get<Order[]>(AppSetting.API_URL + 'API/orders/' + this.authService.getCustomer().userName);
  }
}
