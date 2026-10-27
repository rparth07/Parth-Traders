import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {
  newsletterMessage = '';
  newsletterUnavailable(event: Event) {
    event.preventDefault();
    this.newsletterMessage = 'Email signup is not available yet. Please contact Parth Traders directly.';
  }

  constructor() { }

  ngOnInit(): void {
  }

}
