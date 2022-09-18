import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { Category } from '../category';
import { CategoryService } from '../category.service';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css'],
})
export class AddCategoryComponent implements OnInit {
  @Input() category!: Category | null;

  constructor(
    private categoryService: CategoryService,
    private activeModal: NgbActiveModal,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    console.log(this.category);
  }

  addCategory(form: NgForm) {
    if (form.valid) {
      var category = <Category>form.value;
      this.categoryService.addCategory(category).subscribe({
        next: (value: any) => {
          this.closeModal();
          this.categoryService.getUpdatedCategories.next('Updated categories');
        },
        error: (err: { error: { errors: any } }) => {
          this.showToaster(err.error.errors.categoryDto);
        },
      });
    }
  }

  updateCategory(categoryForm: NgForm, oldCategory: Category) {
    if (categoryForm.valid) {
      this.categoryService
        .updateCategory(oldCategory?.categoryName, categoryForm.value)
        .subscribe({
          next: (value) => {
            this.closeModal();
            this.categoryService.getUpdatedCategories.next(
              'Updated categories'
            );
          },
          error: (err: { error: { errors: any } }) => {
            this.showToaster(err.error.errors.categoryDto);
          },
        });
    }
  }

  closeModal() {
    this.activeModal.close();
  }

  showToaster(errorMsg: string) {
    this.toastr.error(errorMsg, 'Error');
  }
}
