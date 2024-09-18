import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { CartService } from '../services/cart.service';
import { AuthService } from '../services/auth.service';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  selectedCategory: string;
  totalOrderItems: number = 0;
  customerName: string = '';
  shouldDisplaySearchBox: boolean = true;

  categories: string[] = ['All Categories'];

  constructor(private productService: ProductService, private cartService: CartService, private authService: AuthService, private router: Router) {
    this.selectedCategory = 'All Categories';
    productService.getCategories()
      .subscribe(categories => this.categories.push(...categories));
    // console.log(this.categories);

    this.cartService.getAllProductCountEvent().subscribe((orderItemsCount: number) => {
      this.totalOrderItems = orderItemsCount;
    })

    this.customerName = authService.getCustomerName();

    router.events.subscribe((route) => {
      if (route instanceof NavigationEnd) {
        this.shouldDisplaySearchBox = route.url.includes('user/home');
      }
    });
  }

  ngOnInit(): void {
  }

  redirectToCheckout() {
    this.router.navigate(['/user/checkout']);
  }

  redirectToWIP() {
    this.router.navigate(['/user/WIP']);
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

  isAuthenticated(): Boolean {
    return this.authService.isAuthenticated();
  }

  redirectToLogIn(): void {
    this.authService.redirectToLogIn();
  }

  redirectToPastOrders(): void {
    this.router.navigate(['/user/orders']);
  }
}
