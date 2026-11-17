export type TravelMode = 'walk' | 'car';

// Google Maps: תומך בהליכה וברכב
export type LatLng = { lat: number; lng: number };

export function googleMapsUrl(lat: number, lng: number, mode: TravelMode, origin?: LatLng | null) {
  const travelmode = mode === 'car' ? 'driving' : 'walking';
  const from = origin ? `&origin=${origin.lat},${origin.lng}` : '';
  return `https://www.google.com/maps/dir/?api=1${from}&destination=${lat},${lng}&travelmode=${travelmode}`;
}

// Waze: ניווט לרכב בלבד (אין בו מצב הליכה)
export function wazeUrl(lat: number, lng: number) {
  return `https://waze.com/ul?ll=${lat},${lng}&navigate=yes`;
}