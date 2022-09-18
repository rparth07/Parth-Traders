import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, map } from 'rxjs';
import { Supplier } from './supplier';
import { SupplierService } from './supplier.service';

@Component({
  selector: 'app-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.css'],
})
export class SupplierComponent implements OnInit {
  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loading$ = this.loadingSubject.asObservable();

  dataSource!: MatTableDataSource<Supplier>;
  dataLength: number = 100;

  updatableSupplier!: Supplier | null;

  columns = [
    {
      columnDef: 'position',
      header: 'No.',
      cell: (element: Supplier) => `${element.position}`,
    },
    {
      columnDef: 'supplierName',
      header: 'Name',
      cell: (element: Supplier) => `${element.supplierName}`,
    },
    {
      columnDef: 'supplierAddress',
      header: 'Description',
      cell: (element: Supplier) => `${element.supplierAddress}`,
    },
    {
      columnDef: 'supplierEmail',
      header: 'Email',
      cell: (element: Supplier) => `${element.supplierEmail}`,
    },
    {
      columnDef: 'supplierPhoneNumber',
      header: 'Phone Number',
      cell: (element: Supplier) => `${element.supplierPhoneNumber}`,
    },
  ];

  displayedColumns = this.columns.map((c) => c.columnDef);

  constructor(
    private modalService: NgbModal,
    private toastr: ToastrService,
    private supplierService: SupplierService
  ) {}

  ngOnInit(): void {
    this.getSuppliers();

    this.supplierService.getUpdatedSuppliers.subscribe((response) => {
      this.getSuppliers();
    });
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
      this.supplierService.addSupplierCsvFile(formData).subscribe({
        next: (value) => {
          this.getSuppliers();
        },
        error: (err: { error: string }) => {
          this.showToaster(err.error);
        },
      });
    }
    event.target.value = '';
  }

  getSuppliers(): void {
    this.loadingSubject.next(true);
    this.supplierService
      .fetchSuppliers()
      .pipe(
        map((suppliers: Supplier[]) =>
          suppliers.map((supplier: Supplier, index: number) => {
            supplier.position = index + 1;
            return supplier;
          })
        )
        // need to find a way to display products in table
      )
      .subscribe((response: Supplier[]) => {
        this.dataLength = response.length;
        this.dataSource = new MatTableDataSource(response);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      });
  }

  deleteSupplier(supplier: Supplier) {
    this.supplierService.deleteSupplier(supplier.supplierName);
  }

  openUpdateSupplierModal(supplier?: Supplier) {
    console.log(supplier);
  }

  showToaster(errorMsg: string) {
    this.toastr.error(errorMsg, 'Error');
  }
}
