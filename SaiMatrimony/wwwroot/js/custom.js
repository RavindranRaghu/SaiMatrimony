(function ($) {

  "use strict";

    // PRE LOADER
    $(window).load(function(){
      $('.preloader').fadeOut(1000); // set duration in brackets    
    });
    
    $("#search-html").html(loadProfile());    

    $(document).on('click', ".search-main",function () {

        var isOpened = $(this).hasClass("opened");
        var searchid = $(this).data("searchid");
        var obj = $("#search-icon-"+searchid);

        if (isOpened) {
            $("#search-detail-" + searchid).hide();
            if (obj.hasClass("fa-chevron-up")) {
                obj.addClass("fa-chevron-down");
                obj.removeClass("fa-chevron-up");
                $(this).removeClass("opened");
            }
        }
        else {
            $("#search-detail-" + searchid).show();
            if (obj.hasClass("fa-chevron-down")) {
                obj.addClass("fa-chevron-up")
                obj.removeClass("fa-chevron-down")
                $(this).addClass("opened");
            }
        }
    });

    $(document).on('keyup', ".education", function () {
        loadProfile();
    });

    $(document).on('keyup', ".location", function () {
        loadProfile();
    });

    $(document).on('keyup', ".profession", function () {
        loadProfile();
    });

    $(document).on('change', ".gender", function () {
        loadProfile();
    });

    $(".search-btn").click(function () {
        var current = $(this);
        $(".search-btn").each(function (item) {
            if ($(this).hasClass("badge")) {
                $(this).removeClass("badge");
            }
        })
        current.addClass("badge");
        $("#category").val($(this).data("category"));
        $("#search-html").html(loadProfile());
    });



    // MENU
    $('.navbar-collapse a').on('click',function(){
      $(".navbar-collapse").collapse('hide');
    });

    $(window).scroll(function() {
      if ($(".navbar").offset().top > 50) {
        $(".navbar-fixed-top").addClass("top-nav-collapse");
          } else {
            $(".navbar-fixed-top").removeClass("top-nav-collapse");
          }
    });
    

    // PARALLAX EFFECT
    $.stellar({
      horizontalScrolling: false,
    }); 


    // MAGNIFIC POPUP
    $('.image-popup').magnificPopup({
        type: 'image',
        removalDelay: 300,
        mainClass: 'mfp-with-zoom',
        gallery:{
          enabled:true
        },
        zoom: {
        enabled: true, // By default it's false, so don't forget to enable it

        duration: 300, // duration of the effect, in milliseconds
        easing: 'ease-in-out', // CSS transition easing function

        // The "opener" function should return the element from which popup will be zoomed in
        // and to which popup will be scaled down
        // By defailt it looks for an image tag:
        opener: function(openerElement) {
        // openerElement is the element on which popup was initialized, in this case its <a> tag
        // you don't need to add "opener" option if this code matches your needs, it's defailt one.
        return openerElement.is('img') ? openerElement : openerElement.find('img');
        }
      }
    });


    // SMOOTH SCROLL
    $(function() {
      $('.custom-navbar a, #home a').on('click', function(event) {
        var $anchor = $(this);
          $('html, body').stop().animate({
            scrollTop: $($anchor.attr('href')).offset().top - 49
          }, 1000);
            event.preventDefault();
      });
    });  


    function loadProfile() {
        var edu = $(".education:first").val();
        var pro = $(".profession:first").val();
        var location = $(".location:first").val();
        var gen = $(".gender:first").val();
        var category = $("#category").val();

        var url = "/home/getprofiles?edu=" + edu + "&pro=" + pro + "&location=" + location + "&gen=" + gen + "&category=" + category;        
        $.ajax({
            url: url,
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (result) {

                $('#actualflow-pagination').pagination({
                    dataSource: result,
                    pageSize: 10,
                    showGoInput: true,
                    showGoButton: true,
                    callback: function (data, pagination) {
                        var html = '';
                        data.forEach(function (item) {
                            html += searchHtmlForProfile(item)
                        });                        
                        $("#actualflow-pagination").prev().html(html);
                        //$("#loading").hide();
                        $(".search-detail").hide();
                    }
                });
            },
            error: function (error) {
                console.log(error);
            }
        });
    }

    function searchHtmlForProfile(profile) {
        var searchHtml = '<div class="search-items">';
        searchHtml += '<div class="search-main" data-searchid="' + profile.profileId+'">';
        searchHtml += '<div class="row">';
        searchHtml += '<div class="col-md-4">';
        searchHtml += '<span class="search-main-title">Name: </span>';
        searchHtml += '<span class="search-main-text">' + profile.firstName + " " + profile.lastName + '</span>';
        searchHtml += '</div>';
        searchHtml += '<div class="col-md-3">';
        searchHtml += '<span class="search-main-title">Year Born: </span>';
        searchHtml += '<span class="search-main-text">' + profile.yearOfBirth +'</span>';
        searchHtml += '</div>';
        searchHtml += '<div class="col-md-4">';
        searchHtml += '<span class="search-main-title">Profession: </span>';
        searchHtml += '<span class="search-main-text">' + profile.profession +'</span>';
        searchHtml += '</div>';
        searchHtml += '<div class="col-md-1">';
        searchHtml += '<span class="search-icon fa fa-chevron-down" id="search-icon-' + profile.profileId +'"></span>';
        searchHtml += '</div>';
        searchHtml += '</div>';
        searchHtml += '</div>';
        searchHtml += '<div class="media blog-thumb search-detail" id="search-detail-' + profile.profileId +'">';
        searchHtml += '<div class="media-object media-left">';
        searchHtml += '<a href="blog-detail.html"><img src="/images/avatar.png" class="img-responsive avatar-img" alt=""></a>';
        searchHtml += '</div>';
        searchHtml += '<div class="media-body blog-info">';
        searchHtml += '<small><i class="fa fa-clock-o"></i> December 22, 2017</small>';
        searchHtml += '<h3><a href="blog-detail.html">' + profile.interest +'</a></h3>';
        searchHtml += '<p>' + profile.expectation +'</p>';
        searchHtml += '<a href="blog-detail.html" class="btn section-btn">Propose Match</a>';
        searchHtml += '</div>';
        searchHtml += '</div>';
        searchHtml += '</div>';
        return searchHtml;
    }

})(jQuery);
