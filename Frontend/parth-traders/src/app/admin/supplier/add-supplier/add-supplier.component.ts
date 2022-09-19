import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { Supplier } from '../supplier';
import { SupplierService } from '../supplier.service';

@Component({
  selector: 'app-add-supplier',
  templateUrl: './add-supplier.component.html',
  styleUrls: ['./add-supplier.component.css'],
})
export class AddSupplierComponent implements OnInit {
  @Input() supplier!: Supplier | null;

  constructor(
    private supplierService: SupplierService,
    private activeModal: NgbActiveModal,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    console.log(this.supplier);
  }

  addSupplier(form: NgForm) {
    if (form.valid) {
      var supplier = <Supplier>form.value;
      this.supplierService.addSupplier(supplier).subscribe({
        next: (value: any) => {
          this.closeModal();
          this.supplierService.getUpdatedSuppliers.next('Updated suppliers');
        },
        error: (err: { error: { errors: any } }) => {
          this.showToaster(err.error.errors.supplierDto);
        },
      });
    }
  }

  updateSupplier(supplierForm: NgForm, oldSupplier: Supplier) {
    if (supplierForm.valid) {
      this.supplierService
        .updateSupplier(oldSupplier?.supplierName, supplierForm.value)
        .subscribe({
          next: (value) => {
            this.closeModal();
            this.supplierService.getUpdatedSuppliers.next('Updated suppliers');
          },
          error: (err: { error: { errors: any } }) => {
            this.showToaster(err.error.errors.supplierDto);
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
