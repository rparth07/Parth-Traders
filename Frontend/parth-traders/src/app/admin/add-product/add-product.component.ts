import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
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
  errors: string[] = [];
  types = ['success', 'error', 'info', 'warning'];

  constructor(
    private productService: ProductService,
    private activeModal: NgbActiveModal,
    private toastr: ToastrService
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
      this.showToaster('Product added successfully');
      async () => {
        this.errors = await this.productService.addProduct(product);
        console.log('this.errors = ', this.errors);
        if (this.errors.length == 0) {
          this.closeModal();
        } else {
          this.errors.forEach((error) => {
            this.showToaster(error);
          });
        }
      };
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

  showToaster(errMsg: string) {
    this.toastr.error(errMsg);
  }
}
