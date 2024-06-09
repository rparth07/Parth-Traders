import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  categories: string[] = [];
  checkedCategories: { [key: string]: boolean } = {};
  animateX: { [key: string]: boolean } = {};
  animateY: { [key: string]: boolean } = {};

  constructor(private productService: ProductService) {
    this.categories = productService.getCategories();
  }

  ngOnInit(): void {
  }

  filterByCheckedCategories() {
    this.productService.productFilter$.next(this.getCheckedCategories());
  }

  toggleCategory(event: Event, category: string): void {
    event.preventDefault();
    if (this.checkedCategories[category]) {
      this.animateY[category] = false;
      setTimeout(() => {
        this.animateX[category] = false;
      }, 50);
      this.checkedCategories[category] = false;
    } else {
      this.animateX[category] = true;
      setTimeout(() => {
        this.animateY[category] = true;
      }, 100);
      this.checkedCategories[category] = true;
    }
    this.filterByCheckedCategories();
  }

  isChecked(category: string): boolean {
    return !!this.checkedCategories[category];
  }

  isAnimateX(category: string): boolean {
    return !!this.animateX[category];
  }

  isAnimateY(category: string): boolean {
    return !!this.animateY[category];
  }

  getCheckedCategories(): string[] {
    return Object.keys(this.checkedCategories).filter(key => this.checkedCategories[key]);
  }
}
