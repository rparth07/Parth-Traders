import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { compare } from 'fast-json-patch';
import { DomainConstants } from '../shared/domain.constants';
import { Product } from './product';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(private http: HttpClient) {}

  addProductCsvFile(form: FormData) {
    this.http.post(DomainConstants.URL + 'Admin/Product', form).subscribe({
      next: (value) => console.log(value),
      error: (err) => console.log(err),
    });
  }

  fetchProducts() {
    return this.http
      .get<Product[]>(DomainConstants.URL + 'Admin/Product')
      .pipe();
  }

  addProduct(product: Product) {
    this.http
      .post<Product>(DomainConstants.URL + 'Admin/Product', product)
      .subscribe({
        next: (value) => console.log(value),
        error: (err) => console.log(err),
      });
  }

  updateProduct(oldProduct: Product, newProduct: Product) {
    const patch = compare(oldProduct, newProduct);
    console.log('patch=', patch);
    this.http
      .put<Product>(
        DomainConstants.URL + 'admin/products/' + oldProduct.productName,
        patch
      )
      .subscribe({
        next: (value) => console.log(value),
        error: (err) => console.log(err),
      });
  }

  deleteProduct(productName: string) {
    this.http
      .delete<Product>(DomainConstants.URL + 'admin/products/' + productName)
      .subscribe({
        next: (value) => console.log(value),
        error: (err) => console.log(err),
      });
  }
}
