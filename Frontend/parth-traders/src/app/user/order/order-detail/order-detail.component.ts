import { Component, Input } from '@angular/core';
import { Order } from '../../core/models/Order';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrl: './order-detail.component.css'
})
export class OrderDetailComponent {
  @Input() order!: Order; // Receive the selected order
}
