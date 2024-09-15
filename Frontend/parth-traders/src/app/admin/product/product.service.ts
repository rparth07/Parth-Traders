import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { DomainConstants } from '../shared/domain.constants';
import { Product } from './product';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  getUpdatedProducts = new Subject<string>();

  constructor(private http: HttpClient) { }

  addProductCsvFile(form: FormData) {
    return this.http.post(DomainConstants.URL + 'admin/products', form);
  }

  fetchProducts() {
    return this.http.get<Product[]>(DomainConstants.URL + 'admin/products');
  }

  addProduct(product: Product): any {
    return this.http.post(DomainConstants.URL + 'admin/products/add-product', product);
  }

  updateProduct(oldProductName: string, newProduct: Product) {
    return this.http.post<Product>(
      DomainConstants.URL + 'admin/products/' + oldProductName,
      newProduct
    );
  }

  deleteProduct(productName: string) {
    this.http
      .delete<Product>(DomainConstants.URL + 'admin/products/' + productName)
      .subscribe({
        next: (value) => {
          this.getUpdatedProducts.next('Updated products');
        },
        error: (err) => console.log(err),
      });
  }
}
