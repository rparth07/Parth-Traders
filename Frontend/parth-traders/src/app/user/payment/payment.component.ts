import { Component, ElementRef, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import { OrderService } from '../services/order.service';
import { AppSetting } from '../../shared/app-settings';

@Component({ selector: 'app-payment', templateUrl: './payment.component.html', styleUrl: './payment.component.css' })
export class PaymentComponent {
  amount = 0;
  paymentError = '';
  processing = false;
  canRestart = false;
  pendingId = sessionStorage.getItem('pending_sandbox_order') || '';
  @ViewChild('paymentRef', { static: true }) paymentRef!: ElementRef;
  constructor(private router: Router, private orderService: OrderService, private cartService: CartService, private http: HttpClient) {}
  async ngOnInit(): Promise<void> {
    this.amount = this.orderService.getTotalOrderPrice();
    if (this.pendingId) {
      try {
        const status = await firstValueFrom(this.http.get<any>(AppSetting.API_URL + 'api/payments/orders/' + encodeURIComponent(this.pendingId)));
        this.amount = status.total; this.canRestart = status.remoteStatus === 'CREATED';
        if (status.transactionId) { await this.complete(status.transactionId); return; }
        this.paymentError = 'A sandbox checkout is pending. Check its status before starting another payment.';
      } catch { this.paymentError = 'The previous sandbox checkout could not be checked. Please contact Parth Traders before retrying payment.'; }
      return;
    }
    if (this.amount <= 0) { await this.router.navigate(['/user/home']); return; }
    try {
      const config = await firstValueFrom(this.http.get<{ enabled: boolean; clientId: string; mode: string }>(AppSetting.API_URL + 'api/payments/config'));
      if (!config.enabled || config.mode !== 'sandbox') { this.paymentError = 'Sandbox payments are not configured yet. Contact Parth Traders for help.'; return; }
      if (!window.paypal) await new Promise<void>((resolve, reject) => {
        const script = document.createElement('script');
        script.src = 'https://www.paypal.com/sdk/js?currency=USD&client-id=' + encodeURIComponent(config.clientId);
        const timer = setTimeout(() => reject(new Error('Payment service timeout')), 25000);
        script.onload = () => { clearTimeout(timer); resolve(); };
        script.onerror = () => { clearTimeout(timer); reject(new Error('Payment service unavailable')); };
        document.head.appendChild(script);
      });
      await window.paypal.Buttons({
        style: { layout: 'horizontal', color: 'blue', shape: 'rect', label: 'paypal' },
        createOrder: async () => {
          const cart = this.cartService.getCartItems().map(item => ({sku: item.productSku, quantity: item.quantity}));
          const order = await firstValueFrom(this.http.post<{id: string; total: number}>(AppSetting.API_URL + 'api/payments/orders', cart));
          this.amount = order.total; this.pendingId = order.id; sessionStorage.setItem('pending_sandbox_order', order.id); return order.id;
        },
        onApprove: async () => this.checkPayment(),
        onCancel: () => { sessionStorage.removeItem('pending_sandbox_order'); this.pendingId = ''; this.paymentError = 'Sandbox payment was cancelled. No payment has been recorded.'; },
        onError: () => { this.paymentError = 'The sandbox payment could not be completed. Check its status before retrying.'; }
      }).render(this.paymentRef.nativeElement);
    } catch { this.paymentError = 'Payment service is unavailable. Return to checkout and try again later.'; }
  }
  async checkPayment() {
    if (!this.pendingId || this.processing) return;
    this.processing = true; this.paymentError = '';
    try {
      const result = await firstValueFrom(this.http.post<{transactionId: string}>(AppSetting.API_URL + 'api/payments/orders/' + encodeURIComponent(this.pendingId) + '/capture', {}));
      await this.complete(result.transactionId);
    } catch { this.paymentError = 'Sandbox payment is not confirmed. Check again or contact Parth Traders before starting another payment.'; }
    finally { this.processing = false; }
  }
  private async complete(transactionId: string) {
    this.orderService.setTransactionId(transactionId); sessionStorage.removeItem('pending_sandbox_order');
    this.cartService.emitClearCartEvent(); await this.router.navigate(['/user/confirm']);
  }
  async startNewCheckout() {
    if (!this.canRestart || this.processing) return;
    this.processing = true;
    try {
      const status = await firstValueFrom(this.http.get<any>(AppSetting.API_URL + 'api/payments/orders/' + encodeURIComponent(this.pendingId)));
      if (status.remoteStatus !== 'CREATED' || status.transactionId) { this.paymentError = 'Check the existing payment status before starting again.'; this.canRestart = false; return; }
      sessionStorage.removeItem('pending_sandbox_order'); this.pendingId = ''; this.paymentError = '';
      await this.ngOnInit();
    } catch { this.paymentError = 'Could not check the previous payment. Please try again later.'; }
    finally { this.processing = false; }
  }
  cancel() { void this.router.navigate(['/user/checkout']); }
}
