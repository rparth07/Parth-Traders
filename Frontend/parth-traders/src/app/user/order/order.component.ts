import { Component, ViewChild } from '@angular/core';
import { Order } from '../core/models/Order';
import { OrderService } from '../services/order.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})
export class OrderComponent {
  orders: Order[] = [];
  selectedOrder: Order | null = null;
  displayedColumns: string[] = ['transactionId', 'userName', 'grandTotal', 'actions'];
  dataSource: MatTableDataSource<Order>;
  totalOrders: number = 0; // Total records
  pageSize = 10; // Default page size
  pageIndex = 0; // Default starting page

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private orderService: OrderService) {
    this.dataSource = new MatTableDataSource(this.orders);
  }

  ngOnInit(): void {
    this.fetchOrders();
  }

  fetchOrders(): void {
    this.orderService.getOrders().subscribe((data: Order[]) => {
      this.orders = data;
      this.totalOrders = data.length; // Set total records
      this.dataSource.data = this.orders;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  onSelectOrder(order: Order): void {
    this.selectedOrder = order; // Set the selected order
  }
}
