import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
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
export class OrderComponent implements OnInit, AfterViewInit {
  orders: Order[] = [];
  selectedOrder: Order | null = null;
  displayedColumns: string[] = ['transactionId', 'userName', 'grandTotal', 'actions'];
  dataSource: MatTableDataSource<Order> = new MatTableDataSource<Order>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private orderService: OrderService) {
  }

  ngOnInit(): void {
    this.fetchOrders();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  fetchOrders(): void {
    this.orderService.getOrders().subscribe((data: Order[]) => {
      this.orders = data;
      this.dataSource.data = this.orders;

      if (this.paginator && this.sort) {
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }
    });
  }

  onSelectOrder(order: Order): void {
    this.selectedOrder = order; // Set the selected order
  }
}
