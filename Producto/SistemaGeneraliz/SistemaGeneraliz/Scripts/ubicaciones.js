var map;
var marker;
var markersArray = [];

function initialize() {
    var mapOptions = {
        zoom: 16,
        scaleControl: false,
        navigationControl: false,
        streetViewControl: false,
        zoomControl: false,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);

    
    //if ($("#ubicaciones").val() != "") {
    //    alert($("#ubicaciones").val());    
    //    //var ubicacionesJson = JSON.parse($("#ubicaciones").val());
    //}

    //var lat = $("#Latitud").val();
    //var lon = $("#Longitud").val();
    var pos = new google.maps.LatLng(-12.08611459617003, -77.00229406356812);
    map.setCenter(pos);
    marker = new google.maps.Marker({
        position: pos,
        map: map,
        animation: google.maps.Animation.DROP,
        //icon: new google.maps.MarkerImage("http://google-maps-icons.googlecode.com/files/administration.png", null, null, new google.maps.Point(8, 8))
    });

    markersArray.push(marker);

}

function addInfoWindow(marker, message) {
    var info = message;

    var infoWindow = new google.maps.InfoWindow({
        content: message
    });

    google.maps.event.addListener(marker, 'click', function () {
        infoWindow.open(map, marker);
    });
}

function clearOverlays() {
    for (var i = 0; i < markersArray.length; i++) {
        markersArray[i].setMap(null);
    }
    markersArray = [];
}

google.maps.event.addDomListener(window, 'load', initialize);
