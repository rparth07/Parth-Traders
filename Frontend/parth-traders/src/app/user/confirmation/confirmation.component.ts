import { Component, OnInit } from '@angular/core';
import { OrderService } from '../services/order.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrl: './confirmation.component.css'
})
export class ConfirmationComponent implements OnInit {

  transactionId = "";

  constructor(private orderService: OrderService, private router: Router) { }

  ngOnInit(): void {
    this.transactionId = this.orderService.getTransactionId();
    if (!this.transactionId)
      this.router.navigate(['/user/home']);
  }
}
