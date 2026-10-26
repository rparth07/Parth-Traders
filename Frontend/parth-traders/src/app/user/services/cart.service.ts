import { discountedUnitPrice } from '../../shared/pricing';
import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Product } from '../core/models/Product';
import { OrderDetail } from '../core/models/OrderDetail';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private addToCartEvent = new EventEmitter();
  private allProductCountEvent = new BehaviorSubject<number>(0);

  private orderDetails: OrderDetail[] = [];

  constructor() {
    try {
      const saved = JSON.parse(sessionStorage.getItem('parth_cart') || '[]');
      if (Array.isArray(saved)) this.orderDetails = saved.filter(item => typeof item.productSku === 'string' && Number.isInteger(item.quantity) && item.quantity > 0 && Number.isFinite(item.price) && Number.isFinite(item.discount)).map(item => {
        const restored = Object.assign(Object.create(OrderDetail.prototype), item) as OrderDetail;
        restored.totalPrice = Math.round(restored.quantity * discountedUnitPrice(restored.price, restored.discount) * 100) / 100;
        return restored;
      });
    } catch { this.orderDetails = []; }
    this.emitAllProductCountEvent();
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
    return this.orderDetails.length === 0;
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
    try { sessionStorage.setItem('parth_cart', JSON.stringify(this.orderDetails)); } catch {}
    this.allProductCountEvent.next(this.orderDetails.reduce((acc, current) => acc + current.quantity, 0));
  }

  public getAllProductCountEvent() {
    return this.allProductCountEvent;
  }

  public emitClearCartEvent() {
    this.orderDetails = [];
    this.emitAllProductCountEvent();
  }
}
