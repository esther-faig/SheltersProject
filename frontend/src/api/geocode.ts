// המרת כתובת לקואורדינטות (OpenStreetMap, בלי מפתח)
export async function geocodeAddress(address: string): Promise<{ lat: number; lng: number } | null> {
  const url =
    `https://nominatim.openstreetmap.org/search?q=${encodeURIComponent(address)}` +
    `&format=jsonv2&limit=1&countrycodes=il&accept-language=he`;
  try {
    const res = await fetch(url);
    if (!res.ok) throw new Error();
    const list = (await res.json()) as { lat: string; lon: string }[];
    if (list.length === 0) return null;
    return { lat: parseFloat(list[0].lat), lng: parseFloat(list[0].lon) };
  } catch {
    throw new Error('שירות איתור הכתובות אינו זמין כרגע. נסו שוב בעוד רגע');
  }
}