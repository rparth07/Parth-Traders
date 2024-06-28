import { ElementRef, EventEmitter, Injectable } from '@angular/core';
import { Product } from '../core/models/Product';
import { Order } from '../core/models/Order';
import { Customer } from '../core/models/Customer';
import { OrderDetail } from '../core/models/OrderDetail';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private addToCartEvent = new EventEmitter();
  private order: Order;

  constructor() { this.order = new Order(new Customer('', '', '', '')) }

  public addToCart(product: Product, productSize: string) {
    if (this.order.hasProduct(product)) {
      this.order.updateProductCount(product);
    } else {
      this.order.addNewProduct(product, productSize);
    }
    this.emitAddItemToCartEvent();
  }

  removeFromCartAt(index: number) {
    this.order.removeOrderItemAt(index);
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
  }

  public incrementProductCountOf(orderDetail: OrderDetail) {
    this.order.incrementProductCountOf(orderDetail);
  }

}
