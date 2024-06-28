import { Component, ElementRef, NgZone, OnDestroy, OnInit, Renderer2, ViewChild } from '@angular/core';
import { CartService } from '../services/cart.service';
import { Product } from '../core/models/Product';
import { Subscription } from 'rxjs';
import { OrderDetail } from '../core/models/OrderDetail';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit, OnDestroy {
  shouldFlash: Boolean = true;

  @ViewChild('cart', { static: true }) cartRef!: ElementRef;

  private addToCartSubscription!: Subscription;

  constructor(private cartService: CartService, private ngZone: NgZone, private renderer: Renderer2) {
  }

  ngOnInit(): void {
    this.addToCartSubscription = this.cartService.getAddToCartEvent().subscribe(() => {
      this.flashLastProduct();
    });
  }

  ngOnDestroy(): void {
    if (this.addToCartSubscription) {
      this.addToCartSubscription.unsubscribe();
    }
  }

  getCartItems() {
    return this.cartService.getCartItems();
  }

  removeFromCart(index: number) {
    this.shouldFlash = false;
    this.cartService.removeFromCartAt(index);
  }

  incrementProductCountOf(orderDetail: OrderDetail) {
    this.cartService.incrementProductCountOf(orderDetail);
    this.flashLastProduct();
  }

  decrementProductCountFrom(orderDetail: OrderDetail) {
    this.cartService.decrementProductCountFrom(orderDetail);
    this.flashLastProduct();
  }

  //TODO: need to flash the last product when increment or decrement and add to cart is clicked
  flashLastProduct() {
    this.shouldFlash = true;
    setTimeout(() => {
      this.ngZone.run(() => {
        const lastCartItem = this.cartRef.nativeElement.querySelectorAll('.flash')[0];
        this.renderer.removeClass(lastCartItem, 'flash');
      });
    }, 10);
  }
}


