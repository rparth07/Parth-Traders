import { ElementRef, EventEmitter, Injectable } from '@angular/core';
import { Product } from '../core/models/Product';
import { Order } from '../core/models/Order';
import { Customer } from '../core/models/Customer';
import { OrderDetail } from '../core/models/OrderDetail';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private addToCartEvent = new EventEmitter();
  private orderCountEvent = new EventEmitter<number>();

  private order: Order;

  constructor(private authService: AuthService) {
    this.order = new Order(this.authService.getCustomer())
  }//TODO: need to add customer

  public addToCart(product: Product, productSize: string) {
    if (this.order.hasProduct(product)) {
      this.order.updateProductCount(product);
    } else {
      this.order.addNewProduct(product, productSize);
    }
    this.emitAddItemToCartEvent();
    this.emitOrderCountEvent();
  }

  removeFromCartAt(index: number) {
    this.order.removeOrderItemAt(index);
    this.emitOrderCountEvent();
  }

  public getCartItems() {
    return this.order.getOrderDetails();
  }

  private emitAddItemToCartEvent() {
    this.addToCartEvent.emit();
  }

  public getAddToCartEvent(): EventEmitter<Product> {
    return this.addToCartEvent;
  }

  public decrementProductCountFrom(orderDetail: OrderDetail) {
    this.order.decrementProductCountFrom(orderDetail);
    this.emitOrderCountEvent();
  }

  public incrementProductCountOf(orderDetail: OrderDetail) {
    this.order.incrementProductCountOf(orderDetail);
    this.emitOrderCountEvent();
  }

  private emitOrderCountEvent() {
    this.orderCountEvent.emit(this.order.getAllProductsCount());
  }

  public getOrderCountEvent() {
    return this.orderCountEvent;
  }
}
