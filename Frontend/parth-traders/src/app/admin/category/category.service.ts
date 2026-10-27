import { ToastrService } from 'ngx-toastr';
import { apiErrorMessage } from '../../shared/api-error';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Category } from './category';
import { DomainConstants } from '../shared/domain.constants';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  getUpdatedCategories = new Subject<string>();

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  addCategoryCsvFile(form: FormData) {
    return this.http.post(DomainConstants.URL + 'admin/categories', form);
  }

  fetchCategories() {
    return this.http.get<Category[]>(DomainConstants.URL + 'admin/categories');
  }

  addCategory(category: Category): any {
    return this.http.post(DomainConstants.URL + 'admin/categories/add-category', category);
  }

  updateCategory(oldCategoryName: string, newCategory: Category) {
    return this.http.post<Category>(
      DomainConstants.URL + 'admin/categories/' + encodeURIComponent(oldCategoryName),
      newCategory
    );
  }

  deleteCategory(categoryName: string) {
    this.http
      .delete<Category>(
        DomainConstants.URL + 'admin/categories/' + categoryName
      )
      .subscribe({
        next: (value) => {
          this.getUpdatedCategories.next('Updated categories');
        },
        error: (err) => this.toastr.error(apiErrorMessage(err), 'Delete failed'),
      });
  }
}
