import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { DomainConstants } from '../shared/domain.constants';
import { Supplier } from './supplier';

@Injectable({
  providedIn: 'root',
})
export class SupplierService {
  getUpdatedSuppliers = new Subject<string>();

  constructor(private http: HttpClient) { }

  addSupplierCsvFile(form: FormData) {
    return this.http.post(DomainConstants.URL + 'admin/suppliers', form);
  }

  fetchSuppliers() {
    return this.http.get<Supplier[]>(DomainConstants.URL + 'admin/suppliers');
  }

  addSupplier(supplier: Supplier): any {
    return this.http.post(DomainConstants.URL + 'admin/suppliers/add-supplier', supplier);
  }

  updateSupplier(oldSupplierName: string, newSupplier: Supplier) {
    return this.http.post<Supplier>(
      DomainConstants.URL + 'admin/suppliers/' + oldSupplierName,
      newSupplier
    );
  }

  deleteSupplier(supplierName: string) {
    this.http
      .delete<Supplier>(DomainConstants.URL + 'admin/suppliers/' + supplierName)
      .subscribe({
        next: (value) => {
          this.getUpdatedSuppliers.next('Updated suppliers');
        },
        error: (err) => console.log(err),
      });
  }
}
