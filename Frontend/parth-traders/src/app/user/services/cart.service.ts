import { EventEmitter, Injectable } from '@angular/core';
import { Product } from '../core/models/Product';
import { OrderDetail } from '../core/models/OrderDetail';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private addToCartEvent = new EventEmitter();
  private allProductCountEvent = new EventEmitter<number>();

  private orderDetails: OrderDetail[] = [];

  constructor() {
  }

  public addToCart(product: Product) {
    if (this.orderDetails.findIndex(_ => _.productSku == product.productSku) > -1) {
      this.orderDetails[this.orderDetails.findIndex(_ => _.productSku == product.productSku)].incrementProductCount();
    } else {
      this.orderDetails.push(new OrderDetail(product, 1, product.unitPrice));
    }
    this.emitAddItemToCartEvent();
    this.emitAllProductCountEvent();
  }

  removeFromCartAt(index: number) {
    this.orderDetails.splice(index, 1);
    this.emitAllProductCountEvent();
  }

  public getCartItems() {
    return this.orderDetails;
  }

  public isCartEmpty(): Boolean {
    return this.orderDetails.length > 0;
  }

  private emitAddItemToCartEvent() {
    this.addToCartEvent.emit();
  }

  public getAddToCartEvent(): EventEmitter<Product> {
    return this.addToCartEvent;
  }

  public decrementProductCountFrom(orderDetail: OrderDetail) {
    const index = this.orderDetails.findIndex(_ => _.productSku == orderDetail.productSku);
    if (this.orderDetails[index].getProductQuantity() == 1) {
      this.orderDetails.splice(index, 1);
    } else {
      this.orderDetails[index].decrementProductCountFrom();
    }
    this.emitAllProductCountEvent();
  }

  public incrementProductCountOf(orderDetail: OrderDetail) {
    const index = this.orderDetails.findIndex(_ => _.productSku == orderDetail.productSku);
    this.orderDetails[index].incrementProductCount();
    this.emitAllProductCountEvent();
  }

  public getTotalOrderPrice(): number {
    return this.orderDetails.reduce((acc, currentOrderDetail) => acc + currentOrderDetail.getTotalPrice(), 0);
  }

  private emitAllProductCountEvent() {
    this.allProductCountEvent.emit(this.orderDetails.reduce((acc, current) => acc + current.quantity, 0));
  }

  public getAllProductCountEvent() {
    return this.allProductCountEvent;
  }

  public emitClearCartEvent() {
    this.orderDetails = [];
    this.emitAllProductCountEvent();
  }
}
