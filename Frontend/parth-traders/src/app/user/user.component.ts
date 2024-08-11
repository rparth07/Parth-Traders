import { Component, OnInit } from '@angular/core';
import { NavigationEnd, NavigationStart, Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css'],
})
export class UserComponent implements OnInit {
  isAuthenticationPage = true;
  constructor(router: Router) {
    // on route change to '/login', set the variable isAuthenticationPage to false
    router.events.forEach((event) => {
      if (event instanceof NavigationStart || event instanceof NavigationEnd) {
        if (event.url.endsWith('/authenticate')) {
          this.isAuthenticationPage = true;
        } else {
          this.isAuthenticationPage = false;
        }
      }
    });
  }

  ngOnInit(): void { }
}
