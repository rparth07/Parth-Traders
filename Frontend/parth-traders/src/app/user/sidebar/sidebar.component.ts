import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { ProductType } from '../core/enums/ProductType';
import { FilterCriteria } from '../core/models/FilterCriteria';
import { Options } from '@angular-slider/ngx-slider';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent implements OnInit {
  categories: string[] = [];
  checkedCategories: { [key: string]: boolean } = {};
  animateX: { [key: string]: boolean } = {};
  animateY: { [key: string]: boolean } = {};

  productTypeKeys = Object.values(ProductType);
  filterCriteria: FilterCriteria;

  minValue: number = 10;
  maxValue: number = 5000;
  sliderOptions: Options = {
    floor: 10,
    ceil: 10000,
    step: 10
  };

  constructor(private productService: ProductService) {
    this.categories = productService.getCategories();
    this.filterCriteria = {
      name: '',
      categories: this.getCheckedCategories(),
      activeProductType: null,
      priceRange: {
        min: null,
        max: null
      },
      ratingRange: {
        min: null,
        max: 5
      }
    };
  }

  ngOnInit(): void {
  }

  triggerProductFilterEvent() {
    this.productService.productFilter$.next(this.filterCriteria);
  }

  toggleCategory(event: Event, category: string): void {
    event.preventDefault();
    if (this.checkedCategories[category]) {
      this.animateY[category] = false;
      setTimeout(() => {
        this.animateX[category] = false;
      }, 50);
      this.checkedCategories[category] = false;
    } else {
      this.animateX[category] = true;
      setTimeout(() => {
        this.animateY[category] = true;
      }, 100);
      this.checkedCategories[category] = true;
    }
    this.filterCriteria.categories = this.getCheckedCategories();
    this.triggerProductFilterEvent();
  }

  isChecked(category: string): boolean {
    return !!this.checkedCategories[category];
  }

  isAnimateX(category: string): boolean {
    return !!this.animateX[category];
  }

  isAnimateY(category: string): boolean {
    return !!this.animateY[category];
  }

  getCheckedCategories(): string[] {
    return Object.keys(this.checkedCategories).filter(key => this.checkedCategories[key]);
  }

  setActiveProductType(type: ProductType): void {
    this.filterCriteria.activeProductType = this.filterCriteria.activeProductType === type ? null : type;
    this.triggerProductFilterEvent();
  }

  filterByPrice() {
    this.filterCriteria.priceRange.min = this.minValue;
    this.filterCriteria.priceRange.max = this.maxValue;
    this.triggerProductFilterEvent();
  }

  updateMinRating(rating: number) {
    this.filterCriteria.ratingRange.min = rating;
    this.triggerProductFilterEvent();
  }

  clearRating() {
    this.filterCriteria.ratingRange.min = null;
    this.triggerProductFilterEvent();
  }
}
