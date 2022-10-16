import { Component, OnInit } from '@angular/core';
import { AdminDetails } from './admin-details';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  admin!: AdminDetails;

  ngOnInit(): void {}

  updateProfile(form: any) {}
}
