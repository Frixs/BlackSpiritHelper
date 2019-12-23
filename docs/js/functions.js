(function($) {
    "use strict"; // Start of use strict.

    // Smooth scrolling using jQuery easing.
    $('a.js-scroll-trigger[href*="#"]:not([href="#"])').click(function() {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
            var target = $(this.hash);

            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');

            if (target.length) {
                $('html, body').animate({ scrollTop: (target.offset().top - 70) }, 1000, "easeInOutExpo");
                return false;
            }
        }
    });

    // Add active class to navbar items on scroll.
    $('body').scrollspy({
        target: '#mainNav',
        offset: 80
    });
    
    // Closes responsive menu when a scroll trigger link is clicked.
    $('.js-scroll-trigger').click(function() {
        $('.navbar-collapse').collapse('hide');
    });

    // Scroll to top BTN appear.
    $(document).scroll(function() {
        var scrollDistance = $(this).scrollTop();
        if (scrollDistance > 100) {
            $('.scroll-to-top').fadeIn();
        } else {
            $('.scroll-to-top').fadeOut();
        }
    });
    
    // Collapse navbar.
    var navbarCollapse = function() {
        if ($("#mainNav").offset().top > 100) {
            $("#mainNav").addClass("navbar-shrink");
        } else {
            $("#mainNav").removeClass("navbar-shrink");
        }
    };
    
    // Collapse if page is not at top.
    navbarCollapse();
    
    // Collapse the navbar when page is scrolled.
    $(window).scroll(navbarCollapse);

})(jQuery); // End of use strict.
