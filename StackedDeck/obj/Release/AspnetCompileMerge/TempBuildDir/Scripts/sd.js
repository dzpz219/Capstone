
$(document).ready(function() {
	var featureImage = $('.featureImage').waypoint(function() {
	  $('.featureImage').addClass('showUp');
	})

	var leftRightFade = $('.heading-left').waypoint(function() {
	  $('.heading-left').addClass('showUp');
	  $('.heading-right').addClass('showUp');
	}, {
		offset: '75%'
	})

	var description = $('.description').waypoint(function() {
	  $('.description').addClass('showUp');
	}, {
		offset: '75%'
	})

	var fadeHeadingFirst = $('.first-heading').waypoint(function() {
	  $('.first-heading').addClass('showUp');
	}, {
		offset: '75%'
	})

	var fadeHeadingSecond = $('.second-heading').waypoint(function() {
	  $('.second-heading').addClass('showUp');
	}, {
		offset: '75%'
	})

	var fadeHeadingThird = $('.third-heading').waypoint(function() {
	  $('.third-heading').addClass('showUp');
	}, {
		offset: '75%'
	})

});