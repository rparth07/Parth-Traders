import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { AdminDetails } from './admin-details';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  admin!: AdminDetails;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.adminSubject.subscribe((_: AdminDetails) => {
      this.admin = _;
    });
  }

  updateProfile(form: any) {}
}
