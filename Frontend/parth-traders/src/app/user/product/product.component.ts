import { Component, ElementRef, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../core/models/Product';
import { discountedUnitPrice } from '../../shared/pricing';
@Component({ selector: 'app-product', templateUrl: './product.component.html', styleUrls: ['./product.component.css'] })
export class ProductComponent {
    @Input()
    product!: Product;
    @Input()
    shouldSwitchToLargeGrid = false;
    @Output()
    addToCartEvent = new EventEmitter<{
        productCard: HTMLDivElement;
        product: Product;
    }>();
    @Output()
    addToCartLargeEvent = new EventEmitter<{
        productCard: HTMLDivElement;
        product: Product;
    }>();
    discountedUnitPrice = discountedUnitPrice;
    imageIndex = 0;
    imageFailed = false;
    constructor(private element: ElementRef) { }
    changeImage(step: number) { this.imageIndex = (this.imageIndex + step + this.product.image_paths.length) % this.product.image_paths.length; this.imageFailed = false; }
    addToCart() { this.addToCartEvent.emit({ productCard: this.element.nativeElement, product: this.product }); }
}
