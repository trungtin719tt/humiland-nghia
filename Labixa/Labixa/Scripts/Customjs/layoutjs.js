$(document).ready(function () {       
        var h = $('.vid-box').height();
        $('.thumb-hover').css('height', h);

        var h2 = $('.thumb-1').width()+20;
        $('.sn').css('margin-left', h2);

        

        $(".khoahowcsroll").slick({
            slidesToShow: 3,
            slidesToScroll: 1,
            infinite: true,
            responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 769,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            ]
        });
        $(".teamslick").slick({
            slidesToShow: 4,
            slidesToScroll: 1,
            infinite: true,
            responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 769,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            },
            ]
        });

        $(".hocvienslick").slick({
            slidesToShow: 4,
            slidesToScroll: 1,
            infinite: true,
            responsive: [
                {
                    breakpoint: 1180,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 1
                    }
                },
            {
                breakpoint: 769,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            },
            ]
        });
        
});


    $(document).ready(function () {
        $('#menu-homes').click(function () {
            $.scrollTo($('#homes'), 1000);
        });
        $('#menu-about').click(function () {
            $.scrollTo($('#about'), 1000);
        });
        $('#menu-courses').click(function () {
            $.scrollTo($('#courses'), 1000);
        });
        $('#menu-news').click(function () {
            $.scrollTo($('#news'), 1000);
        });
        $('#menu-work').click(function () {
            $.scrollTo($('#work'), 1000);
        });
        $('#menu-contacts').click(function () {
            $.scrollTo($('#contacts'), 1000);
        });
       
        var listspan4 = $(".span4");
        var maxh = 0;
        for (var i = 0; i < listspan4.length; i++) {
            if (maxh < listspan4[i].offsetHeight) {
                maxh = listspan4[i].offsetHeight;
            }
        }
        $(".row-fluid .span4").css("min-height", maxh);
    });
    
