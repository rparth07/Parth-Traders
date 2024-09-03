import { Component, OnInit } from "@angular/core";
import { CartService } from "../services/cart.service";
import { OrderDetail } from "../core/models/OrderDetail";

@Component({
  selector: "app-checkout",
  styleUrls: ["./checkout.component.css"],
  templateUrl: "./checkout.component.html"
})
export class CheckoutComponent implements OnInit {
  totalOrderPrice: number = 0;
  cartItems: OrderDetail[] = [];

  public constructor(private shoppingCartService: CartService) {
  }

  public ngOnInit(): void {
    this.cartItems = this.shoppingCartService.getCartItems();
    this.totalOrderPrice = this.shoppingCartService.getTotalOrderPrice();
  }
}