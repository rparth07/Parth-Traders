import { Component, ElementRef, NgZone, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { CartService } from '../services/cart.service';
import { Subscription } from 'rxjs';
import { OrderDetail } from '../core/models/OrderDetail';
import { Router } from '@angular/router';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit, OnDestroy {
  flashIndex: number = -1;

  @ViewChild('cart', { static: true }) cartRef!: ElementRef;

  private addToCartSubscription!: Subscription;

  constructor(private cartService: CartService, private ngZone: NgZone, private router: Router) {
  }

  ngOnInit(): void {
    this.addToCartSubscription = this.cartService.getAddToCartEvent().subscribe(() => {
      setTimeout(() => {
        this.flashProductAt(this.getCartItems().length - 1);
      }, 10);
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
    this.flashIndex = -1;
    this.cartService.removeFromCartAt(index);
  }

  incrementProductCountOf(orderDetail: OrderDetail) {
    this.cartService.incrementProductCountOf(orderDetail);
    this.flashProductAt(this.getCartItems().indexOf(orderDetail));
  }

  decrementProductCountFrom(orderDetail: OrderDetail) {
    this.cartService.decrementProductCountFrom(orderDetail);
    this.flashProductAt(this.getCartItems().indexOf(orderDetail));
  }

  flashProductAt(productIndex: number) {
    this.ngZone.run(() => {
      this.flashIndex = productIndex;
      setTimeout(() => {
        this.flashIndex = -1;
      }, 700);
    });
  }

  redirectToCheckout() {
    this.router.navigate(['/user/checkout']);
  }

  getImgPathFromSku(sku: string) {
    return `assets/products/${sku}/${sku}_1.jpg`;
  }
}