import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { compare } from 'fast-json-patch';
import { DomainConstants } from '../shared/domain.constants';
import { Product, ProductRequest } from './product';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(private http: HttpClient) {}

  addProductCsvFile(form: FormData) {
    this.http.post(DomainConstants.URL + 'admin/products', form).subscribe({
      next: (value) => console.log(value),
      error: (err) => console.log(err),
    });
  }

  fetchProducts() {
    var products = this.http.get<Product[]>(
      DomainConstants.URL + 'admin/products'
    );

    products.subscribe({
      error: (err) => console.log('err = ', err),
    });

    return products;
  }

  addProduct(product: ProductRequest) {
    this.http
      .post<ProductRequest>(DomainConstants.URL + 'admin/products', product)
      .subscribe({
        next: (value) => console.log(value),
        error: (err) => console.log(err),
      });
  }

  updateProduct(oldProduct: ProductRequest, newProduct: ProductRequest) {
    const patch = compare(oldProduct, newProduct);
    console.log('patch=', patch);
    this.http
      .put<ProductRequest>(
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
