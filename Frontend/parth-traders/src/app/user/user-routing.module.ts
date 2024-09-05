import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticateComponent } from './authenticate/authenticate.component';
import { HomeComponent } from './home/home.component';
import { AuthGuardService as AuthGuard, AuthGuardService } from './services/auth-guard.service';
import { LogInGuardService as LogInGuard } from './services/log-in-guard.service';
import { CheckoutComponent } from './checkout/checkout.component';
import { PaymentComponent } from './payment/payment.component';
import { ConfirmationComponent } from './confirmation/confirmation.component';

const routes: Routes = [
  {
    path: 'authenticate',
    component: AuthenticateComponent,
    canActivate: [LogInGuard],
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'checkout',
    component: CheckoutComponent,
    // canActivate: [AuthGuardService],
  },
  {
    path: 'payment',
    component: PaymentComponent,
    // canActivate: [AuthGuardService],
  },
  {
    path: 'confirm',
    component: ConfirmationComponent,
    // canActivate: [AuthGuardService],
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'home',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserRoutingModule { }
