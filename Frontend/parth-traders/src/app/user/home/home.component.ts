import { Component, Renderer2, NgZone, OnInit, OnDestroy, ElementRef, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { discountedUnitPrice } from '../../shared/pricing';
import { ProductService } from '../services/product.service';
import { Product } from '../core/models/Product';
import { CartService } from '../services/cart.service';
@Component({ selector: 'app-home', templateUrl: './home.component.html', styleUrls: ['./home.component.css'] })
export class HomeComponent implements OnInit, OnDestroy {
    private subscriptions = new Subscription();
    loadError = '';
    filtersOpen = false;
    fetching = false;
    loading = false;
    isLargeGrid = false;
    pageSize = 12;
    sortOrder = 'default';
    cartMessage = '';
    activeFilters: string[] = [];
    @ViewChild('resultsHeading') resultsHeading?: ElementRef<HTMLElement>;
    products: Product[] = [];
    filterProducts: Product[] = [];
    shownProducts: Product[] = [];
    constructor(private renderer: Renderer2, private ngZone: NgZone, private productService: ProductService, private cartService: CartService) { }
    ngOnInit() { this.subscriptions.add(this.productService.productFilter$.subscribe(() => this.applyFilters())); this.fetchProducts(); }
    ngOnDestroy() { this.subscriptions.unsubscribe(); }
    applyFilters() {
        const f = this.productService.productFilter$.value;
        this.activeFilters = [];
        if (f.priceRange.min !== null) this.activeFilters.push('Minimum price: $' + f.priceRange.min.toFixed(2));
        if (f.priceRange.max !== null) this.activeFilters.push('Maximum price: $' + f.priceRange.max.toFixed(2));
        if (f.ratingRange.min !== null) this.activeFilters.push('Rating: ' + f.ratingRange.min + ' stars and up');
        this.filterProducts = this.products.filter(p => (!f.name || (p.productName + ' ' + p.productSku + ' ' + p.productDescription).toLowerCase().includes(f.name.trim().toLowerCase())) && (!f.categories.length || f.categories.includes(p.categoryName)) && (!f.activeProductType || f.activeProductType === p.productType) && (f.priceRange.min === null || discountedUnitPrice(p.unitPrice, p.discount) >= f.priceRange.min) && (f.priceRange.max === null || discountedUnitPrice(p.unitPrice, p.discount) <= f.priceRange.max) && (f.ratingRange.min === null || p.productRating >= f.ratingRange.min) && (f.ratingRange.max === null || p.productRating <= f.ratingRange.max));
        this.filterProducts.sort((a, b) => this.sortOrder === 'price-asc' ? discountedUnitPrice(a.unitPrice, a.discount) - discountedUnitPrice(b.unitPrice, b.discount) : this.sortOrder === 'price-desc' ? discountedUnitPrice(b.unitPrice, b.discount) - discountedUnitPrice(a.unitPrice, a.discount) : this.sortOrder === 'name' ? a.productName.localeCompare(b.productName) : this.sortOrder === 'rating' ? b.productRating - a.productRating : 0);
        this.productService.matchingCount$.next(this.fetching ? null : this.filterProducts.length);
        this.refreshShownProduct();
    }
    refreshShownProduct() { this.shownProducts = this.filterProducts.slice(0, this.pageSize); }
    onScroll() { if (this.shownProducts.length >= this.filterProducts.length)
        return; this.shownProducts = this.filterProducts.slice(0, this.shownProducts.length + this.pageSize); }
    switchToLargeGrid() { this.isLargeGrid = true; }
    switchToSmallGrid() { this.isLargeGrid = false; }
    showFilterResults() {
        this.filtersOpen = false;
        // Wait for the mobile panel to leave layout before positioning the results.
        setTimeout(() => {
            const heading = this.resultsHeading?.nativeElement;
            if (!heading?.isConnected) return;
            heading.scrollIntoView({block: 'start', behavior: 'instant' as ScrollBehavior});
            heading.focus({preventScroll: true});
        }, 0);
    }
    resetFilters() { this.productService.resetFilters(); }
    onAddToCart({ product }: {
        productCard: HTMLDivElement;
        product: Product;
    }) { const added = this.cartService.addToCart(product); this.cartMessage = added ? product.productName + ' added to your cart.' : 'The available stock limit for ' + product.productName + ' is already in your cart.'; }
    onAddToCartLarge(event: {
        productCard: HTMLDivElement;
        product: Product;
    }) { this.onAddToCart(event); }
    fetchProducts() { if (this.fetching)
        return; this.fetching = true; this.productService.matchingCount$.next(null); this.loadError = ''; this.subscriptions.add(this.productService.getProducts().subscribe({ next: products => { this.products = products; this.fetching = false; this.applyFilters(); }, error: () => { this.fetching = false; this.loadError = 'We could not reach the product service. Try again or contact Parth Traders for help.'; } })); }
}
