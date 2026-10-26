import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css'],
})
export class LogInComponent implements OnInit {
  isLoadingResults = false;
  loadError = '';

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit(): void {}

  //need to add errors in the form
  login(form: NgForm) {
    if (this.isLoadingResults) return;
    if (form.invalid) { this.loadError = 'Enter your username and password.'; return; }
    this.loadError = '';
    this.isLoadingResults = true;
    this.authService.login(form.value).subscribe({
      next: () => {
        this.isLoadingResults = false;
        this.router
          .navigate(['/admin/products'])
          .then((_) => console.log('You are secure now!'));
      },
      error: (err: any) => {
        this.loadError = 'Unable to sign in. Check your details or try again later.';
        this.isLoadingResults = false;
      },
    });
  }
}
