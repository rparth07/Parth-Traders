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
  filterProducts: Product[] = [];
  shownProducts: Product[] = [];
  loading: boolean = false;
  isLargeGrid = false;
  pageSize = 6;

  @ViewChild('home', { static: true }) homeComponent!: ElementRef;

  constructor(private renderer: Renderer2, private ngZone: NgZone, private productService: ProductService, private cartService: CartService) {
    this.fetchProducts();
  }

  ngOnInit(): void {
    this.productService.productFilter$.subscribe(filterCriteria => {
      // if (this.filterProducts.length <= 0)
      //   this.filterProducts = this.products;
      if (filterCriteria) {
        this.filterProducts = this.products.filter(_ => {
          // console.log('active: ' + filterCriteria.activeProductType, " and current: ", _.productType);
          return (filterCriteria.name === '' || _.productName.trim().toLowerCase().includes(filterCriteria.name.trim().toLowerCase()))
            && (filterCriteria.categories.length === 0 || filterCriteria.categories.findIndex(c => c === _.categoryName) > -1)
            && (!filterCriteria.activeProductType || filterCriteria.activeProductType === _.productType)
            && (!filterCriteria.priceRange.min || filterCriteria.priceRange.min <= _.unitPrice)
            && (!filterCriteria.priceRange.max || filterCriteria.priceRange.max >= _.unitPrice)
            && (!filterCriteria.ratingRange.min || filterCriteria.ratingRange.min <= _.productRating)
            && (!filterCriteria.ratingRange.max || filterCriteria.ratingRange.max >= _.productRating)
        });
      }
      this.refreshShownProduct();
    });
  }

  refreshShownProduct() {
    this.shownProducts = this.filterProducts.slice(0, this.pageSize);
  }

  async onScroll() {
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
      this.cartService.addToCart(product);
    }, 1100);
  }

  onAddToCartLarge({ productCard, product }: { productCard: HTMLDivElement, product: Product }) {
    setTimeout(() => {
      this.cartService.addToCart(product);
    }, 1100);
  }

  fetchProducts(): void {
    this.productService.getProducts()
      .subscribe((next) => {
        this.products = next;
        // console.dir(this.products, { depth: null }); // Now you'll see the products in the console
        this.filterProducts = this.products;
        // console.log(this.filterProducts);
        this.refreshShownProduct(); // Call this to refresh the shown products
      });
  }
}

