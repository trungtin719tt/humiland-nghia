var map;
var markers = [];
function initAutocomplete() {
    var lat = /*document.getElementById("index-lat").nodeValue*/ -33.8688;
    var long = /*document.getElementById("index-long").nodeValue*/ 151.2195;
    map = new google.maps.Map(document.getElementById('map_canvas'), {
        center: { lat:  lat, lng: long },
        zoom: 13,
        mapTypeId: 'roadmap'
    });
    // Create the search box and link it to the UI element.
    //var input = document.getElementById('pac-input');
    //var searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
    var uluru = { lat: lat, lng: long };
    addMarker(uluru, map);
    // Bias the SearchBox results towards current map's viewport.
    //map.addListener('bounds_changed', function () {
    //    searchBox.setBounds(map.getBounds());
    //});

    google.maps.event.addListener(map, 'click', function (event) {
        deleteMarkers();
        addMarker(event.latLng);
    });
    // var markers = [];

    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    //searchBox.addListener('places_changed', function () {
    //    var places = searchBox.getPlaces();

    //    if (places.length == 0) {
    //        return;
    //    }
        // Clear out the old markers.
        // markers.forEach(function(marker) {
        //     marker.setMap(null);
        // });
        // markers = [];


        // For each place, get the icon, name and location.
        //var bounds = new google.maps.LatLngBounds();
        //places.forEach(function (place) {
        //    if (!place.geometry) {
        //        console.log("Returned place contains no geometry");
        //        return;
        //    }
        //    var icon = {
        //        url: place.icon,
        //        size: new google.maps.Size(71, 71),
        //        origin: new google.maps.Point(0, 0),
        //        anchor: new google.maps.Point(17, 34),
        //        scaledSize: new google.maps.Size(25, 25)
        //    };

        //    // Create a marker for each place.
        //    markers.push(new google.maps.Marker({
        //        map: map,
        //        icon: icon,
        //        title: place.name,
        //        position: place.geometry.location
        //    }));

        //    if (place.geometry.viewport) {
        //        // Only geocodes have viewport.
        //        bounds.union(place.geometry.viewport);
        //    } else {
        //        bounds.extend(place.geometry.location);
        //    }
        //});
        map.fitBounds(bounds);
    //});
}
// function addMarker(location, map) {
//     // Add the marker at the clicked location, and add the next-available label
//     // from the array of alphabetical characters.
//     var marker = new google.maps.Marker({
//         position: location,
//         label: labels[labelIndex++ % labels.length],
//         map: map
//     });
// }
function addMarker(location) {
    var marker = new google.maps.Marker({
        position: location,
        map: map
    });
    markers.push(marker);
    // alert(marker.getPosition().lat() + "\n" + marker.getPosition().lng());
}
// Sets the map on all markers in the array.
function setMapOnAll(map) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}

// Removes the markers from the map, but keeps them in the array.
function clearMarkers() {
    setMapOnAll(null);
}

// Shows any markers currently in the array.
function showMarkers() {
    setMapOnAll(map);
}

// Deletes all markers in the array by removing references to them.
function deleteMarkers() {
    clearMarkers();
    markers = [];
}