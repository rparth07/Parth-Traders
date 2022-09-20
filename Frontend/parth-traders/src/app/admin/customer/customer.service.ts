import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { DomainConstants } from '../shared/domain.constants';
import { Customer } from './customer';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  getUpdatedCustomers = new Subject<string>();

  constructor(private http: HttpClient) {}

  addCustomerCsvFile(form: FormData) {
    return this.http.post(DomainConstants.URL + 'admin/customers', form);
  }

  fetchCustomers() {
    return this.http.get<Customer[]>(DomainConstants.URL + 'admin/customers');
  }

  addCustomer(customer: Customer): any {
    return this.http.post(DomainConstants.URL + 'admin/customers', customer);
  }

  updateCustomer(oldCustomerName: string, newCustomer: Customer) {
    return this.http.post<Customer>(
      DomainConstants.URL + 'admin/customers/' + oldCustomerName,
      newCustomer
    );
  }

  deleteCustomer(customerName: string) {
    this.http
      .delete<Customer>(DomainConstants.URL + 'admin/customers/' + customerName)
      .subscribe({
        next: (value) => {
          this.getUpdatedCustomers.next('Updated customers');
        },
        error: (err) => console.log(err),
      });
  }
}
