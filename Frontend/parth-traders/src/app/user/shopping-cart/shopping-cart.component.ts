import { ChangeDetectorRef, Component, ElementRef, NgZone, OnDestroy, OnInit, Renderer2, ViewChild } from '@angular/core';
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
  flashIndex: number = -1;

  @ViewChild('cart', { static: true }) cartRef!: ElementRef;

  private addToCartSubscription!: Subscription;

  constructor(private cartService: CartService, private ngZone: NgZone) {
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
}