import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Product, ProductRequest, ProductType } from '../product/product';
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

  productKeys(): Array<string> {
    var keys = Object.keys(this.productType);
    return keys.slice(keys.length / 2);
  }

  ngOnInit(): void {
    console.log(this.product);
  }

  addProduct(form: NgForm) {
    if (form.valid) {
      var product = <Product>form.value;
      console.log('product = ', product);
      this.productService.addProduct(product);
      this.closeModal();
    }
  }

  updateProduct(productForm: NgForm, oldProduct: ProductRequest) {
    if (productForm.valid) {
      this.productService.updateProduct(oldProduct, productForm.value);
    }
  }

  closeModal() {
    this.activeModal.close();
  }
}
