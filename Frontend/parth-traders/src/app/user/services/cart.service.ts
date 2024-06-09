import { ElementRef, EventEmitter, Injectable } from '@angular/core';
import { Product } from '../core/models/Product';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private addToCartEvent = new EventEmitter<Product>();

  constructor() { }

  emitAddItemToCartEvent(product: Product) {
    this.addToCartEvent.emit(product);
  }
  getAddToCartEvent(): EventEmitter<Product> {
    return this.addToCartEvent;
  }
}
