import { apiErrorMessage } from '../../../shared/api-error';
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
  saving=false;
  saveError='';
  @Input() category!: Category | null;

  constructor(
    private categoryService: CategoryService,
    private activeModal: NgbActiveModal,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    // console.log(this.category);
  }

  addCategory(form: NgForm) {
    if (this.saving) return;
    if (form.valid) {
      this.saving=true; this.saveError='';
      var category = <Category>form.value;
      this.categoryService.addCategory(category).subscribe({
        next: (value: any) => {
          this.saving=false; this.closeModal();
          this.categoryService.getUpdatedCategories.next('Updated categories');
        },
        error: (err: { error: { errors: any } }) => {
          this.showToaster(apiErrorMessage(err));
        },
      });
    }
  }

  updateCategory(categoryForm: NgForm, oldCategory: Category) {
    if (this.saving) return;
    if (categoryForm.valid) {
      this.saving=true; this.saveError='';
      this.categoryService
        .updateCategory(oldCategory?.categoryName, categoryForm.value)
        .subscribe({
          next: (value) => {
            this.saving=false; this.closeModal();
            this.categoryService.getUpdatedCategories.next(
              'Updated categories'
            );
          },
          error: (err: { error: { errors: any } }) => {
            this.showToaster(apiErrorMessage(err));
          },
        });
    }
  }

  closeModal() {
    this.activeModal.close();
  }

  showToaster(errorMsg: string) {
    this.saving=false; this.saveError=errorMsg; this.toastr.error(errorMsg, 'Save failed');
  }
}
