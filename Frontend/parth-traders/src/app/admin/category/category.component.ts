import { apiErrorMessage } from '../../shared/api-error';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, fromEvent, map, finalize } from 'rxjs';
import { Category } from './category';
import { CategoryService } from '../category/category.service';
import { AddCategoryComponent } from './add-category/add-category.component';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css'],
})
export class CategoryComponent implements OnInit {
  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loading$ = this.loadingSubject.asObservable();

  dataSource = new MatTableDataSource<Category>();
  loadError = '';
  dataLength: number = 0;

  updatableCategory!: Category | null;

  columns = [
    {
      columnDef: 'position',
      header: 'No.',
      cell: (element: Category) => `${element.position}`,
    },
    {
      columnDef: 'categoryName',
      header: 'Name',
      cell: (element: Category) => `${element.categoryName}`,
    },
    {
      columnDef: 'categoryDescription',
      header: 'Description',
      cell: (element: Category) => `${element.categoryDescription}`,
    },
  ];

  displayedColumns = this.columns.map((c) => c.columnDef);

  constructor(
    private modalService: NgbModal,
    private toastr: ToastrService,
    private categoryService: CategoryService
  ) { }

  ngOnInit(): void {
    this.getCategories();

    this.categoryService.getUpdatedCategories.subscribe((response) => {
      this.getCategories();
    });
  }

  applyFilter(filter: { searchValue: string }) {
    this.dataSource.filter = filter.searchValue.trim().toLowerCase();
    this.dataSource.paginator?.firstPage();
  }

  addCsv(event: any) {
    event.preventDefault();
    const file = event.target.files[0];
    if (file) {
      const formData = new FormData();
      formData.append('file', file);
      this.categoryService.addCategoryCsvFile(formData).subscribe({
        next: (value) => {
          this.getCategories();
        },
        error: (err: { error: string }) => {
          this.showToaster(apiErrorMessage(err));
        },
      });
    }
    event.target.value = '';
  }

  getCategories(): void {
    this.loadingSubject.next(true);
    this.loadError = '';
    this.categoryService
      .fetchCategories()
      .pipe(
        map(
          (categories: Category[]) =>
            categories.map((category: Category, index: number) => {
              category.position = index + 1;
              return category;
            })
          //need to find a way to also show products in the table
        )
      )
      .pipe(finalize(() => this.loadingSubject.next(false)))
      .subscribe({next: (response: Category[]) => {
        this.dataLength = response.length;
        this.dataSource.data = response;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }, error: error => { this.loadError = apiErrorMessage(error, 'Unable to load categories. Try again.'); }});
  }

  deleteCategory(category: Category) {
    this.categoryService.deleteCategory(category.categoryName);
  }

  openUpdateCategoryModal(category?: Category) {
    const modalRef = this.modalService.open(AddCategoryComponent, {
      modalDialogClass: 'modal-lg modal-dialog-centered',
      backdrop: 'static',
      keyboard: false,
    });

    if (category) {
      this.updatableCategory = category;
    } else {
      this.updatableCategory = null;
    }

    modalRef.componentInstance.category = this.updatableCategory;
  }

  showToaster(errorMsg: string) {
    this.toastr.error(errorMsg, 'Error');
  }
}
