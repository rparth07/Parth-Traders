import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Product, ProductType } from '../product/product';
import { ProductService } from '../product/product.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css'],
})
export class AddProductComponent implements OnInit {
  @Input() product!: Product | null;
  productType = ProductType;

  constructor(
    private productService: ProductService,
    private activeModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    console.log(this.product);
  }

  addProduct(form: NgForm) {
    if (form.valid) {
      this.productService.addProduct(form.value);
    }
  }

  updateProduct(productForm: NgForm, oldProduct: Product) {
    if (productForm.valid) {
      this.productService.updateProduct(oldProduct, productForm.value);
    }
  }

  closeModal() {
    this.activeModal.close();
  }
}
