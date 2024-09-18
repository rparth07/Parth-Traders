import { Component, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import { style } from '@angular/animations';
import { OrderService } from '../services/order.service';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent {
  amount = 0;

  @ViewChild('paymentRef', { static: true }) paymentRef!: ElementRef;

  constructor(private router: Router, private orderService: OrderService, private cartService: CartService) {
  }

  ngOnInit(): void {
    this.amount = this.orderService.getTotalOrderPrice();
    // console.log(this.amount);
    if (this.amount == 0) {
      this.router.navigate(['/user/home']);
    }
    window.paypal.Buttons({
      style: {
        layout: 'horizontal',
        color: 'blue',
        shape: 'rect',
        label: 'paypal',
      },

      createOrder: (data: any, actions: any) => {
        return actions.order.create({
          purchase_units: [
            {
              amount: {
                value: this.amount.toString(),
                currency_code: 'USD'
              }
            }
          ]
        })
      },
      onApprove: (data: any, actions: any) => {
        return actions.order.capture().then((details: any) => {
          if (details.status == 'COMPLETED') {
            this.orderService.createOrder(details.id).subscribe({
              next: (value: any) => {
                this.cartService.emitClearCartEvent();
                this.router.navigate(['/user/confirm']);
              },
              error: (err: { error: { errors: any } }) => {
                alert("something went wrong, please try again");
              }
            });
          }
        })
      },
      onError: (error: any) => {
        console.log(error);
      }
    }).render(this.paymentRef.nativeElement);
  }

  cancel() {
    this.router.navigate(['/user/cart']);
  }
}
