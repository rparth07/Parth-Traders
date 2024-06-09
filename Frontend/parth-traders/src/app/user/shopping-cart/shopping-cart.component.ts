import { AfterViewInit, Component, ElementRef, NgZone, OnChanges, OnDestroy, OnInit, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { CartService } from '../services/cart.service';
import { Product } from '../core/models/Product';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit, OnDestroy, AfterViewInit {
  shouldFlash: Boolean = true;
  cartItems: Product[] = [];
  @ViewChild('cartIconTop', { static: true }) cartIconTop!: ElementRef;
  @ViewChild('cart', { static: true }) cartRef!: ElementRef;

  private addToCartSubscription!: Subscription;

  constructor(private cartService: CartService, private ngZone: NgZone, private renderer: Renderer2) {
  }

  ngAfterViewInit(): void {
    this.cartService.setCartIconTop(this.cartIconTop);
    console.log('view updated!');
  }

  removeFromCart(index: number) {
    this.shouldFlash = false;
    this.cartItems.splice(index, 1);
  }

  ngOnInit(): void {
    this.addToCartSubscription = this.cartService.getAddToCartEvent().subscribe((product: Product) => {
      this.shouldFlash = true;
      this.cartItems.push(product);
      setTimeout(() => {
        this.ngZone.run(() => {
          const lastCartItem = this.cartRef.nativeElement.querySelectorAll('.flash')[0];
          this.renderer.removeClass(lastCartItem, 'flash');
        });
      }, 10);
    });
  }

  ngOnDestroy(): void {
    if (this.addToCartSubscription) {
      this.addToCartSubscription.unsubscribe();
    }
  }
}
