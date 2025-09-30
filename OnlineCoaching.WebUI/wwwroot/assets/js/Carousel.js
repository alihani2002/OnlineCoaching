$(document).ready(function () {
    if ($('.bbb_viewed_slider').length) {
        var viewedSlider = $('.bbb_viewed_slider');

        viewedSlider.owlCarousel({
            loop: true,
            margin: 20,
            autoplay: true,
            autoplayTimeout: 5000,
            nav: false,
            dots: false,
            responsive: {
                0: { items: 1 },
                575: { items: 2 },
                768: { items: 3 },
                991: { items: 4 },
                1199: { items: 5 }
            }
        });

        $('.bbb_viewed_prev').click(function () {
            viewedSlider.trigger('prev.owl.carousel');
        });

        $('.bbb_viewed_next').click(function () {
            viewedSlider.trigger('next.owl.carousel');
        });
    }
});
