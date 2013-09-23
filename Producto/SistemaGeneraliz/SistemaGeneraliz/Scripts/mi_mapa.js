var map;
var marker;

function initialize() {
    var mapOptions = {
        zoom: 12,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);

    google.maps.event.addListener(map, 'click', function (event) {
        placeMarker(event.latLng);
    });

    // Try HTML5 geolocation
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = new google.maps.LatLng(position.coords.latitude,
                                             position.coords.longitude);

            //var infowindow = new google.maps.InfoWindow({
            //    map: map,
            //    position: pos,
            //    content: 'Su ubicación.'
            //});

            map.setCenter(pos);

            marker = new google.maps.Marker({
                position: pos,
                map: map,
                animation: google.maps.Animation.DROP,
                //icon: new google.maps.MarkerImage("http://google-maps-icons.googlecode.com/files/administration.png", null, null, new google.maps.Point(8, 8))
            });
        }, function () {
            handleNoGeolocation(true);
        });
    } else {
        // Browser doesn't support Geolocation
        handleNoGeolocation(false);
    }
}

function placeMarker(location) {
    //alert(location);
    //marker.setPosition(location);

    if (marker) {
        marker.setPosition(location);
    } else {
        marker = new google.maps.Marker({
            position: location,
            map: map,
            animation: google.maps.Animation.DROP,
            //icon: new google.maps.MarkerImage("http://google-maps-icons.googlecode.com/files/administration.png", null, null, new google.maps.Point(8, 8))
        });
    }
    var loc = location.toString();
    var lat = loc.split(",")[0];
    var lng = loc.split(",")[1];

    lat = lat.substring(1, lat.length);
    lng = lng.substring(0, lat.length - 1);
    $("#Latitud").val(lat);
    $("#Longitud").val(lng);
    //alert("a: " + $("#Latitud").val() + " " + $("#Longitud").val());
}

function handleNoGeolocation(errorFlag) {
    if (errorFlag) {
        var content = 'Error: The Geolocation service failed.';
    } else {
        var content = 'Error: Your browser doesn\'t support geolocation.';
    }

    var options = {
        map: map,
        position: new google.maps.LatLng(-12.08611459617003, -77.00229406356812),
        content: content
    };

    var infowindow = new google.maps.InfoWindow(options);
    map.setCenter(options.position);
    marker = new google.maps.Marker({
        position: new google.maps.LatLng(-12.08611459617003, -77.00229406356812),
        map: map,
        animation: google.maps.Animation.DROP,
        //icon: new google.maps.MarkerImage("http://google-maps-icons.googlecode.com/files/administration.png", null, null, new google.maps.Point(8, 8))
    });
    $("#Latitud").val("-12.08611459617003");
    $("#Longitud").val("-77.00229406356812");
}


google.maps.event.addDomListener(window, 'load', initialize);
