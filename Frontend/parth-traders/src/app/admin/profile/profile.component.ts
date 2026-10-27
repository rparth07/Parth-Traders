import { Component, OnChanges, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { AdminDetails } from './admin-details';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  admin!: AdminDetails;
  message = '';
  saving = false;

  constructor(private authService: AuthService) {
    this.admin = this.authService.admin!;
  }

  ngOnInit(): void { }

  updateProfile(form: any) {
    if (form.invalid || this.saving) return;
    this.saving = true; this.message = '';
    this.authService.updateProfile(this.admin).subscribe({
      next: (res) => {
        this.admin = res.admin;
        this.saving = false; this.message = 'Profile saved.';
      },
      error: (err: any) => {
        this.saving = false; this.message = 'Profile could not be saved. Check your details and try again.';
      },
    });
  }
}
