import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, map } from 'rxjs';
import { Customer, PaymentType } from './customer';
import { CustomerService } from './customer.service';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css'],
})
export class CustomerComponent implements OnInit {
  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loading$ = this.loadingSubject.asObservable();

  dataSource!: MatTableDataSource<Customer>;
  paymentType = PaymentType;
  dataLength: number = 100;

  updatableCustomer!: Customer | null;

  columns = [
    {
      columnDef: 'position',
      header: 'No.',
      cell: (element: Customer) => `${element.position}`,
    },
    {
      columnDef: 'customerName',
      header: 'Name',
      cell: (element: Customer) => `${element.customerName}`,
    },
    {
      columnDef: 'createdDate',
      header: 'Created Date',
      cell: (element: Customer) => `${element.createdDate}`,
    },
    {
      columnDef: 'customerEmail',
      header: 'Email',
      cell: (element: Customer) => `${element.customerEmail}`,
    },
    {
      columnDef: 'customerPhoneNumber',
      header: 'Phone Number',
      cell: (element: Customer) => `${element.customerPhoneNumber}`,
    },
    {
      columnDef: 'customerAddress',
      header: 'Address',
      cell: (element: Customer) => `${element.customerAddress}`,
    },
    {
      columnDef: 'paymentType',
      header: 'Type',
      cell: (element: Customer) => `${this.paymentType[element.paymentType]}`,
    },
  ];

  displayedColumns = this.columns.map((c) => c.columnDef);

  constructor(
    private modalService: NgbModal,
    private toastr: ToastrService,
    private customerService: CustomerService
  ) { }

  ngOnInit(): void {
    this.getCustomers();

    this.customerService.getUpdatedCustomers.subscribe((response) => {
      this.getCustomers();
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
      this.customerService.addCustomerCsvFile(formData).subscribe({
        next: (value) => {
          this.getCustomers();
        },
        error: (err: { error: string }) => {
          this.showToaster(err.error);
        },
      });
    }
    event.target.value = '';
  }

  getCustomers(): void {
    this.loadingSubject.next(true);
    this.customerService
      .fetchCustomers()
      .pipe(
        map(
          (customers: Customer[]) =>
            customers.map((customer: Customer, index: number) => {
              customer.position = index + 1;
              return customer;
            })
          //need to find a way to show orders in the table
        )
      )
      .subscribe((response: Customer[]) => {
        this.dataLength = response.length;
        this.dataSource = new MatTableDataSource(response);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      });
  }

  deleteCustomer(customer: Customer) {
    this.customerService.deleteCustomer(customer.customerName);
  }

  openUpdateCustomerModal(customer?: Customer) {
    console.log(customer);
  }

  showToaster(errorMsg: string) {
    this.toastr.error(errorMsg, 'Error');
  }
}
