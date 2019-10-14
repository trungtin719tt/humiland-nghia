jQuery(document).ready(function($) {

	/*-----------------------------------------------------------------------------------*/
	/*	SLIDER
	/*-----------------------------------------------------------------------------------*/

	//if ($.fn.cssOriginal != undefined) $.fn.css = $.fn.cssOriginal;

	$('.banner').revolution({
		delay : 9000,
		startheight : 496,
		startwidth : 1035,

		navigationType : "none", //bullet, thumb, none, both		(No Thumbs In FullWidth Version !)
		navigationArrows : "verticalcentered", //nexttobullets, verticalcentered, none
		navigationStyle : "round", //round,square,navbar

		touchenabled : "on", // Enable Swipe Function : on/off
		onHoverStop : "on", // Stop Banner Timet at Hover on Slide on/off

		hideThumbs : 200,

		navOffsetHorizontal : 0,
		navOffsetVertical : -35,

		stopAtSlide : -1, // Stop Timer if Slide "x" has been Reached. If stopAfterLoops set to 0, then it stops already in the first Loop at slide X which defined. -1 means do not stop at any slide. stopAfterLoops has no sinn in this case.
		stopAfterLoops : -1, // Stop Timer if All slides has been played "x" times. IT will stop at THe slide which is defined via stopAtSlide:x, if set to -1 slide never stop automatic

		shadow : 0, //0 = no Shadow, 1,2,3 = 3 Different Art of Shadows  (No Shadow in Fullwidth Version !)
		fullWidth : "off", // Turns On or Off the Fullwidth Image Centering in FullWidth Modus

	});

	//=================================== styled selects =================================//
	$('select').selectpicker();

	//=================================== Olaceholder ie =================================//
	$('input, textarea').placeholder();

	//=================================== Tooltips =====================================//

	if ($.fn.tooltip()) {
		$('[class="tooltip_hover"]').tooltip();
		$('.port-twitter a, .port-facebook a').tooltip({
			placement : 'right'
		});

	}
	//=================================== Gallery =================================//

	//simplemodal
	$('.modal').click(function(e) {
		var href = $(this).attr('href');
		$(href).modal();
		return false;
	});

	$('#ateam').click(function() {//Id del elemento cliqueable
		$('html, body').animate({
			scrollTop : 0
		}, 'slow');
		return false;
	});

	//scroll
	var defaults = {
		containerID : 'moccaUItoTop',
		containerHoverClass : 'moccaUIhover',
		scrollSpeed : 1150,
		easingType : 'linear'
	};

	$().UItoTop();

	$('nav li a').on('click', function(e) {
		//prevenir en comportamiento predeterminado del enlace
		e.preventDefault();
		//obtenemos el id del elemento en el que debemos posicionarnos
		var strAncla = $(this).attr('href');

		//utilizamos body y html, ya que dependiendo del navegador uno u otro no funciona
		$('body,html').stop(true, true).animate({
			//realizamos la animacion hacia el ancla
			scrollTop : $(strAncla).offset().top
		}, 1000);
	});

	//inputs auto fill
	$('input[type="text"], textarea').each(function() {
		var currentValue = $(this).val();
		$(this).focus(function() {
			if ($(this).val() == currentValue) {
				$(this).val('');
			};
		});
		$(this).blur(function() {
			if ($(this).val() == '') {
				$(this).val(currentValue);
			};
		});
	});

	//scroll porfolio

	function portfolioInit() {
		var cantItems = $('#grid .view').length;
		var itemWidth = $('#grid .view').width();
		var contentWidth = (itemWidth + 20) * cantItems;
		//alert(contentWidth)
		$('.portfolio .scroll-content').width(contentWidth);
		$('.scroll-pane').jScrollPane({
			horizontalDragMaxWidth : 65
		});
		$('#grid .view').css("overflow", "visible");
	}

	portfolioInit()
	$(window).resize(function() {
		portfolioInit()
	});

	//drag tooltip
	$('.jspDrag').hover(function() {
		$(document).on('mousemove', '.jspDrag', function() {
			position = $(this).position();
			$('.tooltipDrag').css("left", +position.left + "px");
		});
		$('.tooltipDrag').fadeIn();
	}, function() {
		$('.tooltipDrag').hide();
	});

	//client list
	$(window).load(function() {
		$('.genericSection .clients').each(function() {
			var totalWidth = 0;
			$(this).children("ul").children("li").children("a").each(function() {
				totalWidth = totalWidth + 30 + $(this).width();
			});
			$(this).width(totalWidth);
		});
		$('.clients-scroll').jScrollPane({
			showArrows : 1,
			speed : 100,
			horizontalGutter : 25,
			trackClickRepeatFreq : 350
		});
	});
	$(window).resize(function() {
		$('.clients-scroll').jScrollPane({
			showArrows : 1,
			speed : 100,
			horizontalGutter : 25,
			trackClickRepeatFreq : 350
		});
	});
	
	//=================================== Fancybox Lightbox =====================================//
	
	$("a.bigimages").fancybox({
		'transitionIn'	:	'elastic',
		'transitionOut'	:	'elastic',
		'speedIn'		:	600, 
		'speedOut'		:	200, 
		'overlayShow'	:	false
	});

	//=================================== form forms =================================//
	$("#contact-form").submit(function() {
		var elem = $(this);
		var urlTarget = $(this).attr("action");
		$.ajax({
			type : "POST",
			url : urlTarget,
			dataType : "html",
			data : $(this).serialize(),
			beforeSend : function() {
				elem.prepend("<div class='loading alert'>" + "<a class='close' data-dismiss='alert'>×</a>" + "Loading" + "</div>");
				//elem.find(".loading").show();
			},
			success : function(response) {
				elem.prepend(response);
				//elem.find(".response").html(response);
				elem.find(".loading").hide();
				elem.find("input[type='text'],input[type='email'],textarea").val("");
			}
		})
		return false;
	});

	//overview
	$('.over').hover(function() {
		$(this).children('.overview').css('visibility', 'visible');
	}, function() {
		$(this).children('.overview').css('visibility', 'hidden');
	});
	$('.over-hex').hover(function() {
		$(this).children('.overview').css('visibility', 'visible');
		$(this).children('.over-content').css('visibility', 'visible');
	}, function() {
		$(this).children('.overview').css('visibility', 'hidden');
		$(this).children('.over-content').css('visibility', 'hidden');
	});

	//flickr
	$('#flickr').jflickrfeed({
		limit : 6,
		qstrings : {
			id : '44802888@N04'
		},
		itemTemplate : '<li class="ifour box">' + '<a href="{{image_b}}"></a>' + '<img src="{{image_s}}" alt="{{title}}" /></a>' + '</li>'
	});

	$('.logos').hoverizr();

	//map
	$(window).load(function() {
		var latlng = new google.maps.LatLng(43.362613105917106, -1.7916297912597656);
		var myOptions = {
			zoom : 15,
			center : latlng,
			mapTypeId : google.maps.MapTypeId.ROADMAP
		};
		var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
		var marcador = new google.maps.Marker({
			position : new google.maps.LatLng(43.362613105917106, -1.7916297912597656),
			map : map,
			title : 'Ubicación',
		});
	});

});
