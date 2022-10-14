import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css'],
})
export class LogInComponent implements OnInit {
  constructor(private authService: AuthService) {}

  ngOnInit(): void {}

  login(form: NgForm) {
    this.authService.login(form.value).subscribe((res) => {
      console.log('res from login', res);
    });
  }
}
