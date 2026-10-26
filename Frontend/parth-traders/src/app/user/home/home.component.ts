import { Component, ElementRef, NgZone, OnInit, OnDestroy, Renderer2, ViewChild } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Subscription } from 'rxjs';
import { discountedUnitPrice } from '../../shared/pricing';

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
export class HomeComponent implements OnInit, OnDestroy {
  private subscriptions = new Subscription();
  loadError = '';
  filtersOpen = false;
  fetching = false;
  products: Product[] = [];
  filterProducts: Product[] = [];
  shownProducts: Product[] = [];
  loading: boolean = false;
  isLargeGrid = false;
  pageSize = 6;

  @ViewChild('home', { static: true }) homeComponent!: ElementRef;

  constructor(private renderer: Renderer2, private ngZone: NgZone, private productService: ProductService, private cartService: CartService) {

  }

  ngOnInit(): void {
    this.subscriptions.add(this.productService.productFilter$.subscribe(() => this.applyFilters()));
    this.fetchProducts();
  }

  ngOnDestroy() { this.subscriptions.unsubscribe(); }

  applyFilters() {
    const f = this.productService.productFilter$.value;
    this.filterProducts = this.products.filter(p =>
      (!f.name || p.productName.trim().toLowerCase().includes(f.name.trim().toLowerCase())) &&
      (!f.categories.length || f.categories.includes(p.categoryName)) &&
      (!f.activeProductType || f.activeProductType === p.productType) &&
      (f.priceRange.min === null || discountedUnitPrice(p.unitPrice, p.discount) >= f.priceRange.min) &&
      (f.priceRange.max === null || discountedUnitPrice(p.unitPrice, p.discount) <= f.priceRange.max) &&
      (f.ratingRange.min === null || p.productRating >= f.ratingRange.min) &&
      (f.ratingRange.max === null || p.productRating <= f.ratingRange.max));
    this.refreshShownProduct();
  }

  refreshShownProduct() {
    this.shownProducts = this.filterProducts.slice(0, this.pageSize);
  }

  async onScroll() {
    if (this.loading || this.shownProducts.length >= this.filterProducts.length) return;
    this.loading = true;
    try {
      await this.delay(1000);

      const startIndex = this.shownProducts.length;
      const endIndex = startIndex + this.pageSize;
      const newProducts = this.filterProducts.slice(startIndex, endIndex);
      this.isLargeGrid ? this.switchToLargeGrid() : this.switchToSmallGrid();

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
  }

  switchToSmallGrid() {
    this.isLargeGrid = false;
  }

  onAddToCart({ productCard, product }: { productCard: HTMLDivElement, product: Product }) {
    this.cartService.addToCart(product);
    const position = productCard.getBoundingClientRect();
    const actualTop = position.top + window.scrollY;
    const actualLeft = position.left + window.scrollX;
    const floatingCart = this.renderer.createElement('div');
    this.renderer.addClass(floatingCart, 'floating-cart');

    const clone = productCard.cloneNode(true);
    this.renderer.setStyle(clone, 'width', '100%');
    this.renderer.setStyle(clone, 'position', 'initial');
    this.renderer.setStyle(floatingCart, 'top', `${actualTop}px`);
    this.renderer.setStyle(floatingCart, 'left', `${actualLeft}px`);
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
    }, 1100);
  }

  onAddToCartLarge({ productCard, product }: { productCard: HTMLDivElement, product: Product }) {
    this.cartService.addToCart(product);
  }

  fetchProducts(): void {
    if (this.fetching) return;
    this.fetching = true;
    this.loadError = '';
    this.subscriptions.add(this.productService.getProducts().subscribe({
      next: products => {
        this.products = products;
        this.fetching = false;
        this.applyFilters();
      },
      error: () => {
        this.fetching = false;
        this.loadError = 'We cannot reach the product service. Please try again or contact Parth Traders for current products and prices.';
      }
    }));
  }
}
