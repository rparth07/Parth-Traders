import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddProductComponent } from './admin/add-product/add-product.component';
import { ProductComponent } from './admin/product/product.component';
import { AdminComponent } from './admin/admin.component';
import { UserComponent } from './user/user.component';
import { HeaderComponent } from './admin/header/header.component';
import { NgxUiLoaderModule, NgxUiLoaderHttpModule } from 'ngx-ui-loader';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgxUiLoaderModule,
    NgxUiLoaderHttpModule.forRoot({ showForeground: true }),
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
