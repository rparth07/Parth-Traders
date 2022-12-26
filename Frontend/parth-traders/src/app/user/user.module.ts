import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';
import { MatIconModule } from '@angular/material/icon';
import { HomeComponent } from './home/home.component';
import { ProductComponent } from './product/product.component';
import { ProductListComponent } from './product-list/product-list.component';

@NgModule({
  declarations: [UserComponent, HeaderComponent, HomeComponent, ProductComponent, ProductListComponent],
  imports: [CommonModule, MatIconModule, UserRoutingModule],
})
export class UserModule {}
