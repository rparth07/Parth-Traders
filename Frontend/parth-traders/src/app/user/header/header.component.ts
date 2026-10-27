import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { NavigationEnd, Router } from '@angular/router';
import { ProductService } from '../services/product.service';
import { CartService } from '../services/cart.service';
import { AuthService } from '../services/auth.service';
@Component({selector:'app-header',templateUrl:'./header.component.html',styleUrls:['./header.component.css']})
export class HeaderComponent implements OnInit, OnDestroy {
  selectedCategory='All Categories';selectedCategoryCount=0;searchText='';totalOrderItems=0;customerName='';shouldDisplaySearchBox=true;
  categories=['All Categories'];private subscriptions=new Subscription();
  constructor(private productService:ProductService,private cartService:CartService,private authService:AuthService,private router:Router){this.shouldDisplaySearchBox=router.url.includes('/user/home');this.customerName=authService.getCustomerName();}
  ngOnInit(){
    this.subscriptions.add(this.productService.getCategories().subscribe({next:categories=>this.categories=['All Categories',...categories],error:()=>{}}));
    this.subscriptions.add(this.cartService.getAllProductCountEvent().subscribe(count=>this.totalOrderItems=count));
    this.subscriptions.add(this.router.events.subscribe(event=>{if(event instanceof NavigationEnd)this.shouldDisplaySearchBox=event.urlAfterRedirects.includes('/user/home');}));
    this.subscriptions.add(this.productService.productFilter$.subscribe(f=>{this.searchText=f.name;this.selectedCategoryCount=f.categories.length;this.selectedCategory=f.categories.length>1?'__multiple__':f.categories[0]||'All Categories';}));
  }
  ngOnDestroy(){this.subscriptions.unsubscribe();}
  selectCategory(category:string){this.selectedCategory=category;this.searchProduct('clicked',this.searchText);}
  searchProduct(key:string,value:string){
    if(key!=='clicked'&&key!=='Enter'&&!(key==='input'&&value===''))return;
    const current=this.productService.productFilter$.value;
    this.productService.productFilter$.next({...current,name:value,categories:this.selectedCategory==='__multiple__'?current.categories:this.selectedCategory==='All Categories'?[]:[this.selectedCategory]});
  }
  signOut(){this.authService.logout();}
  isAuthenticated(){return this.authService.isAuthenticated();}
}
