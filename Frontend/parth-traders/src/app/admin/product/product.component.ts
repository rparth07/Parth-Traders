import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable, tap } from 'rxjs';
import { AddProductComponent } from '../add-product/add-product.component';
import { Product } from './product';
import { ProductService } from './product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
})
export class ProductComponent implements OnInit {
  products$!: Observable<Product[]>;
  currentPageData$!: Observable<Product[]>;

  pageSize = 5;
  selectedPageNumber = 0;

  totalPages: number[] = [];
  updatableProduct!: Product | null;

  constructor(
    private modalService: NgbModal,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.getProducts();
  }

  addCsv(event: any) {
    const file = event.target.files[0];
    if (file) {
      const formData = new FormData();
      formData.append('file', file);
      this.productService.addProductCsvFile(formData);
    }
    event.target.value = '';
  }

  getProducts(): void {
    this.products$ = this.productService.fetchProducts().pipe(
      tap((product) => {
        this.setTotalPage(product);
      })
    );
    this.onPageChange(this.selectedPageNumber);
  }

  deleteProduct(productName: string) {
    this.productService.deleteProduct(productName);
  }

  openUpdateProductModal(product?: Product) {
    const modalRef = this.modalService.open(AddProductComponent, {
      modalDialogClass: 'modal-lg modal-dialog-centered',
      backdrop: 'static',
      keyboard: false,
    });

    if (product) {
      this.updatableProduct = product;
    } else {
      this.updatableProduct = null;
    }

    modalRef.componentInstance.product = this.updatableProduct;
  }

  onPageChange(pageNumber: number) {
    this.selectedPageNumber = pageNumber;

    //paginate using npm library for pagination
  }

  setTotalPage(products: Product[]) {
    this.totalPages = Array(Math.ceil(products.length / this.pageSize))
      .fill(0)
      .map((x, i) => i);
  }
}
