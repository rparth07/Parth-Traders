import { Component, OnInit } from '@angular/core';
import { NavigationEnd, NavigationStart, Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css'],
})
export class UserComponent implements OnInit {
  IsUserLogin = false;
  constructor(router: Router) {
    // on route change to '/login', set the variable IsUserLogin to false
    router.events.forEach((event) => {
      if (event instanceof NavigationStart || event instanceof NavigationEnd) {
        if (event.url.endsWith('/authenticate')) {
          this.IsUserLogin = false;
        } else {
          this.IsUserLogin = true;
        }
      }
    });
  }

  ngOnInit(): void { }
}
