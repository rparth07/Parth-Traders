import { Component, Input, OnInit } from '@angular/core';
import { Product } from './product';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
})
export class ProductComponent implements OnInit {
  @Input() product!: Product;
  @Input() title!: string;
  @Input() price!: number;
  @Input() rating!: number;
  @Input() image!: string;
  constructor() {}

  ngOnInit(): void {}
}
