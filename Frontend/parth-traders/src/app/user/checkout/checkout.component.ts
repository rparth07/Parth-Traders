import { Component, OnInit } from "@angular/core";
import { CartService } from "../services/cart.service";
import { OrderDetail } from "../core/models/OrderDetail";
import { OrderService } from "../services/order.service";

@Component({
  selector: "app-checkout",
  styleUrls: ["./checkout.component.css"],
  templateUrl: "./checkout.component.html"
})
export class CheckoutComponent implements OnInit {
  totalOrderPrice: number = 0;
  orderItems: OrderDetail[] = [];

  public constructor(private orderService: OrderService) {
  }

  public ngOnInit(): void {
    this.orderService.initializeOrder();
    this.orderItems = this.orderService.getOrderItems();
    this.totalOrderPrice = this.orderService.getTotalOrderPrice();
  }

  getImgPathFromSku(sku: string) {
    return `assets/products/${sku}/${sku}_1.jpg`;
  }
}