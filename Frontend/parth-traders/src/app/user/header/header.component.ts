import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  searchInput: string;
  selectedCategory: string;

  categories: string[];

  constructor(private productService: ProductService) {
    this.searchInput = '';
    this.selectedCategory = 'All Categories';
    this.categories = productService.getCategories();
    this.categories.push('All Categories');
  }

  ngOnInit(): void {
  }

  searchProduct(event: Event) {
    const filterCriteria = {
      name: this.searchInput,
      categories: [this.selectedCategory === 'All Categories' ? '' : this.selectedCategory],
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

    if ((event as KeyboardEvent).key === 'Enter' || (event as MouseEvent)) {//Find a way to identify mouse and keyboard event
      if (this.searchInput.trim() !== '') {
        this.productService.productFilter$.next(filterCriteria);
      }
    }
  }
}
