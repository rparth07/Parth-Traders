import { CommonModule } from '@angular/common';
import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { SidebarComponent } from '../user/sidebar/sidebar.component';
import { HomeComponent } from '../user/home/home.component';
import { ProductService } from '../user/services/product.service';
import { CartService } from '../user/services/cart.service';
import { AppSetting } from './app-settings';

describe('Actual price and rating controls', () => {
  let fixture: ComponentFixture<SidebarComponent>;
  let home: HomeComponent;
  let service: ProductService;
  let http: HttpTestingController;
  const products = [
    {productId:1,productSku:'CONTROL-CHEAP',productName:'Cheap fixture',unitPrice:10,discount:0,productRating:4.6},
    {productId:2,productSku:'CONTROL-SALE',productName:'Discounted fixture',unitPrice:25,discount:20,productRating:4.8},
    {productId:3,productSku:'CONTROL-HIGH',productName:'Higher fixture',unitPrice:50,discount:0,productRating:4.9},
    {productId:4,productSku:'CONTROL-EXTRA',productName:'Extra fixture',unitPrice:100,discount:0,productRating:4.9}
  ].map(p=>({...p,categoryName:'Control fixtures',productDescription:'Test only',productType:0,unitsInStock:5}));
  beforeEach(fakeAsync(() => {
    sessionStorage.clear();
    TestBed.configureTestingModule({imports:[CommonModule,FormsModule,HttpClientTestingModule],declarations:[SidebarComponent]});
    http=TestBed.inject(HttpTestingController);service=TestBed.inject(ProductService);
    home=new HomeComponent({} as any,{} as any,service,TestBed.inject(CartService));home.ngOnInit();
    fixture=TestBed.createComponent(SidebarComponent);fixture.detectChanges();
    http.expectOne(AppSetting.API_URL+'API/admin/categories').flush([{categoryName:'Control fixtures'}]);
    http.expectOne(AppSetting.API_URL+'API/admin/products').flush(products.map(p=>({...p})));
    tick();fixture.detectChanges();tick();fixture.detectChanges();
  }));
  afterEach(()=>{http.verify();home.ngOnDestroy();fixture.destroy();sessionStorage.clear();});
  function enter(name:string,value:string){
    const input=fixture.nativeElement.querySelector('input[name="'+name+'"]') as HTMLInputElement;
    input.value=value;input.dispatchEvent(new Event('input'));tick();fixture.detectChanges();tick();fixture.detectChanges();
  }
  function rating(label:string){
    const select=fixture.nativeElement.querySelector('#minimum-rating') as HTMLSelectElement;
    select.value=Array.from(select.options).find(o=>o.textContent?.trim()===label)!.value;
    select.dispatchEvent(new Event('change'));tick();fixture.detectChanges();tick();fixture.detectChanges();
  }
  function click(text:string){
    const button=(Array.from(fixture.nativeElement.querySelectorAll('button')) as HTMLButtonElement[]).find(b=>b.textContent?.trim()===text)!;
    button.click();tick();fixture.detectChanges();tick();fixture.detectChanges();
  }
  it('filters immediately from real numeric input events, including discounted boundary prices',fakeAsync(()=>{
    enter('maximum','20');expect(home.filterProducts.map(p=>p.productSku)).toEqual(['CONTROL-CHEAP','CONTROL-SALE']);
    enter('minimum','20');expect(home.filterProducts.map(p=>p.productSku)).toEqual(['CONTROL-SALE']);
    expect(fixture.nativeElement.textContent).toContain('1 matching products');
    expect(home.activeFilters).toContain('Minimum price: $20.00');
  }));
  it('uses numeric rating values and combines them with the price range',fakeAsync(()=>{
    rating('4.9 stars & up');expect(home.filterProducts.length).toBe(2);
    enter('maximum','60');expect(home.filterProducts.map(p=>p.productSku)).toEqual(['CONTROL-HIGH']);
    rating('Any rating');expect(home.filterProducts.length).toBe(3);
  }));
  it('restores an unbounded side when its input is cleared',fakeAsync(()=>{
    enter('minimum','20');enter('maximum','50');expect(home.filterProducts.length).toBe(2);
    enter('minimum','');expect(service.productFilter$.value.priceRange.min).toBeNull();expect(home.filterProducts.length).toBe(3);
    enter('maximum','');expect(home.filterProducts.length).toBe(4);
  }));
  it('shows invalid ranges and negative values without silently replacing the previous valid range',fakeAsync(()=>{
    enter('maximum','20');enter('minimum','50');expect(home.filterProducts.length).toBe(2);
    expect(fixture.nativeElement.querySelector('[role="alert"]').textContent).toContain('Minimum must not exceed maximum');
    expect(service.productFilter$.value.priceRange.min).toBeNull();
    enter('maximum','60');expect(home.filterProducts.map(p=>p.productSku)).toEqual(['CONTROL-HIGH']);
    enter('minimum','-1');expect(fixture.nativeElement.querySelector('[role="alert"]').textContent).toContain('zero or more');
    expect(service.productFilter$.value.priceRange.min).toBe(50);
  }));
  it('clears price separately and resets both selected controls with Clear all',fakeAsync(()=>{
    rating('4.9 stars & up');enter('maximum','60');click('Clear price');expect(home.filterProducts.length).toBe(2);
    enter('minimum','50');click('Clear all');expect(home.filterProducts.length).toBe(4);
    expect(service.productFilter$.value.ratingRange.min).toBeNull();expect(home.activeFilters).toEqual([]);
    expect((fixture.nativeElement.querySelector('input[name="minimum"]') as HTMLInputElement).value).toBe('');
  }));
  it('positions mobile results after the filter panel has left layout',fakeAsync(()=>{
    const scroll=jasmine.createSpy();const focus=jasmine.createSpy();
    home.resultsHeading={nativeElement:{isConnected:true,scrollIntoView:scroll,focus}} as any;
    home.filtersOpen=true;home.showFilterResults();expect(home.filtersOpen).toBeFalse();expect(scroll).not.toHaveBeenCalled();
    tick();expect(scroll).toHaveBeenCalledWith({block:'start',behavior:'instant'});expect(focus).toHaveBeenCalledWith({preventScroll:true});
  }));
  it('exposes zero results and an explicit action to return to the product list',fakeAsync(()=>{
    rating('4.9 stars & up');enter('maximum','20');expect(home.filterProducts.length).toBe(0);
    expect(fixture.nativeElement.textContent).toContain('0 matching products');
    const emitted=jasmine.createSpy();fixture.componentInstance.filtersApplied.subscribe(emitted);click('Show 0 products');expect(emitted).toHaveBeenCalled();
  }));
});
