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
  loading = false;
  loadError = '';
  selectedOrder: Order | null = null;
  displayedColumns: string[] = ['transactionId', 'userName', 'grandTotal', 'actions'];
  dataSource: MatTableDataSource<Order> = new MatTableDataSource<Order>();

  @ViewChild(MatPaginator) set paginator(value: MatPaginator) { this.dataSource.paginator = value; }
  @ViewChild(MatSort) set sort(value: MatSort) { this.dataSource.sort = value; }

  constructor(private orderService: OrderService) {
  }

  ngOnInit(): void {
    this.fetchOrders();
  }

  ngAfterViewInit(): void {

  }

  fetchOrders(): void {
    this.loading = true;
    this.loadError = '';
    this.orderService.getOrders().subscribe({next: (data: Order[]) => {
      this.loading = false;
      this.orders = data;
      this.dataSource.data = this.orders;

    }, error: () => { this.loading = false; this.loadError = 'Orders could not be loaded. Please try again later.'; }});
  }

  onSelectOrder(order: Order): void {
    this.selectedOrder = order; // Set the selected order
  }
}
