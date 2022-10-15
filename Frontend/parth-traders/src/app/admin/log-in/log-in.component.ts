import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css'],
})
export class LogInComponent implements OnInit {
  isLoadingResults = false;

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit(): void {}

  login(form: NgForm) {
    this.authService.login(form.value).subscribe({
      next: (res) => {
        this.isLoadingResults = false;
        this.router
          .navigate(['/admin/products'])
          .then((_) => console.log('You are secure now!'));
      },
      error: (err: any) => {
        console.log(err);
        this.isLoadingResults = false;
      },
    });
  }
}
