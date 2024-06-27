import { Component, ElementRef, NgZone, OnInit, Renderer2, ViewChild } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Subject } from 'rxjs';

import { ProductService } from '../services/product.service';
import { Product } from '../core/models/Product';
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
  shownProducts: Product[] = [];
  loading: boolean = false;
  isLargeGrid = false;
  changeGridToLargeSubject$: Subject<boolean> = new Subject<boolean>();
  pageSize = 6;

  @ViewChild('home', { static: true }) homeComponent!: ElementRef;

  constructor(private renderer: Renderer2, private ngZone: NgZone, private productService: ProductService, private cartService: CartService) {
    this.products = productService.getProducts();
    this.refreshShownProduct();
  }

  ngOnInit(): void {
    this.productService.productFilter$.subscribe(next => {
      this.products = this.productService.getProducts();
      if (next) {
        this.products = this.products.filter(_ => {
          return (next.name === '' || _.productName.trim().toLowerCase().includes(next.name.trim().toLowerCase()))
            && (next.categories.length === 0 || next.categories.findIndex(c => c === _.category) > -1)
            && (!next.activeProductType || next.activeProductType === _.productType)
            && (!next.priceRange.min || next.priceRange.min <= _.price)
            && (!next.priceRange.max || next.priceRange.max >= _.price)
            && (!next.ratingRange.min || next.ratingRange.min <= _.rating)
            && (!next.ratingRange.max || next.ratingRange.max >= _.rating)
        });
      }
      this.refreshShownProduct();
    });
  }

  refreshShownProduct() {
    this.shownProducts = this.products.slice(0, this.pageSize);
  }

  async onScroll() {
    this.loading = true;
    try {
      await this.delay(800);

      const startIndex = this.shownProducts.length;
      const endIndex = startIndex + this.pageSize;
      const newProducts = this.products.slice(startIndex, endIndex);

      this.shownProducts = [...this.shownProducts, ...newProducts];
    } finally {
      this.loading = false;
    }
  }

  private delay(ms: number): Promise<void> {//TODO: need to globalize this to use it all places instead of timeout
    return new Promise(resolve => setTimeout(resolve, ms));
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
    const position = productCard.getBoundingClientRect();
    const floatingCart = this.renderer.createElement('div');
    this.renderer.addClass(floatingCart, 'floating-cart');

    const clone = productCard.cloneNode(true);
    this.renderer.setStyle(clone, 'width', '100%');
    this.renderer.setStyle(clone, 'position', 'initial');
    this.renderer.setStyle(floatingCart, 'top', `${position.top}px`);
    this.renderer.setStyle(floatingCart, 'left', `${position.left}px`);
    this.renderer.appendChild(floatingCart, clone);
    this.renderer.appendChild(this.homeComponent.nativeElement, floatingCart);

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