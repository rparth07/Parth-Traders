import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { Product, ProductType } from '../product';
import { ProductService } from '../product.service';

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
    private activeModal: NgbActiveModal,
    private toastr: ToastrService
  ) { }

  productKeys(): Array<string> {
    var keys = Object.keys(this.productType);
    return keys.slice(keys.length / 2);
  }

  ngOnInit(): void {
  }

  addProduct(form: NgForm) {
    if (form.valid) {
      var product = <Product>form.value;
      this.productService.addProduct(product).subscribe({
        next: (value: any) => {
          this.closeModal();
          this.productService.getUpdatedProducts.next('Updated products');
        },
        error: (err: { error: { errors: any } }) => {
          this.showToaster(err.error.errors.productDto);
        },
      });
    }
  }

  updateProduct(productForm: NgForm, oldProduct: Product) {
    if (productForm.valid) {
      this.productService
        .updateProduct(oldProduct?.productName, productForm.value)
        .subscribe({
          next: (value) => {
            this.closeModal();
            this.productService.getUpdatedProducts.next('Updated products');
          },
          error: (err: { error: { errors: any } }) => {
            this.showToaster(err.error.errors.productDto);
          },
        });
    }
  }

  closeModal() {
    this.activeModal.close();
  }

  showToaster(errorMsg: string) {
    this.toastr.error(errorMsg, 'Error');
  }
}
