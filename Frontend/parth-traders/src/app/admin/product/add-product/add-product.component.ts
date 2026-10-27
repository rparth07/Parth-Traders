import { apiErrorMessage } from '../../../shared/api-error';
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
  saving=false;
  saveError='';
  @Input() product!: Product | null;
  productType = ProductType;

  constructor(
    private productService: ProductService,
    private activeModal: NgbActiveModal,
    private toastr: ToastrService
  ) { }

  productKeys(): Array<keyof typeof ProductType> {
    var keys = Object.keys(this.productType);
    return keys.slice(keys.length / 2) as Array<keyof typeof ProductType>;
  }

  ngOnInit(): void {
  }

  addProduct(form: NgForm) {
    if (this.saving) return;
    if (form.valid) {
      this.saving=true; this.saveError='';
      var product = <Product>form.value;
      this.productService.addProduct(product).subscribe({
        next: (value: any) => {
          this.saving=false; this.closeModal();
          this.productService.getUpdatedProducts.next('Updated products');
        },
        error: (err: { error: { errors: any } }) => {
          this.showToaster(apiErrorMessage(err));
        },
      });
    }
  }

  updateProduct(productForm: NgForm, oldProduct: Product) {
    if (this.saving) return;
    if (productForm.valid) {
      this.saving=true; this.saveError='';
      this.productService
        .updateProduct(oldProduct?.productName, {...oldProduct, ...productForm.value})
        .subscribe({
          next: (value) => {
            this.saving=false; this.closeModal();
            this.productService.getUpdatedProducts.next('Updated products');
          },
          error: (err: { error: { errors: any } }) => {
            this.showToaster(apiErrorMessage(err));
          },
        });
    }
  }

  closeModal() {
    this.activeModal.close();
  }

  showToaster(errorMsg: string) {
    this.saving=false; this.saveError=errorMsg; this.toastr.error(errorMsg, 'Save failed');
  }
}
