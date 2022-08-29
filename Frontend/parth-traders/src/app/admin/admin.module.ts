import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { EnumConverterPipe } from './shared/enum-converter.pipe';
import { ProductComponent } from './product/product.component';
import { HeaderComponent } from './header/header.component';
import { AdminComponent } from './admin.component';
import { AdminRoutingModule } from './admin-routing.module';

@NgModule({
  declarations: [
    AdminComponent,
    HeaderComponent,
    ProductComponent,
    EnumConverterPipe,
  ],
  imports: [CommonModule, NgbModule, AdminRoutingModule],
  exports: [EnumConverterPipe, AdminComponent],
  bootstrap: [AdminComponent],
})
export class AdminModule {}
