import { Component, Input, OnInit } from '@angular/core';
import { Product } from '../core/models/Product';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  @Input() product!: Product;
  constructor() { }

  ngOnInit(): void {
    this.makeCarousel();
  }

  changeColor(): void {
    return;
  }

  liftCard(): void {
    $('.product').css('z-index', "20");
    $('.make3D').addClass('animate');
    $('div.carouselNext').addClass('visible');
    $('div.carouselPrev').addClass('visible');
  }

  dropCard(): void {
    $('.make3D').removeClass('animate');
    $('.product').css('z-index', "20");
    $('div.carouselNext').removeClass('visible');
    $('div.carouselPrev').removeClass('visible');
  }

  flipCardToBack(): void {
    $('div.carouselNext').removeClass('visible');
    $('div.carouselPrev').removeClass('visible');
    $('.make3D').addClass('flip-10');

    setTimeout(function () {
      $('.make3D').removeClass('flip-10').addClass('flip90').find('div.shadow').show().fadeTo(80, 1, function () {
        $('.product-front, .product-front div.shadow').hide();
      });
    }, 50);

    setTimeout(function () {
      $('.make3D').removeClass('flip90').addClass('flip190');
      $('.product-back').show().find('div.shadow').show().fadeTo(90, 0);
      setTimeout(function () {
        $('.make3D').removeClass('flip190').addClass('flip180').find('div.shadow').hide();
        setTimeout(function () {
          $('.make3D').css('transition', '100ms ease-out');
          $('.cx, .cy').addClass('s1');
          setTimeout(function () { $('.cx, .cy').addClass('s2'); }, 100);
          setTimeout(function () { $('.cx, .cy').addClass('s3'); }, 200);
          $('div.carouselNext, div.carouselPrev').addClass('visible');
        }, 100);
      }, 100);
    }, 150);
  }

  flipCardToFront(): void {
    $('.make3D').removeClass('flip180').addClass('flip190');

    setTimeout(function () {
      $('.make3D').removeClass('flip190').addClass('flip90');

      $('.product-back div.shadow').css('opacity', 0).fadeTo(100, 1, function () {
        $('.product-back, .product-back div.shadow').hide();
        $('.product-front, .product-front div.shadow').show();
      });
    }, 50);

    setTimeout(function () {
      $('.make3D').removeClass('flip90').addClass('flip-10');
      $('.product-front div.shadow').show().fadeTo(100, 0);
      setTimeout(function () {
        $('.product-front div.shadow').hide();
        $('.make3D').removeClass('flip-10').css('transition', '100ms ease-out');
        $('.cx, .cy').removeClass('s1 s2 s3');
      }, 100);
    }, 150);
  }

  makeCarousel(): void {
    var carousel = $('.carousel ul');
    var carouselSlideWidth = 315;
    var carouselWidth = 0;
    var isAnimating = false;
    var currSlide = 0;
    $(carousel).attr('rel', currSlide);

    // building the width of the casousel
    $(carousel).find('li').each(function () {
      carouselWidth += carouselSlideWidth;
    });
    $(carousel).css('width', carouselWidth);

    // Load Next Image
    $('div.carouselNext').on('click', function () {
      var currentLeft = Math.abs(parseInt($(carousel).css("left")));
      var newLeft = currentLeft + carouselSlideWidth;
      if (newLeft == carouselWidth || isAnimating === true) { return; }
      $(carousel).css({
        'left': "-" + newLeft + "px",
        "transition": "300ms ease-out"
      });
      isAnimating = true;
      currSlide++;
      $(carousel).attr('rel', currSlide);
      setTimeout(function () { isAnimating = false; }, 300);
    });

    // Load Previous Image
    $('div.carouselPrev').on('click', function () {
      var currentLeft = Math.abs(parseInt($(carousel).css("left")));
      var newLeft = currentLeft - carouselSlideWidth;
      if (newLeft < 0 || isAnimating === true) { return; }
      $(carousel).css({
        'left': "-" + newLeft + "px",
        "transition": "300ms ease-out"
      });
      isAnimating = true;
      currSlide--;
      $(carousel).attr('rel', currSlide);
      setTimeout(function () { isAnimating = false; }, 300);
    });
  }

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
