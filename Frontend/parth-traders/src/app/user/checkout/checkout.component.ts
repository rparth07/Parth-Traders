import { Component, OnInit } from '@angular/core';
import { discountedUnitPrice } from '../../shared/pricing';
import { OrderDetail } from '../core/models/OrderDetail';
import { OrderService } from '../services/order.service';
import { CartService } from '../services/cart.service';
import { ProductService } from '../services/product.service';
@Component({ selector: 'app-checkout', templateUrl: './checkout.component.html', styleUrls: ['./checkout.component.css'] })
export class CheckoutComponent implements OnInit {
    discountedUnitPrice = discountedUnitPrice;
    totalOrderPrice = 0;
    orderItems: OrderDetail[] = [];
    loading = false;
    loadError = '';
    cartMessage = '';
    lastRemoved?: {item:OrderDetail;index:number};
    constructor(private orderService: OrderService, private cart: CartService, private products: ProductService) { }
    ngOnInit() { this.sync(); if (this.orderItems.length)
        this.refreshPrices(); }
    sync() { this.orderService.initializeOrder(); this.orderItems = this.cart.getCartItems(); this.totalOrderPrice = this.cart.getTotalOrderPrice(); }
    refreshPrices() { this.loading = true; this.loadError = ''; this.products.getProducts().subscribe({ next: products => { this.cart.refreshProducts(products); this.sync(); this.loading = false; }, error: () => { this.loading = false; this.loadError = 'Current prices and stock could not be checked. Try again before continuing.'; } }); }
    changeQuantity(item: OrderDetail, increment: boolean) { if(!increment && item.quantity===1){this.remove(this.orderItems.indexOf(item));return;} increment ? this.cart.incrementProductCountOf(item) : this.cart.decrementProductCountFrom(item); this.sync(); }
    remove(index:number) {
        const item=this.cart.removeFromCartAt(index); if(!item)return;
        this.lastRemoved={item,index};this.cartMessage=(item.productName||item.productSku)+' removed from your cart.';this.sync();
    }
    undoRemoval() {
        if(!this.lastRemoved)return;
        const restored=this.cart.restoreCartItem(this.lastRemoved.item,this.lastRemoved.index);
        this.cartMessage=restored?'Item restored to your cart.':'This item is already in your cart.';
        this.lastRemoved=undefined;this.sync();
    }
    get signedIn() { return this.orderService.isCustomerSignedIn(); }
    get invalidStock() { return this.orderItems.some(i => i.quantity > (i.unitsInStock ?? 0)); }
    getImgPathFromSku(sku: string) { return `assets/products/${sku}/${sku}_1.jpg`; }
}
