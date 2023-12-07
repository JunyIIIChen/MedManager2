let map;
let infoWindow;
let directionsService;
let directionsDisplay;

async function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: -37.9101, lng: 145.1353 },
        zoom: 13,
    });

    infoWindow = new google.maps.InfoWindow();

    directionsService = new google.maps.DirectionsService();
    directionsDisplay = new google.maps.DirectionsRenderer();
    directionsDisplay.setMap(map);
    directionsDisplay.setPanel(document.getElementById('directionsPanel'));

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            position => {
                const pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude,
                };
                map.setCenter(pos);
            },
            () => {
                handleLocationError(true, map.getCenter());
            }
        );
    } else {
        handleLocationError(false, map.getCenter());
    }

    // use fectch to get api
    fetch("Clinics/GetClinics")
        .then(response => response.json())
        .then(clinics => {
            for (let clinic of clinics) {
                geocodeAddress(map, clinic);
            }
        })
        .catch(error => {
            console.error("Error fetching clinics:", error);
        });
}

function handleLocationError(browserHasGeolocation, pos) {
    infoWindow.setPosition(pos);
    infoWindow.setContent(
        browserHasGeolocation
            ? "Error: The Geolocation service failed."
            : "Error: Your browser doesn't support geolocation."
    );
    infoWindow.open(map);
}

function geocodeAddress(map, clinic) {
    const geocoder = new google.maps.Geocoder();

    geocoder.geocode({ 'address': clinic.ClinicAddress }, (results, status) => {
        if (status === google.maps.GeocoderStatus.OK) {
            const marker = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location
            });

            google.maps.event.addListener(marker, 'click', function () {
                const infoContent = `
                    <div>
                        <strong>${clinic.ClinicName}</strong><br>
                        Address: ${clinic.ClinicAddress}<br>
                        Phone: ${clinic.ClinicPhone}<br>
                        Description: ${clinic.ClinicDescription}
                    </div>
                `;

                infoWindow.setContent(infoContent);
                infoWindow.open(map, marker);
            });
        } else {
            console.error('Geocode was not successful for the following reason:', status);
        }
    });
}

function calculateAndDisplayRoute() {
    const start = document.getElementById('start').value;
    const end = document.getElementById('end').value;

    directionsService.route({
        origin: start,
        destination: end,
        travelMode: 'DRIVING'
    }, (response, status) => {
        if (status === 'OK') {
            directionsDisplay.setDirections(response);
        } else {
            window.alert('Directions request failed due to ' + status);
        }
    });
}
