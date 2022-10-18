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

  constructor(private authService: AuthService) {
    this.admin = this.authService.admin;
  }

  ngOnInit(): void {}

  updateProfile(form: any) {
    this.authService.updateProfile(this.admin).subscribe({
      next: () => {
        console.log('Profile updated successfully!');
      },
      error: (err: any) => {
        console.log(err);
      },
    });
  }
}
