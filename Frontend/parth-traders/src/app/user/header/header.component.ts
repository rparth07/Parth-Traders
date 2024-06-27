import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  selectedCategory: string;

  categories: string[];

  constructor(private productService: ProductService) {
    this.selectedCategory = 'All Categories';
    this.categories = productService.getCategories();
    this.categories.push('All Categories');
  }

  ngOnInit(): void {
  }

  searchProduct(key: string, searchValue: string) {
    if (key === 'Backspace' && searchValue.length === 1)
      searchValue = '';

    const filterCriteria = {
      name: searchValue,
      categories: this.selectedCategory === 'All Categories' ? [] : [this.selectedCategory],
      activeProductType: null,
      priceRange: {
        min: null,
        max: null
      },
      ratingRange: {
        min: null,
        max: null
      }
    };
    if (key === 'clicked' || key === 'Enter' || (key === 'Backspace' && searchValue === '')) {
      this.productService.productFilter$.next(filterCriteria);
    }
  }
}
