import { Component, Input, AfterViewInit, Renderer2, ViewChild, ElementRef } from '@angular/core';
import { Product } from '../core/models/Product';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements AfterViewInit {
  @Input() product!: Product;
  @ViewChild('carouselContainer') carouselContainer!: ElementRef<HTMLUListElement>;
  @ViewChild('productElement') productCardRef!: ElementRef<HTMLDivElement>;

  constructor(private renderer: Renderer2) { }

  ngAfterViewInit(): void {
    // Make carousel on component initialization
    this.makeCarousel();
  }

  // Method to change product color
  changeColor(color: string): void {
    // Implement color change logic here
  }

  // Method to lift product card
  liftCard(): void {
    const productCard = this.productCardRef.nativeElement;
    this.renderer.setStyle(productCard, 'z-index', '20');
    const make3D = productCard.querySelector('.make3D');
    if (make3D) {
      this.renderer.addClass(make3D, 'animate');
      const carouselNext = productCard.querySelector('div.carouselNext');
      const carouselPrev = productCard.querySelector('div.carouselPrev');
      if (carouselNext && carouselPrev) {
        this.renderer.addClass(carouselNext, 'visible');
        this.renderer.addClass(carouselPrev, 'visible');
      }
    }
  }

  // Method to drop product card
  dropCard(): void {
    const productCard = this.productCardRef.nativeElement;
    const make3D = productCard.querySelector('.make3D');
    if (make3D) {
      this.renderer.removeClass(make3D, 'animate');
      const carouselNext = productCard.querySelector('div.carouselNext');
      const carouselPrev = productCard.querySelector('div.carouselPrev');
      if (carouselNext && carouselPrev) {
        this.renderer.removeClass(carouselNext, 'visible');
        this.renderer.removeClass(carouselPrev, 'visible');
      }
    }
  }

  // Method to flip product card to back
  flipCardToBack(): void {
    this.renderer.removeClass(document.querySelector('div.carouselNext'), 'visible');
    this.renderer.removeClass(document.querySelector('div.carouselPrev'), 'visible');
    this.renderer.addClass(document.querySelector('.make3D'), 'flip-10');

    setTimeout(() => {
      this.renderer.removeClass(document.querySelector('.make3D'), 'flip-10');
      this.renderer.addClass(document.querySelector('.make3D'), 'flip90');
      this.renderer.setStyle(document.querySelector('.product-front .shadow'), 'display', 'block');
      this.renderer.setStyle(document.querySelector('.product-front'), 'display', 'none');
    }, 50);

    setTimeout(() => {
      this.renderer.removeClass(document.querySelector('.make3D'), 'flip90');
      this.renderer.addClass(document.querySelector('.make3D'), 'flip190');
      this.renderer.setStyle(document.querySelector('.product-back'), 'display', 'block');
      this.renderer.setStyle(document.querySelector('.product-back .shadow'), 'display', 'block');
      this.renderer.setStyle(document.querySelector('.product-back .shadow'), 'opacity', '0');
      setTimeout(() => {
        this.renderer.removeClass(document.querySelector('.make3D'), 'flip190');
        this.renderer.addClass(document.querySelector('.make3D'), 'flip180');
        this.renderer.setStyle(document.querySelector('.make3D'), 'transition', '100ms ease-out');
        this.renderer.addClass(document.querySelector('.cx'), 's1');
        setTimeout(() => { this.renderer.addClass(document.querySelector('.cx'), 's2'); }, 100);
        setTimeout(() => { this.renderer.addClass(document.querySelector('.cx'), 's3'); }, 200);
        this.renderer.addClass(document.querySelector('div.carouselNext'), 'visible');
        this.renderer.addClass(document.querySelector('div.carouselPrev'), 'visible');
      }, 100);
    }, 150);
  }

  // Method to flip product card to front
  flipCardToFront(): void {
    this.renderer.removeClass(document.querySelector('.make3D'), 'flip180');
    this.renderer.addClass(document.querySelector('.make3D'), 'flip190');

    setTimeout(() => {
      this.renderer.removeClass(document.querySelector('.make3D'), 'flip190');
      this.renderer.addClass(document.querySelector('.make3D'), 'flip90');
      this.renderer.setStyle(document.querySelector('.product-back .shadow'), 'opacity', '0');
      setTimeout(() => {
        this.renderer.setStyle(document.querySelector('.product-back'), 'display', 'none');
        this.renderer.setStyle(document.querySelector('.product-front .shadow'), 'display', 'block');
        this.renderer.setStyle(document.querySelector('.product-front .shadow'), 'opacity', '0');
        this.renderer.setStyle(document.querySelector('.make3D'), 'transition', '100ms ease-out');
        this.renderer.removeClass(document.querySelector('.cx'), 's1');
        this.renderer.removeClass(document.querySelector('.cx'), 's2');
        this.renderer.removeClass(document.querySelector('.cx'), 's3');
      }, 100);
    }, 50);

    setTimeout(() => {
      this.renderer.removeClass(document.querySelector('.make3D'), 'flip90');
      this.renderer.addClass(document.querySelector('.make3D'), 'flip-10');
      this.renderer.setStyle(document.querySelector('.product-front .shadow'), 'display', 'block');
      this.renderer.setStyle(document.querySelector('.product-front .shadow'), 'opacity', '0');
      setTimeout(() => {
        this.renderer.setStyle(document.querySelector('.product-front .shadow'), 'display', 'none');
        this.renderer.setStyle(document.querySelector('.make3D'), 'transition', '100ms ease-out');
      }, 100);
    }, 150);
  }

  // Method to make carousel
  makeCarousel(): void {
    const carousel = this.carouselContainer.nativeElement;
    let carouselWidth = 0;
    const carouselSlideWidth = 315;
    let isAnimating = false;
    let currSlide = 0;
    carousel.setAttribute('rel', currSlide.toString());

    // Building the width of the carousel
    carousel.querySelectorAll('li').forEach(() => {
      carouselWidth += carouselSlideWidth;
    });
    this.renderer.setStyle(carousel, 'width', `${carouselWidth}px`);

    // Load Next Image
    this.renderer.listen(document.querySelector('div.carouselNext'), 'click', () => {
      const currentLeft = Math.abs(parseInt(getComputedStyle(carousel).left));
      const newLeft = currentLeft + carouselSlideWidth;
      if (newLeft === carouselWidth || isAnimating === true) { return; }
      this.renderer.setStyle(carousel, 'left', `-${newLeft}px`);
      this.renderer.setStyle(carousel, 'transition', '300ms ease-out');
      isAnimating = true;
      currSlide++;
      carousel.setAttribute('rel', currSlide.toString());
      setTimeout(() => { isAnimating = false; }, 300);
    });

    // Load Previous Image
    this.renderer.listen(document.querySelector('div.carouselPrev'), 'click', () => {
      const currentLeft = Math.abs(parseInt(getComputedStyle(carousel).left));
      const newLeft = currentLeft - carouselSlideWidth;
      if (newLeft < 0 || isAnimating === true) { return; }
      this.renderer.setStyle(carousel, 'left', `-${newLeft}px`);
      this.renderer.setStyle(carousel, 'transition', '300ms ease-out');
      isAnimating = true;
      currSlide--;
      carousel.setAttribute('rel', currSlide.toString());
      setTimeout(() => { isAnimating = false; }, 300);
    });
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
