import { Component, Input, AfterViewInit, Renderer2, ViewChild, ElementRef, OnInit, OnDestroy, EventEmitter, Output, SimpleChanges, OnChanges } from '@angular/core';
import { Product } from '../core/models/Product';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
import { Observable, Subscription } from 'rxjs';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit, AfterViewInit, OnDestroy, OnChanges {
  @Input() product!: Product;
  selectedProductSize: string | null = null;
  productCard: HTMLDivElement | undefined;
  productImagePath: string;
  @ViewChild('productElement') productCardRef!: ElementRef<HTMLDivElement>;

  private changeGridToLargeSubject$!: Subscription;
  @Input() changeGridToLargeObservable!: Observable<boolean>;
  @Input('shouldSwitchToLargeGrid') shouldSwitchToLargeGrid: boolean = false;

  @Output() addToCartEvent = new EventEmitter<{ productCard: HTMLDivElement, product: Product, productSize: string }>();

  constructor(private renderer: Renderer2, carouselConfig: NgbCarouselConfig) {
    this.productImagePath = this.product?.image_paths[0];
    carouselConfig.interval = 0;
    carouselConfig.keyboard = true;
  }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.productCard == undefined)
      return;

    if (changes["shouldSwitchToLargeGrid"].currentValue === false) {
      this.flipCardToFront();
    } else {
      this.flipCardIfLargeGrid();
    }
  }

  ngAfterViewInit(): void {
    // Make carousel on component initialization
    this.productCard = this.productCardRef.nativeElement;
    this.flipCardIfLargeGrid();
  }

  flipCardIfLargeGrid() {
    const make3D = this.productCard?.querySelector('.make3D');
    if (this.shouldSwitchToLargeGrid) {
      this.renderer.addClass(make3D, 'flip180');
    } else {
      this.renderer.removeClass(make3D, 'flip180');
    }
  }

  ngOnDestroy(): void {
    this.changeGridToLargeSubject$.unsubscribe();
  }

  // Method to lift product card
  liftCard(): void {
    this.renderer.setStyle(this.productCard, 'z-index', '20');
    const make3D = this.productCard?.querySelector('.make3D');
    if (make3D) {
      this.renderer.addClass(make3D, 'animate');
      const carouselNext = this.productCard?.querySelector('div.carouselNext');
      const carouselPrev = this.productCard?.querySelector('div.carouselPrev');
      if (carouselNext && carouselPrev) {
        this.renderer.addClass(carouselNext, 'visible');
        this.renderer.addClass(carouselPrev, 'visible');
      }
    }
  }

  // Method to drop product card
  dropCard(): void {
    const make3D = this.productCard?.querySelector('.make3D');
    if (make3D) {
      this.renderer.removeClass(make3D, 'animate');
      const carouselNext = this.productCard?.querySelector('div.carouselNext');
      const carouselPrev = this.productCard?.querySelector('div.carouselPrev');
      if (carouselNext && carouselPrev) {
        this.renderer.removeClass(carouselNext, 'visible');
        this.renderer.removeClass(carouselPrev, 'visible');
      }
    }
  }

  // Method to flip product card to back
  flipCardToBack(): void {
    const make3D = this.productCard?.querySelector('.make3D');
    if (make3D)
      this.renderer.addClass(make3D, 'flip-10');

    setTimeout(() => {
      this.renderer.removeClass(make3D, 'flip-10');
      this.renderer.addClass(make3D, 'flip90');
      this.renderer.setStyle(this.productCard?.querySelector('.product-front'), 'display', 'none');
    }, 50);

    setTimeout(() => {
      this.renderer.removeClass(make3D, 'flip90');
      this.renderer.addClass(make3D, 'flip190');
      this.renderer.setStyle(this.productCard?.querySelector('.product-back'), 'display', 'block');
      setTimeout(() => {
        this.renderer.removeClass(make3D, 'flip190');
        this.renderer.addClass(make3D, 'flip180');
        this.renderer.setStyle(make3D, 'transition', '100ms ease-out');
        this.renderer.addClass(this.productCard?.querySelector('.cx'), 's1');
        this.renderer.addClass(this.productCard?.querySelector('.cy'), 's1');
        setTimeout(() => {
          this.renderer.addClass(this.productCard?.querySelector('.cx'), 's2');
          this.renderer.addClass(this.productCard?.querySelector('.cy'), 's2');
        }, 100);
        setTimeout(() => {
          this.renderer.addClass(this.productCard?.querySelector('.cx'), 's3');
          this.renderer.addClass(this.productCard?.querySelector('.cy'), 's3');
        }, 200);
      }, 100);
    }, 150);
  }

  // Method to flip product card to front
  flipCardToFront(): void {
    const make3D = this.productCard?.querySelector('.make3D');
    if (make3D) {
      this.renderer.removeClass(make3D, 'flip180');
      this.renderer.addClass(make3D, 'flip190');
    }

    setTimeout(() => {
      this.renderer.removeClass(make3D, 'flip190');
      this.renderer.addClass(make3D, 'flip90');
      setTimeout(() => {
        this.renderer.setStyle(this.productCard?.querySelector('.product-back'), 'display', 'none');
        this.renderer.setStyle(this.productCard?.querySelector('.product-front'), 'display', 'block');
      }, 100);
    }, 50);

    setTimeout(() => {
      this.renderer.removeClass(make3D, 'flip90');
      this.renderer.addClass(make3D, 'flip-10');
      setTimeout(() => {
        this.renderer.removeClass(make3D, 'flip-10');
        this.renderer.setStyle(make3D, 'transition', '100ms ease-out');

        this.renderer.removeClass(this.productCard?.querySelector('.cx'), 's1');
        this.renderer.removeClass(this.productCard?.querySelector('.cx'), 's2');
        this.renderer.removeClass(this.productCard?.querySelector('.cx'), 's3');

        this.renderer.removeClass(this.productCard?.querySelector('.cy'), 's1');
        this.renderer.removeClass(this.productCard?.querySelector('.cy'), 's2');
        this.renderer.removeClass(this.productCard?.querySelector('.cy'), 's3');
      }, 100);
    }, 150);
  }

  //[WIP] Method to add product to cart in large view
  addCartLarge(): void {
    var carousel = $(".carousel-container");
    const imgIndex: number = parseInt(carousel.attr("rel") || "0", 10); // Parsing to integer with fallback to 0 if "rel" attribute is not set
    const img: HTMLImageElement = carousel.find('img').eq(imgIndex).get(0)!;

    var position = $(img).offset()!;

    var productName = $('h4').get(0)!.innerHTML;

    $("body").append('<div class="floating-cart"></div>');
    var cart = $('div.floating-cart');
    $("<img src='" + img.src + "' class='floating-image-large' />").appendTo(cart);

    $(cart).css({ 'top': position.top + 'px', "left": position.left + 'px' }).fadeIn("slow").addClass('moveToCart');
    setTimeout(function () { $("body").addClass("MakeFloatingCart"); }, 800);

    setTimeout(function () {
      $('div.floating-cart').remove();
      $("body").removeClass("MakeFloatingCart");

      var cartItem = "<div class='cart-item'><div class='img-wrap'><img src='" + img.src + "' alt='' /></div><span>" + productName + "</span><strong>$39</strong><div class='cart-item-border'></div><div class='delete-item'></div></div>";

      $("#cart .empty").hide();
      $("#cart").append(cartItem);
      $("#checkout").fadeIn(500);

      $("#cart .cart-item").last()
        .addClass("flash")
        .find(".delete-item").click(function () {
          $(this).parent().fadeOut(300, function () {
            $(this).remove();
            if ($("#cart .cart-item").length == 0) {
              $("#cart .empty").fadeIn(500);
              $("#checkout").fadeOut(500);
            }
          })
        });
      setTimeout(function () {
        $("#cart .cart-item").last().removeClass("flash");
      }, 10);

    }, 1000);
  }

  // Method to handle add to cart functionality
  addToCart(): void {
    if (this.selectedProductSize) {
      const productFrontDiv: HTMLDivElement = this.productCard?.querySelectorAll('.product-front')[0] as HTMLDivElement;
      this.addToCartEvent.emit({ productCard: productFrontDiv, product: this.product, productSize: this.selectedProductSize as string });
    } else {
      alert("Please select a size.");
    }
  }

  updateProductSize(productSize: string) {
    this.selectedProductSize = productSize;
  }
}
