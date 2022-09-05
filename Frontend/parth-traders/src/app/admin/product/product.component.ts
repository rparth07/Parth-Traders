import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable, tap } from 'rxjs';
import { AddProductComponent } from '../add-product/add-product.component';
import { Product, ProductType } from './product';
import { ProductService } from './product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
})
export class ProductComponent implements OnInit {
  ProductType = ProductType;
  products!: Product[];
  currentPageData$!: Observable<Product[]>;

  pageSize = 5;
  selectedPageNumber = 0;

  totalPages: number[] = [];
  updatableProduct!: Product | null;

  columns = [
    {
      columnDef: 'productName',
      header: 'Product Name',
      cell: (element: Product) => `${element.productName}`,
    },
    {
      columnDef: 'productType',
      header: 'Product Type',
      cell: (element: Product) => `${element.productType}`,
    },
    {
      columnDef: 'productDescription',
      header: 'Product Description',
      cell: (element: Product) => `${element.productDescription}`,
    },
    {
      columnDef: 'supplierName',
      header: 'Supplier Name',
      cell: (element: Product) => `${element.supplierName}`,
    },
    {
      columnDef: 'categoryName',
      header: 'Category Name',
      cell: (element: Product) => `${element.categoryName}`,
    },
    {
      columnDef: 'piecesPerUnit',
      header: 'Pieces Per Unit',
      cell: (element: Product) => `${element.piecesPerUnit}`,
    },
    {
      columnDef: 'singlePieceMRP',
      header: 'Single Piece MRP',
      cell: (element: Product) => `${element.singlePieceMRP}`,
    },
    {
      columnDef: 'unitPrice',
      header: 'Unit Price',
      cell: (element: Product) => `${element.unitPrice}`,
    },
    {
      columnDef: 'discount',
      header: 'Discount',
      cell: (element: Product) => `${element.discount}`,
    },
    {
      columnDef: 'Units Left In Stocks',
      header: 'Units In Stock',
      cell: (element: Product) => `${element.unitsInStock}`,
    },
  ];

  displayedColumns = this.columns.map((c) => c.columnDef);

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
    this.productService.fetchProducts().subscribe((response: Product[]) => {
      this.products = response;
    });
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
