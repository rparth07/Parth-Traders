import { Injectable } from '@angular/core';
import { Subject, catchError, map, of, throwError } from 'rxjs';
import { Product } from '../core/models/Product';
import { FilterCriteria } from '../core/models/FilterCriteria';
import { convertIntToProductType, ProductType } from '../core/enums/ProductType';
import { HttpClient } from '@angular/common/http';
import { AppSetting } from 'src/app/shared/app-settings';

@Injectable({
    providedIn: 'root'
})
export class ProductService {
    productFilter$ = new Subject<FilterCriteria>();

    constructor(private http: HttpClient) { }

    getProducts() {
        return this.http.get<Product[]>(AppSetting.API_URL + 'API/admin/products')
            .pipe(
                map(response => {
                    response.forEach(_ => {
                        _.image_paths = this.generateImgPaths(_.productSku);
                        _.productType = convertIntToProductType(_.productType as number)!;
                    });
                    return response;
                }),
                catchError(error => {
                    return throwError(error);
                })
            );
    }

    getCategories() {
        return this.http.get<{ categoryName: string }[]>(AppSetting.API_URL + 'API/admin/categories')
            .pipe(
                map(response => {
                    return response.map(category => category.categoryName);
                }),
                catchError(error => {
                    return throwError(error);
                })
            );
    }

    private generateImgPaths(sku: string): string[] {
        return [`assets/products/${sku}/${sku}_1.jpg`,
        `assets/products/${sku}/${sku}_2.jpg`,
        `assets/products/${sku}/${sku}_3.jpg`
        ]
    }
}
