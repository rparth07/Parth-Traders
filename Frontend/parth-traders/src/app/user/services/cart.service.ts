import { ElementRef, EventEmitter, Injectable } from '@angular/core';
import { Product } from '../core/models/Product';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartIconTop!: ElementRef;
  private addToCartEvent = new EventEmitter<Product>();

  constructor() { }

  emitAddItemToCartEvent(product: Product) {
    this.addToCartEvent.emit(product);
  }

  setCartIconTop(ref: ElementRef) {
    this.cartIconTop = ref;
  }

  getCartIconTop(): ElementRef {
    return this.cartIconTop;
  }

  getAddToCartEvent(): EventEmitter<Product> {
    return this.addToCartEvent;
  }
}
