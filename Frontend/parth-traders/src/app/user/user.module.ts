import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';
import { MatIconModule } from '@angular/material/icon';
import { HomeComponent } from './home/home.component';
import { FooterComponent } from './footer/footer.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { ProductComponent } from './product/product.component';
import { NgbCarouselModule } from '@ng-bootstrap/ng-bootstrap';
import { SidebarComponent } from './sidebar/sidebar.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
  declarations: [
    UserComponent,
    HeaderComponent,
    HomeComponent,
    FooterComponent,
    ShoppingCartComponent,
    ProductComponent,
    SidebarComponent,
  ],
  imports: [CommonModule, MatIconModule, NgbCarouselModule, UserRoutingModule, InfiniteScrollModule, MatProgressSpinnerModule],
})
export class UserModule { }
