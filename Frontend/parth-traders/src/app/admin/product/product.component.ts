import { LiveAnnouncer } from '@angular/cdk/a11y';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MatSort, Sort } from '@angular/material/sort';
import { BehaviorSubject, delay, map, Observable, startWith, tap } from 'rxjs';

import { AddProductComponent } from '../add-product/add-product.component';
import { Product, ProductType } from './product';
import { ProductService } from './product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
})
export class ProductComponent implements OnInit {
  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loading$ = this.loadingSubject.asObservable();

  dataSource!: MatTableDataSource<Product>;
  productType = ProductType;
  dataLength: number = 100;

  updatableProduct!: Product | null;

  columns = [
    {
      columnDef: 'Position',
      header: 'No.',
      cell: (element: Product) => `${element.position}`,
    },
    {
      columnDef: 'productName',
      header: 'Name',
      cell: (element: Product) => `${element.productName}`,
    },
    {
      columnDef: 'productType',
      header: 'Type',
      cell: (element: Product) => `${this.productType[element.productType]}`,
    },
    {
      columnDef: 'productDescription',
      header: 'Description',
      cell: (element: Product) => `${element.productDescription}`,
    },
    {
      columnDef: 'supplierName',
      header: 'Supplier',
      cell: (element: Product) => `${element.supplierName}`,
    },
    {
      columnDef: 'categoryName',
      header: 'Category',
      cell: (element: Product) => `${element.categoryName}`,
    },
    {
      columnDef: 'singlePieceMRP',
      header: 'Single Piece MRP',
      cell: (element: Product) => `${element.singlePieceMRP}`,
    },
    {
      columnDef: 'piecesPerUnit',
      header: 'Pieces Per Unit',
      cell: (element: Product) => `${element.piecesPerUnit}`,
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
      header: 'Stocks(Units)',
      cell: (element: Product) => `${element.unitsInStock}`,
    },
  ];

  displayedColumns = this.columns.map((c) => c.columnDef);

  constructor(
    private modalService: NgbModal,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.productService.getUpdatedProducts.subscribe((response) => {
      this.getProducts();
    });
    this.getProducts();
  }

  applyFilter(filter: { searchValue: string }) {
    this.dataSource.filter = filter.searchValue.trim().toLowerCase();
  }

  addCsv(event: any) {
    event.preventDefault();
    const file = event.target.files[0];
    if (file) {
      const formData = new FormData();
      formData.append('file', file);
      this.productService.addProductCsvFile(formData);
    }
    event.target.value = '';
    this.getProducts();
  }

  getProducts(): void {
    this.loadingSubject.next(true);
    this.productService
      .fetchProducts()
      .pipe(
        map((products: Product[]) =>
          products.map((product: Product, index: number) => {
            product.position = index + 1;
            return product;
          })
        )
      )
      .subscribe((response: Product[]) => {
        this.dataLength = response.length;
        this.dataSource = new MatTableDataSource(response);
        this.dataSource.paginator = this.paginator;
        //sort is not working for No., Type and Stocks(Units) columns
        this.dataSource.sort = this.sort;
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
}
