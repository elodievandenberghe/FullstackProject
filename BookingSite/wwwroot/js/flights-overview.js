// This file can be included in your site.js or as a separate file

// Flight filter enhancement
function enhanceFlightFilters() {
    const fromAirport = document.getElementById('fromAirportId');
    const toAirport = document.getElementById('toAirportId');
    const departureDate = document.getElementById('departureDate');

    // Update available destination airports based on selected departure airport
    fromAirport.addEventListener('change', async function () {
        if (!fromAirport.value) return;

        try {
            const response = await fetch(`/api/vluchten/${fromAirport.value}`);
            if (response.ok) {
                const flights = await response.json();

                // Get unique destination airports from flights
                const destinations = [...new Set(flights.map(f => ({ id: f.toAirportId, name: f.toAirport })))];

                // Update destination dropdown
                toAirport.innerHTML = '<option value="">-- All Arrival Airports --</option>';
                destinations.forEach(dest => {
                    const option = document.createElement('option');
                    option.value = dest.id;
                    option.textContent = dest.name;
                    toAirport.appendChild(option);
                });
            }
        } catch (error) {
            console.error('Error loading destination airports:', error);
        }
    });

    // Add datepicker enhancement if not using HTML5 date inputs
    if (departureDate.type !== 'date') {
        // If browser doesn't support date inputs, you might want to add a date picker library here
    }
}

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function () {
    enhanceFlightFilters();
});
