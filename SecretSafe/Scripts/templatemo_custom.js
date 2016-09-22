"use strict";

jQuery(document).ready(function($){

	/************** Menu Content Opening *********************/

    

	/************** Gallery Hover Effect *********************/
	$(".overlay").hide();

	$('.gallery-item').hover(
	  function() {
	    $(this).find('.overlay').fadeIn(800);
	  },
	  function() {
	    $(this).find('.overlay').fadeOut(800);
	  }
	);


	/************** LightBox *********************/
	$(function(){
		$('[data-rel="lightbox"]').lightbox();
	});


	$("a.menu-toggle-btn").click(function() {
	  $(".responsive_menu").stop(true,true).slideToggle();
	  return false;
	});

    $(".responsive_menu a").click(function(){
		$('.responsive_menu').hide();
	});

});
