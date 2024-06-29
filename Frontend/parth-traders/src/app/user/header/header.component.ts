import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  selectedCategory: string;
  totalOrderItems: number = 0;

  categories: string[];

  constructor(private productService: ProductService, private cartService: CartService) {
    this.selectedCategory = 'All Categories';
    this.categories = productService.getCategories();
    this.categories.push('All Categories');

    this.cartService.getOrderCountEvent().subscribe((orderItemsCount: number) => {
      this.totalOrderItems = orderItemsCount;
    })
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
