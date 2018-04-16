function InitSlider(){
    $('.OneSlideSlider').slick({
        autoplay: false,      
        arrows: false,   
        slidesToShow: 1,
        slidesToScroll: 1,
        asNavFor : ".ManySlideSlider"

    });
    $('.ManySlideSlider').slick({     
        arrows: false,
        slidesToShow: 4,
        slidesToScroll: 1,
        focusOnSelect: true,
        vertical: true,
        draggable: false,
        adaptiveHeight: true,
        asNavFor : ".OneSlideSlider"

    });
  }