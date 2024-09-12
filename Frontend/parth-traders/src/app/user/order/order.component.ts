import { Component } from '@angular/core';
import { Order } from '../core/models/Order';
import { OrderService } from '../services/order.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})
export class OrderComponent {
  orders: Order[] = []; // Store fetched orders
  selectedOrder: Order | null = null; // To track the selected order

  constructor(private orderService: OrderService) { }

  ngOnInit(): void {
    this.fetchOrders();
  }

  fetchOrders(): void {
    this.orderService.getOrders().subscribe((data: Order[]) => {
      this.orders = data;
    });
  }

  onSelectOrder(order: Order): void {
    this.selectedOrder = order; // Set the selected order
  }
}
