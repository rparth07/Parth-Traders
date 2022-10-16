import { Component, OnInit } from '@angular/core';
import { fromEvent } from 'rxjs';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  enableHeader: boolean = false;
  adminName: string = 'admin';

  constructor(private authService: AuthService) {
    fromEvent(window, 'scroll').subscribe((event) => {
      window.pageYOffset >= 35
        ? (this.enableHeader = true)
        : (this.enableHeader = false);
    });
  }

  ngOnInit(): void {
    this.authService.adminNameSubject.subscribe((_: string) => {
      this.adminName = _;
    });
  }

  logout() {
    this.authService.logout();
  }
}
