import { Component, OnInit } from '@angular/core';
import { NgbNavbar } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  constructor() {
    jQuery(window).scroll(function () {
      if (window.pageYOffset >= 35) {
        //remove home-header class
        $('nav').removeClass('home-header');
      } else {
        //add home-header class
        $('nav').addClass('home-header');
      }
    });
  }

  ngOnInit(): void {}
}
