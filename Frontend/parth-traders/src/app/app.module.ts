import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddProductComponent } from './admin/add-product/add-product.component';
import { ProductComponent } from './admin/product/product.component';
import { AdminComponent } from './admin/admin.component';
import { UserComponent } from './user/user.component';

@NgModule({
  declarations: [
    AppComponent,
    AddProductComponent,
    ProductComponent,
    AdminComponent,
    UserComponent,
  ],
  imports: [BrowserModule, AppRoutingModule, HttpClientModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
