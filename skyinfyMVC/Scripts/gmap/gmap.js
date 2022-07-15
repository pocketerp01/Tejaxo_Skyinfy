
//Define a variable with all map points.
//Define a DirectionsRenderer variable.

var geocoder = new google.maps.Geocoder();

function geocodePosition(pos) {
    geocoder.geocode({
        latLng: pos
    }, function (responses) {
        if (responses && responses.length > 0) {
            updateMarkerAddress(responses[0].formatted_address);
        } else {
            updateMarkerAddress('Cannot determine address at this location.');
        }
    });
}

function updateMarkerStatus(str) {
    //document.getElementById('markerStatus').innerHTML = str;
}

function updateMarkerPosition(latLng) {
    document.getElementById('txtlat').value = [
        latLng.lat(),
        latLng.lng()
    ].join(', ');

    //    <% -- var myHidden = document.getElementById('<%= txtlat.ClientID %>');

    //if (myHidden)//checking whether it is found on DOM, but not necessary
    //{
    //    myHidden.value = latLng.value;
    //} --%>
    //<% --var myHidden1 = document.getElementById('<%= hflng.ClientID %>');

    //if (myHidden1)//checking whether it is found on DOM, but not necessary
    //{
    //    myHidden1.value = latLng.lng();
    //} --%>
}

function updateMarkerAddress(str) {
    document.getElementById('txtaddr1').value = str;
}

function geolocationError(positionError) {

}
var _mapPoints = new Array();
var _directionsRenderer = '';
var map, marker = new google.maps.Marker();
directionsService = new google.maps.DirectionsService;
function initialize(lat, long) {
    debugger
    var lat = lat;
    var long = long;
    //  var city = position.coords.locality;
    var latLng = new google.maps.LatLng(lat, long);

    //create map Options
    var mapOptions = {
        center: latLng,
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    //end Map option
    map = new google.maps.Map(document.getElementById('mapCanvas'), mapOptions);

    //set services


    this.directionsDisplay = new google.maps.DirectionsRenderer;
    this.directionsDisplay.setMap(map);
    _directionsRenderer = new google.maps.DirectionsRenderer();

    //Set the map for directionsRenderer
    _directionsRenderer.setMap(map);
    //Set different options for DirectionsRenderer mehtods.
    //draggable option will used to drag the route.
    _directionsRenderer.setOptions({
        draggable: true
    });

    // Update current position info.
    //updateMarkerPosition(latLng);
    //geocodePosition(latLng);
    //snapToRoad(latLng);

    //Search Feature Started
    var input = document.getElementById('txtsearch');
    var autocomplete = new google.maps.places.Autocomplete(input);
    autocomplete.bindTo('bounds', map);
    var infowindow = new google.maps.InfoWindow();

    google.maps.event.addListener(autocomplete, 'place_changed', function () {
        infowindow.close();
        marker.setVisible(false);
        var place = autocomplete.getPlace();
        if (!place.geometry) {
            return;
        }

        // If the place has a geometry, then present it on a map.
        if (place.geometry.viewport) {
            map.fitBounds(place.geometry.viewport);
        } else {
            map.setCenter(place.geometry.location);
            map.setZoom(15); // Why 17? Because it looks good.
        }
        //updateMarkerPosition(place.geometry.location);
        //geocodePosition(place.geometry.location);
        snapToRoad(place.geometry.location);
        //fx(new google.maps.LatLng(place.geometry.location, place.geometry.location));

    });
}

function snapToRoad(latLng) {

    var image = new google.maps.MarkerImage(
        'http://maps.google.com/mapfiles/ms/micons/green-dot.png',
        new google.maps.Size(32, 32),   // size
        new google.maps.Point(0, 0), // origin
        new google.maps.Point(16, 32)   // anchor
    );

    var shadow = new google.maps.MarkerImage(
        'http://maps.google.com/mapfiles/ms/micons/msmarker.shadow.png',
        new google.maps.Size(59, 32),   // size
        new google.maps.Point(0, 0), // origin
        new google.maps.Point(16, 32)   // anchor
    );


    directionsService.route({ origin: latLng, destination: latLng, travelMode: google.maps.DirectionsTravelMode.DRIVING }, function (response, status) {
        ;
        if (status == google.maps.DirectionsStatus.OK) {
            marker = new google.maps.Marker({
                position: response.routes[0].legs[0].start_location,
                map: map,
                title: "Check this cool location",
                icon: image,
                draggable: true,
                shadow: shadow
            });


        } else {
            marker = new google.maps.Marker({
                position: response.routes[0].legs[0].start_location,
                map: map,
                title: "Check this cool location",
                icon: image,
                draggable: true,
                shadow: shadow,

            });

        }
        updateMarkerPosition(response.routes[0].legs[0].start_location);
        geocodePosition(response.routes[0].legs[0].start_location);

        google.maps.event.addListener(marker, 'dragstart', function () {
            updateMarkerAddress('Dragging...');
            //snapToRoad(marker.getPosition());
        });

        google.maps.event.addListener(marker, 'drag', function () {
            updateMarkerStatus('Dragging...');
            updateMarkerPosition(marker.getPosition());
            geocodePosition(marker.getPosition());
            //snapToRoad(marker.getPosition());
        });

        google.maps.event.addListener(marker, 'dragend', function () {
            updateMarkerStatus('Drag ended');
            geocodePosition(marker.getPosition());
            marker.setMap(null);
            snapToRoad(marker.getPosition());

        });
    });
}
function AutocompleteDirectionsHandler(map) {

    this.map = map;
    this.originPlaceId = null;
    this.destinationPlaceId = null;
    this.travelMode = 'DRIVING';
    var originInput = document.getElementById('txtsearch');
    //var destinationInput = document.getElementById('destination-input');
    //var modeSelector = document.getElementById('mode-selector');
    this.directionsService = new google.maps.DirectionsService;
    this.directionsDisplay = new google.maps.DirectionsRenderer;
    this.directionsDisplay.setMap(map);

    var originAutocomplete = new google.maps.places.Autocomplete(
        originInput, { placeIdOnly: true });
    //var destinationAutocomplete = new google.maps.places.Autocomplete(
    //    destinationInput, { placeIdOnly: true });

    //this.setupClickListener('changemode-walking', 'WALKING');
    //this.setupClickListener('changemode-transit', 'TRANSIT');
    //this.setupClickListener('changemode-driving', 'DRIVING');

    this.setupPlaceChangedListener(originAutocomplete, 'ORIG');
    //this.setupPlaceChangedListener(destinationAutocomplete, 'DEST');

    //this.map.controls[google.maps.ControlPosition.TOP_LEFT].push(originInput);
    //this.map.controls[google.maps.ControlPosition.TOP_LEFT].push(destinationInput);
    //this.map.controls[google.maps.ControlPosition.TOP_LEFT].push(modeSelector);
}

AutocompleteDirectionsHandler.prototype.setupPlaceChangedListener = function (autocomplete, mode) {
    ;
    var me = this;
    autocomplete.bindTo('bounds', this.map);
    autocomplete.addListener('place_changed', function () {
        var place = autocomplete.getPlace();
        if (!place.place_id) {
            window.alert("Please select an option from the dropdown list.");
            return;
        }
        if (mode === 'ORIG') {
            me.originPlaceId = place.place_id;
        } else {
            me.destinationPlaceId = place.place_id;
        }
        me.route();

    });

};

AutocompleteDirectionsHandler.prototype.route = function () {
    ;
    if (!this.originPlaceId || !this.destinationPlaceId) {
        return;
    }
    var me = this;

    this.directionsService.route({
        origin: { 'placeId': this.originPlaceId },
        destination: { 'placeId': this.destinationPlaceId },
        travelMode: this.travelMode
    }, function (response, status) {
        if (status === 'OK') {
            me.directionsDisplay.setDirections(response);
        } else {
            window.alert('Directions request failed due to ' + status);
        }
    });
};

function drawRoute(originAddress, destinationAddress, _waypoints) {
    //Define a request variable for route .
    var _request = '';
    ;
    //This is for more then two locatins
    if (_waypoints.length > 0) {
        _request = {
            origin: originAddress,
            destination: destinationAddress,
            waypoints: _waypoints, //an array of waypoints
            optimizeWaypoints: true, //set to true if you want google to determine the shortest route or false to use the order specified.
            travelMode: google.maps.DirectionsTravelMode.DRIVING
        };
    } else {
        //This is for one or two locations. Here noway point is used.
        _request = {
            origin: originAddress,
            destination: destinationAddress,
            travelMode: google.maps.DirectionsTravelMode.DRIVING
        };
    }

    //This will take the request and draw the route and return response and status as output
    directionsService.route(_request, function (_response, _status) {
        if (_status == google.maps.DirectionsStatus.OK) {
            _directionsRenderer.setDirections(_response);
        }
    });
}
    // Onload handler to fire off the app.
    // google.maps.event.addDomListener(window, 'load', initialize);