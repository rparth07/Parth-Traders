import { Component, Input, AfterViewInit, Renderer2, ViewChild, ElementRef } from '@angular/core';
import { Product } from '../core/models/Product';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements AfterViewInit {
  @Input() product!: Product;
  productCard!: HTMLDivElement;
  productImagePath: string;

  @ViewChild('productElement') productCardRef!: ElementRef<HTMLDivElement>;

  constructor(private renderer: Renderer2, carouselConfig: NgbCarouselConfig) {
    this.productImagePath = this.product?.image_paths[0];
    carouselConfig.interval = 0;
    carouselConfig.keyboard = true;
  }

  ngAfterViewInit(): void {
    // Make carousel on component initialization
    this.productCard = this.productCardRef.nativeElement;
  }

  // Method to change product color
  changeColor(color: string): void {
    // Implement color change logic here
  }

  // Method to lift product card
  liftCard(): void {
    this.renderer.setStyle(this.productCard, 'z-index', '20');
    const make3D = this.productCard.querySelector('.make3D');
    if (make3D) {
      this.renderer.addClass(make3D, 'animate');
      const carouselNext = this.productCard.querySelector('div.carouselNext');
      const carouselPrev = this.productCard.querySelector('div.carouselPrev');
      if (carouselNext && carouselPrev) {
        this.renderer.addClass(carouselNext, 'visible');
        this.renderer.addClass(carouselPrev, 'visible');
      }
    }
  }

  // Method to drop product card
  dropCard(): void {
    const make3D = this.productCard.querySelector('.make3D');
    if (make3D) {
      this.renderer.removeClass(make3D, 'animate');
      const carouselNext = this.productCard.querySelector('div.carouselNext');
      const carouselPrev = this.productCard.querySelector('div.carouselPrev');
      if (carouselNext && carouselPrev) {
        this.renderer.removeClass(carouselNext, 'visible');
        this.renderer.removeClass(carouselPrev, 'visible');
      }
    }
  }

  // Method to flip product card to back
  flipCardToBack(): void {
    const make3D = this.productCard.querySelector('.make3D');
    if (make3D)
      this.renderer.addClass(make3D, 'flip-10');

    setTimeout(() => {
      this.renderer.removeClass(make3D, 'flip-10');
      this.renderer.addClass(make3D, 'flip90');
      this.renderer.setStyle(this.productCard.querySelector('.product-front .shadow'), 'display', 'block');
      this.renderer.setStyle(this.productCard.querySelector('.product-front'), 'display', 'none');
    }, 50);

    setTimeout(() => {
      this.renderer.removeClass(make3D, 'flip90');
      this.renderer.addClass(make3D, 'flip190');
      this.renderer.setStyle(this.productCard.querySelector('.product-back'), 'display', 'block');
      this.renderer.setStyle(this.productCard.querySelector('.product-back .shadow'), 'opacity', '0');
      setTimeout(() => {
        this.renderer.removeClass(make3D, 'flip190');
        this.renderer.addClass(make3D, 'flip180');
        this.renderer.setStyle(make3D, 'transition', '100ms ease-out');
        this.renderer.addClass(this.productCard.querySelector('.cx'), 's1');
        this.renderer.addClass(this.productCard.querySelector('.cy'), 's1');
        setTimeout(() => {
          this.renderer.addClass(this.productCard.querySelector('.cx'), 's2');
          this.renderer.addClass(this.productCard.querySelector('.cy'), 's2');
        }, 100);
        setTimeout(() => {
          this.renderer.addClass(this.productCard.querySelector('.cx'), 's3');
          this.renderer.addClass(this.productCard.querySelector('.cy'), 's3');
        }, 200);
      }, 100);
    }, 150);
  }

  // Method to flip product card to front
  flipCardToFront(): void {
    const make3D = this.productCard.querySelector('.make3D');
    if (make3D) {
      this.renderer.removeClass(make3D, 'flip180');
      this.renderer.addClass(make3D, 'flip190');
    }

    setTimeout(() => {
      this.renderer.removeClass(make3D, 'flip190');
      this.renderer.addClass(make3D, 'flip90');
      this.renderer.setStyle(this.productCard.querySelector('.product-back .shadow'), 'opacity', '0');
      this.renderer.setStyle(this.productCard.querySelector('.product-back .shadow'), 'transition', 'opacity 100ms');
      setTimeout(() => {
        this.renderer.setStyle(this.productCard.querySelector('.product-back'), 'display', 'none');
        this.renderer.setStyle(this.productCard.querySelector('.product-back .shadow'), 'display', 'none');
        this.renderer.setStyle(this.productCard.querySelector('.product-front'), 'display', 'block');
        this.renderer.setStyle(this.productCard.querySelector('.product-front .shadow'), 'display', 'none');
      }, 100);
    }, 50);

    setTimeout(() => {
      this.renderer.removeClass(make3D, 'flip90');
      this.renderer.addClass(make3D, 'flip-10');
      // this.renderer.setStyle(this.productCard.querySelector('.product-front .shadow'), 'display', 'block');
      // this.renderer.setStyle(this.productCard.querySelector('.product-front .shadow'), 'opacity', '0');
      setTimeout(() => {
        // this.renderer.setStyle(this.productCard.querySelector('.product-front .shadow'), 'display', 'none');
        this.renderer.removeClass(make3D, 'flip-10');
        this.renderer.setStyle(make3D, 'transition', '100ms ease-out');

        this.renderer.removeClass(this.productCard.querySelector('.cx'), 's1');
        this.renderer.removeClass(this.productCard.querySelector('.cx'), 's2');
        this.renderer.removeClass(this.productCard.querySelector('.cx'), 's3');

        this.renderer.removeClass(this.productCard.querySelector('.cy'), 's1');
        this.renderer.removeClass(this.productCard.querySelector('.cy'), 's2');
        this.renderer.removeClass(this.productCard.querySelector('.cy'), 's3');
      }, 100);
    }, 150);
  }

  // Method to add product to cart in large view
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

  // Method to add product to cart in normal view
  addToCart(): void {
    var productCard = $(this).parent();
    var position = productCard.offset()!;
    var productImage = ($('img').get(0)! as HTMLImageElement).src;
    var productName = $('.product_name').get(0)!.innerHTML;

    $("body").append('<div class="floating-cart"></div>');
    var cart = $('div.floating-cart');
    productCard.clone().appendTo(cart);
    $(cart).css({ 'top': position.top + 'px', "left": position.left + 'px' }).fadeIn("slow").addClass('moveToCart');
    setTimeout(function () { $("body").addClass("MakeFloatingCart"); }, 800);
    setTimeout(function () {
      $('div.floating-cart').remove();
      $("body").removeClass("MakeFloatingCart");


      var cartItem = "<div class='cart-item'><div class='img-wrap'><img src='" + productImage + "' alt='' /></div><span>" + productName + "</span><strong>$39</strong><div class='cart-item-border'></div><div class='delete-item'></div></div>";

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
}
