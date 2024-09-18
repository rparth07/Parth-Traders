import { Component, OnInit } from '@angular/core';
import { fromEvent } from 'rxjs';
import { AuthService } from '../auth.service';
import { AdminDetails } from '../profile/admin-details';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  enableHeader: boolean = false;
  adminName: string = 'admin';

  constructor(private authService: AuthService) {
    this.adminName = this.authService.admin!.userName;
    fromEvent(window, 'scroll').subscribe((event) => {
      window.pageYOffset >= 35
        ? (this.enableHeader = true)
        : (this.enableHeader = false);
    });
  }

  ngOnInit(): void {
    this.authService.adminName.subscribe((res: string) => {
      this.adminName = res;
    });
    // console.log(this.adminName);
  }

  logout() {
    this.authService.logout();
  }
}
