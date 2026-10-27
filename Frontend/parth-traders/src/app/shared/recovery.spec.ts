import { discountedUnitPrice } from './pricing';
import { fakeAsync, flushMicrotasks, TestBed, tick } from '@angular/core/testing';
import { HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from '../user/services/auth.service';
import { ProductService } from '../user/services/product.service';
import { HomeComponent } from '../user/home/home.component';
import { CartService } from '../user/services/cart.service';
import { Product } from '../user/core/models/Product';
import { AuthInterceptor } from '../admin/auth.interceptor';
import { AppSetting } from './app-settings';
describe('Store recovery regressions', () => {
  let http: HttpTestingController;
  const product = { productId: 1, productSku: 'TEST-ONLY', productName: 'Test fixture hammer', categoryName: '', unitPrice: 20.99, discount: 5, productRating: 4.8, productType: 0, unitsInStock: 10 } as Product;
  beforeEach(() => {
    localStorage.clear(); sessionStorage.clear();
    TestBed.configureTestingModule({ imports: [HttpClientTestingModule], providers: [
      { provide: Router, useValue: { navigate: jasmine.createSpy().and.resolveTo(true) } },
      { provide: JwtHelperService, useValue: { isTokenExpired: () => false } },
      { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
    ] }); http = TestBed.inject(HttpTestingController);
  });
  afterEach(() => { http.verify(); localStorage.clear(); });
  it('caps cart quantities at available stock and refreshes source prices', () => {
    const cart=TestBed.inject(CartService); const limited={...product,unitsInStock:1};
    expect(cart.addToCart(limited)).toBeTrue(); expect(cart.addToCart(limited)).toBeFalse();
    cart.incrementProductCountOf(cart.getCartItems()[0]); expect(cart.getCartItems()[0].quantity).toBe(1);
    cart.refreshProducts([{...limited,unitPrice:10,discount:20}]); expect(cart.getTotalOrderPrice()).toBe(8);
  });
  it('sorts filtered products by the actual selling price and resets all filters', () => {
    const service=TestBed.inject(ProductService); const home=new HomeComponent({} as any,{} as any,service,TestBed.inject(CartService));home.ngOnInit();
    http.expectOne(AppSetting.API_URL+'API/admin/products').flush([product,{...product,productSku:'SECOND',productName:'Safety glasses',unitPrice:15.99,discount:20}]);
    home.sortOrder='price-asc';home.applyFilters();expect(home.shownProducts[0].productSku).toBe('SECOND');
    service.productFilter$.next({...service.productFilter$.value,name:'missing'});expect(home.shownProducts.length).toBe(0);home.resetFilters();expect(home.shownProducts.length).toBe(2);home.ngOnDestroy();
  });
  it('applies percentage discounts and rounds unit prices', () => {
    expect(discountedUnitPrice(15.99, 20)).toBe(12.79);
    expect(discountedUnitPrice(0.05, 10)).toBe(0.05);
  });
  it('awaits login response before reporting success', fakeAsync(() => {
    let result: any;
    TestBed.inject(AuthService).loginApi('fixture@example.invalid', 'test-only').then(value => result = value);
    expect(result).toBeUndefined();
    http.expectOne(AppSetting.API_URL + 'api/user/authentication/login').flush({ token: 'fixture-token', customer: { userName: 'fixture' } });
    flushMicrotasks(); expect(result.success).toBeTrue();
  }));
  it('awaits registration and reports failure truthfully', fakeAsync(() => {
    let result: any;
    TestBed.inject(AuthService).registerApi('fixture','123','test','fixture@example.invalid','test-only').then(value => result = value);
    expect(result).toBeUndefined();
    http.expectOne(AppSetting.API_URL + 'api/user/authentication/signup').flush({}, {status: 503, statusText: 'Unavailable'});
    flushMicrotasks(); expect(result.success).toBeFalse(); expect(result.message).toBeTruthy();
  }));
  it('keeps search entered before products finish loading', () => {
    const service = TestBed.inject(ProductService);
    const home = new HomeComponent({} as any, {} as any, service, TestBed.inject(CartService)); home.ngOnInit();
    service.productFilter$.next({...service.productFilter$.value, name: 'missing'});
    http.expectOne(AppSetting.API_URL + 'API/admin/products').flush([product]); expect(home.shownProducts.length).toBe(0);
    service.productFilter$.next({...service.productFilter$.value, name: 'hammer'}); expect(home.shownProducts.length).toBe(1); service.productFilter$.next({...service.productFilter$.value, priceRange: {min: 0, max: 20}}); expect(home.shownProducts.length).toBe(1); home.ngOnDestroy();
  });
  it('recovers a failed catalog request with a real retry', () => {
    const home = new HomeComponent({} as any, {} as any, TestBed.inject(ProductService), TestBed.inject(CartService)); home.ngOnInit();
    http.expectOne(AppSetting.API_URL + 'API/admin/products').error(new ProgressEvent('error'));
    expect(home.fetching).toBeFalse(); expect(home.loadError).toBeTruthy(); home.fetchProducts();
    http.expectOne(AppSetting.API_URL + 'API/admin/products').flush([product]); expect(home.loadError).toBe(''); expect(home.shownProducts.length).toBe(1); home.ngOnDestroy();
  });
  it('bounds stalled API requests', fakeAsync(() => {
    let failed = false; TestBed.inject(HttpClient).get(AppSetting.API_URL + 'API/admin/products').subscribe({error: () => failed = true});
    const req = http.expectOne(AppSetting.API_URL + 'API/admin/products'); tick(90001); expect(req.cancelled).toBeTrue(); expect(failed).toBeTrue();
  }));
  it('never sends credentials to third parties', () => {
    localStorage.setItem('access_token','fixture-secret'); TestBed.inject(HttpClient).get('https://example.invalid/public').subscribe();
    const req = http.expectOne('https://example.invalid/public'); expect(req.request.headers.has('Authorization')).toBeFalse(); req.flush({});
  });
  it('separates customer and admin tokens', () => {
    localStorage.setItem('access_token','customer'); localStorage.setItem('admin_access_token','admin'); TestBed.inject(HttpClient).get(AppSetting.API_URL + 'API/admin/products').subscribe();
    const req = http.expectOne(AppSetting.API_URL + 'API/admin/products'); expect(req.request.headers.get('Authorization')).toBe('Bearer admin'); req.flush([]);
  });
  it('uses displayed discounted prices for every cart quantity', () => {
    const cart = TestBed.inject(CartService); expect(cart.isCartEmpty()).toBeTrue(); cart.addToCart(product); expect(cart.getTotalOrderPrice()).toBeCloseTo(19.94, 2);
    cart.addToCart(product); expect(cart.getTotalOrderPrice()).toBeCloseTo(39.88, 2); cart.decrementProductCountFrom(cart.getCartItems()[0]); expect(cart.getTotalOrderPrice()).toBeCloseTo(19.94, 2); const restored = new CartService(); expect(restored.getTotalOrderPrice()).toBeCloseTo(19.94, 2); restored.addToCart(product); expect(restored.getTotalOrderPrice()).toBeCloseTo(39.88, 2);
  });
});
