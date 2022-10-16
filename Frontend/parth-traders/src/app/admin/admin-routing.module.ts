import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductComponent } from './product/product.component';
import { CategoryComponent } from './category/category.component';
import { SupplierComponent } from './supplier/supplier.component';
import { CustomerComponent } from './customer/customer.component';
import { LogInComponent } from './log-in/log-in.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuardService as AuthGuard } from './auth-guard.service';
import { LogInGuardService as LogInGuard } from './log-in-guard.service';

const routes: Routes = [
  {
    path: 'login',
    component: LogInComponent,
    canActivate: [LogInGuard],
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'products',
    component: ProductComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'customers',
    component: CustomerComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'categories',
    component: CategoryComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'suppliers',
    component: SupplierComponent,
    canActivate: [AuthGuard],
  },
  {
    path: '',
    redirectTo: 'products',
    pathMatch: 'full',
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
