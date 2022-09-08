import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
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
  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loading$ = this.loadingSubject.asObservable();

  dataSource!: MatTableDataSource<Product>;
  ProductType = ProductType;
  dataLength: number = 100;

  updatableProduct!: Product | null;

  columns = [
    {
      columnDef: 'No.',
      header: 'Position',
      cell: (element: Product) => `${element.position}`,
    },
    {
      columnDef: 'productName',
      header: 'Product Name',
      cell: (element: Product) => `${element.productName}`,
    },
    {
      columnDef: 'productType',
      header: 'Product Type',
      cell: (element: Product) => `${ProductType[element.productType]}`,
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
        console.log(this.dataSource);
        this.dataSource.paginator = this.paginator;
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
