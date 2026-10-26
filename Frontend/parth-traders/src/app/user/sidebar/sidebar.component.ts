import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { ProductService } from '../services/product.service';
import { ProductType } from '../core/enums/ProductType';
import { FilterCriteria } from '../core/models/FilterCriteria';

@Component({ selector: 'app-sidebar', templateUrl: './sidebar.component.html', styleUrls: ['./sidebar.component.css'] })
export class SidebarComponent implements OnInit, OnDestroy {
  @Output() filtersApplied = new EventEmitter<void>();
  private subscriptions = new Subscription();
  categories: string[] = [];
  productTypeKeys = Object.values(ProductType);
  ratingOptions = [4.9, 4.8, 4.5, 4, 3, 2, 1];
  filterCriteria: FilterCriteria;
  minValue: number | null = null;
  maxValue: number | null = null;
  matchingCount: number | null = null;
  priceError = '';

  constructor(private productService: ProductService) { this.filterCriteria = productService.productFilter$.value; }
  ngOnInit(): void {
    this.subscriptions.add(this.productService.getCategories().subscribe({next: categories => this.categories = categories, error: () => {}}));
    this.subscriptions.add(this.productService.productFilter$.subscribe(criteria => {
      this.filterCriteria = criteria;
      this.minValue = criteria.priceRange.min;
      this.maxValue = criteria.priceRange.max;
    }));
    this.subscriptions.add(this.productService.matchingCount$.subscribe(count => this.matchingCount = count));
  }
  ngOnDestroy() { this.subscriptions.unsubscribe(); }
  isChecked(category: string) { return this.filterCriteria.categories.includes(category); }
  toggleCategory(event: Event, category: string) {
    const current = this.productService.productFilter$.value;
    this.productService.productFilter$.next({...current, categories: current.categories.includes(category) ? current.categories.filter(c => c !== category) : [...current.categories, category]});
  }
  setActiveProductType(type: ProductType) {
    const current = this.productService.productFilter$.value;
    this.productService.productFilter$.next({...current, activeProductType: current.activeProductType === type ? null : type});
  }
  updatePrice(bound: 'min' | 'max', value: number | null) {
    // Use the emitted input value explicitly; do not depend on two-way binding event order.
    if (bound === 'min') this.minValue = value; else this.maxValue = value;
    this.applyPrice(false);
  }
  applyPrice(showResults = true) {
    if ([this.minValue, this.maxValue].some(v => v !== null && (!Number.isFinite(v) || v < 0))) {
      this.priceError = 'Enter a price of zero or more. The previous valid range is still applied.'; return;
    }
    if (this.minValue !== null && this.maxValue !== null && this.minValue > this.maxValue) {
      this.priceError = 'Minimum must not exceed maximum. The previous valid range is still applied.'; return;
    }
    this.priceError = '';
    const current = this.productService.productFilter$.value;
    this.productService.productFilter$.next({...current, priceRange: {min: this.minValue, max: this.maxValue}});
    if (showResults) this.filtersApplied.emit();
  }
  clearPrice() {
    this.minValue = this.maxValue = null; this.priceError = '';
    this.applyPrice(false);
  }
  updateMinRating(rating: number | null) {
    const current = this.productService.productFilter$.value;
    this.productService.productFilter$.next({...current, ratingRange: {...current.ratingRange, min: rating}});
  }
  clearRating() { this.updateMinRating(null); }
  resetFilters() { this.priceError = ''; this.productService.resetFilters(); }
}
