import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { Product } from '../core/models/Product';
import { Subject } from 'rxjs';
import { animate, state, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  animations: [
    trigger('fadeInOut', [
      state('void', style({ opacity: 0 })), // Initial state
      transition(':enter, :leave', [animate(300)]), // Animation duration (300ms)
    ]),
  ],
})
export class HomeComponent implements OnInit {

  products: Product[] = [];
  isLargeGrid = false;
  changeGridToLargeSubject$: Subject<boolean> = new Subject<boolean>();

  constructor(productService: ProductService) {
    this.products = productService.getProducts();
  }

  ngOnInit(): void {
  }

  switchToLargeGrid() {
    this.isLargeGrid = true;
    this.changeGridToLargeSubject$.next(true);
  }

  switchToSmallGrid() {
    this.isLargeGrid = false;
    this.changeGridToLargeSubject$.next(false);
  }

}
