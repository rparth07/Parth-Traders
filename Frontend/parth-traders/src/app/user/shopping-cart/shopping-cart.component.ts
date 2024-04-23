import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit {

  cartItems: any[];

  constructor(private productService: ProductService) {
    this.cartItems = this.productService.getCartItems();
  }

  removeFromCart(index: number) {
    this.productService.removeFromCart(index);
    this.cartItems = this.productService.getCartItems();
  }

  ngOnInit(): void {
  }

}
