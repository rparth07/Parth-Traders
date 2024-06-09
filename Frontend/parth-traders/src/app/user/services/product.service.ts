import { Injectable } from '@angular/core';
import { Subject, of } from 'rxjs';
import { Product } from '../core/models/Product';

@Injectable({
    providedIn: 'root'
})
export class ProductService {
    productFilter$ = new Subject<string[]>();
    products: Product[] = [
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "12321341",
            title: "FLUTED HEM DRESS",
            category: 'Paperback',
            sku: 'L-Paperback',
            price: 11.96,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/51Zymoq7UnL._SX325_BO1,204,203,200_.jpg"]
        },
        {
            id: "49538094",
            title: "FLUTED HEM DRESS",
            category: 'Glass Bowl',
            sku: '5L-Glass-Bowl',
            price: 239.0,
            rating: 4,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/81O%2BGNdkzKL._AC_SX450_.jpg"]
        },
        {
            id: "4903850",
            title: "FLUTED HEM DRESS",
            category: 'Monitor',
            sku: 'TV-49',
            price: 199.99,
            rating: 3,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/71Swqqe7XAL._AC_SX466_.jpg"]
        },
        {
            id: "23445930",
            title: "FLUTED HEM DRESS",
            category: 'Alexa',
            sku: 'Alexa-3rd',
            price: 98.99,
            rating: 5,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://media.very.co.uk/i/very/P6LTG_SQ1_0000000071_CHARCOAL_SLf?$300x400_retinamobilex2$"]
        },
        {
            id: "3254354345",
            title: "FLUTED HEM DRESS",
            category: 'iPad',
            sku: 'Apple-iPad-Pro',
            price: 598.99,
            rating: 4,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/816ctt5WV5L._AC_SX385_.jpg"]
        },
        {
            id: "90829332",
            title: "FLUTED HEM DRESS",
            category: 'Monitor',
            sku: 'Monitor-49',
            price: 1094.98,
            rating: 4,
            colors: ['222', '6e8cd5', 'f56060', '44c28d'],
            sizes: ['XS', 'S', 'M', 'L', 'XL', 'XXL'],
            image_paths: ["https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/1.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/2.jpg",
                "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/3.jpg",
                "https://images-na.ssl-images-amazon.com/images/I/6125mFrzr6L._AC_SX355_.jpg"]
        }
    ];

    constructor() { }

    getProducts() {
        return this.products;
    }

    getCategories() {
        return this.products.filter((product, index, self) => {
            return self.findIndex(p => p.category === product.category) === index;
        }).map(_ => _.category);
    }
}
