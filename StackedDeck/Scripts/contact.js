function initMap() {
	var myLatLng = {lat: 43.6562112, lng: -79.3810105};

	var map = new google.maps.Map(document.getElementById('map'), {
		center: myLatLng,
		zoom: 15
	});

	var marker = new google.maps.Marker({
		position: myLatLng,
		map: map
	});
}