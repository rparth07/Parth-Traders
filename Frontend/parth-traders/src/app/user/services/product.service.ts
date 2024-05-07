import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { Product } from '../core/models/Product';

@Injectable({
    providedIn: 'root'
})
export class ProductService {
    products: Product[] = [
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            type: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            image_paths: ["https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "49538094",
            title: "FLUTED HEM DRESS",
            type: 'Glass Bowl',
            sku: '5L-Glass-Bowl',
            price: 239.0,
            rating: 4,
            image_paths: ["https://images-na.ssl-images-amazon.com/images/I/81O%2BGNdkzKL._AC_SX450_.jpg"]
        },
        {
            id: "4903850",
            title: "FLUTED HEM DRESS",
            type: 'Monitor',
            sku: 'TV-49',
            price: 199.99,
            rating: 3,
            image_paths: ["https://images-na.ssl-images-amazon.com/images/I/71Swqqe7XAL._AC_SX466_.jpg"]
        },
        {
            id: "23445930",
            title: "FLUTED HEM DRESS",
            type: 'Alexa',
            sku: 'Alexa-3rd',
            price: 98.99,
            rating: 5,
            image_paths: ["https://media.very.co.uk/i/very/P6LTG_SQ1_0000000071_CHARCOAL_SLf?$300x400_retinamobilex2$"]
        },
        {
            id: "3254354345",
            title: "FLUTED HEM DRESS",
            type: 'iPad',
            sku: 'Apple-iPad-Pro',
            price: 598.99,
            rating: 4,
            image_paths: ["https://images-na.ssl-images-amazon.com/images/I/816ctt5WV5L._AC_SX385_.jpg"]
        },
        {
            id: "90829332",
            title: "FLUTED HEM DRESS",
            type: 'Monitor',
            sku: 'Monitor-49',
            price: 1094.98,
            rating: 4,
            image_paths: ["https://images-na.ssl-images-amazon.com/images/I/6125mFrzr6L._AC_SX355_.jpg"]
        }
    ];

    cartItems: any[] = [];

    constructor() { }

    getProducts() {
        return this.products;
    }

    addToCart(product: any) {
        this.cartItems.push(product);
    }

    getCartItems() {
        return this.cartItems;
    }

    removeFromCart(index: number) {
        this.cartItems.splice(index, 1);
    }
}
