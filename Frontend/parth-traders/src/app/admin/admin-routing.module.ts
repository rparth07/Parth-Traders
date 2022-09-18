import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductComponent } from './product/product.component';
import { CategoryComponent } from './category/category.component';
import { SupplierComponent } from './supplier/supplier.component';

const routes: Routes = [
  {
    path: 'products',
    component: ProductComponent,
  },
  {
    path: 'customers',
    component: ProductComponent,
  },
  {
    path: 'categories',
    component: CategoryComponent,
  },
  {
    path: 'suppliers',
    component: SupplierComponent,
  },
  {
    path: '',
    redirectTo: 'products',
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
