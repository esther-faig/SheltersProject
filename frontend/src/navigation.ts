export type TravelMode = 'walk' | 'car';

// Google Maps: תומך בהליכה וברכב
export function googleMapsUrl(lat: number, lng: number, mode: TravelMode) {
  const travelmode = mode === 'car' ? 'driving' : 'walking';
  return `https://www.google.com/maps/dir/?api=1&destination=${lat},${lng}&travelmode=${travelmode}`;
}

// Waze: ניווט לרכב בלבד (אין בו מצב הליכה)
export function wazeUrl(lat: number, lng: number) {
  return `https://waze.com/ul?ll=${lat},${lng}&navigate=yes`;
}