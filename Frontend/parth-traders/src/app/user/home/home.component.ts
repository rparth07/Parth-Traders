import { Component, ElementRef, EventEmitter, NgZone, OnInit, Renderer2, ViewChild } from '@angular/core';
import { ProductService } from '../services/product.service';
import { Product } from '../core/models/Product';
import { Subject } from 'rxjs';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  animations: [
    trigger('fadeInOut', [
      state('void', style({ opacity: 0 })), // Initial state
      transition(':enter, :leave', [animate(300)]), // Animation duration (300ms)
    ]),
  ],
})
export class HomeComponent implements OnInit {

  products: Product[] = [];
  isLargeGrid = false;
  changeGridToLargeSubject$: Subject<boolean> = new Subject<boolean>();

  @ViewChild('home', { static: true }) homeComponent!: ElementRef;

  constructor(private renderer: Renderer2, private ngZone: NgZone, private productService: ProductService, private cartService: CartService) {
    this.products = productService.getProducts();
  }

  ngOnInit(): void {
  }

  switchToLargeGrid() {
    this.isLargeGrid = true;
    this.changeGridToLargeSubject$.next(true);
  }

  switchToSmallGrid() {
    this.isLargeGrid = false;
    this.changeGridToLargeSubject$.next(false);
  }

  onAddToCart({ productCard, product }: { productCard: HTMLDivElement, product: Product }) {
    console.log('add to cart in home component is called!');
    const position = productCard.getBoundingClientRect();
    const floatingCart = this.renderer.createElement('div');
    this.renderer.addClass(floatingCart, 'floating-cart');

    const clone = productCard.cloneNode(true);
    this.renderer.setStyle(clone, 'width', '100%');
    this.renderer.setStyle(clone, 'position', 'initial');
    console.log('created clone: ', clone);
    this.renderer.setStyle(floatingCart, 'top', `${position.top}px`);
    this.renderer.setStyle(floatingCart, 'left', `${position.left}px`);
    this.renderer.setStyle(floatingCart, 'display', 'block');
    this.renderer.appendChild(floatingCart, clone);
    this.renderer.appendChild(this.homeComponent.nativeElement, floatingCart);

    // this.renderer.setStyle(floatingCart, 'opacity', 0);
    // this.renderer.setStyle(floatingCart, 'transition', 'opacity 0.5s'); // adjust the transition duration to your liking
    // setTimeout(() => {
    //   this.renderer.setStyle(floatingCart, 'opacity', 1);
    // }, 0);

    setTimeout(() => {
      this.ngZone.run(() => {
        this.renderer.addClass(floatingCart, 'moveToCart');//TODO
      });
    }, 100);

    setTimeout(() => {
      this.ngZone.run(() => {
        this.renderer.addClass(this.homeComponent.nativeElement, 'MakeFloatingCart');//TODO
      });
    }, 900);

    setTimeout(() => {
      this.renderer.removeChild(this.homeComponent.nativeElement, floatingCart);
      this.renderer.removeClass(this.homeComponent.nativeElement, 'MakeFloatingCart');
      this.cartService.emitAddItemToCartEvent(product);
    }, 1100);
  }
}