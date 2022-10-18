import { Component, OnInit } from '@angular/core';
import { NavigationEnd, NavigationStart, Router } from '@angular/router';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css'],
})
export class AdminComponent implements OnInit {
  IsUserLogin = false;
  constructor(private router: Router) {
    // on route change to '/login', set the variable IsUserLogin to false
    router.events.forEach((event) => {
      if (event instanceof NavigationStart || event instanceof NavigationEnd) {
        if (event.url.endsWith('/login')) {
          this.IsUserLogin = false;
        } else {
          this.IsUserLogin = true;
        }
      }
    });
  }

  ngOnInit(): void {}
}
