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
    this.shouldDisplaySearchBox = this.router.url.includes('/user/home');
    productService.getCategories()
      .subscribe({next: categories => this.categories = ['All Categories', ...categories], error: () => {}});
    // console.log(this.categories);

    this.cartService.getAllProductCountEvent().subscribe((orderItemsCount: number) => {
      this.totalOrderItems = orderItemsCount;
    })

    this.customerName = authService.getCustomerName();

    router.events.subscribe((route) => {
      if (route instanceof NavigationEnd) {
        this.shouldDisplaySearchBox = route.urlAfterRedirects.includes('/user/home');
      }
    });
  }

  ngOnInit(): void {
  }

  signOut() { this.authService.logout(); }

  redirectToCheckout() {
    this.router.navigate(['/user/checkout']);
  }

  redirectToWIP() {
    this.router.navigate(['/user/WIP']);
  }

  searchProduct(key: string, searchValue: string) {

    const filterCriteria = {
      ...this.productService.productFilter$.value,
      name: searchValue,
      categories: this.selectedCategory === 'All Categories' ? [] : [this.selectedCategory],

    };
    if (key === 'clicked' || key === 'Enter' || (key === 'input' && searchValue === '')) {
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
