import { Component, OnInit } from '@angular/core';
import { OrderService } from '../services/order.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrl: './confirmation.component.css'
})
export class ConfirmationComponent implements OnInit {
  countdownSeconds = 10;
  transactionId = "";

  constructor(private orderService: OrderService, private router: Router) { }

  ngOnInit(): void {
    this.transactionId = this.orderService.getTransactionId();
    if (!this.transactionId)
      this.router.navigate(['/user/home']);

    this.displayCountdown();
  }

  displayCountdown() {
    const countdown = setInterval(() => {
      this.countdownSeconds--;

      if (this.countdownSeconds < 0) {
        clearInterval(countdown);
        this.router.navigate(['/user/home']);
      }
    }, 1000);
  }
}
