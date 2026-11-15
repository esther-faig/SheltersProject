import { useEffect } from 'react';
import {
  MapContainer,
  TileLayer,
  Marker,
  Popup,
  useMap,
  useMapEvents,
} from 'react-leaflet';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import markerIcon from 'leaflet/dist/images/marker-icon.png';
import markerIcon2x from 'leaflet/dist/images/marker-icon-2x.png';
import markerShadow from 'leaflet/dist/images/marker-shadow.png';

const defaultIcon = L.icon({
  iconUrl: markerIcon,
  iconRetinaUrl: markerIcon2x,
  shadowUrl: markerShadow,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

const userIcon = L.divIcon({
  className: '',
  html: '<div style="width:16px;height:16px;border-radius:50%;background:#2563eb;border:3px solid #fff;box-shadow:0 0 0 2px #2563eb"></div>',
  iconSize: [16, 16],
  iconAnchor: [8, 8],
});

export type MapPoint = {
  id?: string | number;
  lat: number;
  lng: number;
  title?: string;
};

type Props = {
  center?: { lat: number; lng: number } | null;
  userPos?: { lat: number; lng: number } | null;
  markers?: MapPoint[];
  pickable?: boolean;
  picked?: { lat: number; lng: number } | null;
  onPick?: (lat: number, lng: number) => void;
  height?: number;
  caption?: string;
  zoom?: number;
};

const DEFAULT_CENTER: [number, number] = [32.08, 34.78];

function Recenter({ center, zoom }: { center?: { lat: number; lng: number } | null; zoom: number }) {
  const map = useMap();
  useEffect(() => {
    if (center) map.flyTo([center.lat, center.lng], zoom, { duration: 0.8 });
  }, [center?.lat, center?.lng]); // eslint-disable-line react-hooks/exhaustive-deps
  return null;
}

function ClickPicker({ onPick }: { onPick?: (lat: number, lng: number) => void }) {
  useMapEvents({
    click(e) {
      onPick?.(e.latlng.lat, e.latlng.lng);
    },
  });
  return null;
}

export default function RealMap({
  center,
  userPos,
  markers = [],
  pickable = false,
  picked,
  onPick,
  height = 380,
  caption,
  zoom = 15,
}: Props) {
  const start: [number, number] = center
    ? [center.lat, center.lng]
    : DEFAULT_CENTER;

  return (
    <div>
      <MapContainer
        center={start}
        zoom={center ? zoom : 13}
        scrollWheelZoom
        style={{ height, width: '100%', borderRadius: 16, zIndex: 0 }}
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        <Recenter center={center} zoom={zoom} />

        {pickable && <ClickPicker onPick={onPick} />}

        {pickable && picked && (
          <Marker
            position={[picked.lat, picked.lng]}
            icon={defaultIcon}
            draggable
            eventHandlers={{
              dragend(e) {
                const p = (e.target as L.Marker).getLatLng();
                onPick?.(p.lat, p.lng);
              },
            }}
          />
        )}

        {userPos && (
          <Marker position={[userPos.lat, userPos.lng]} icon={userIcon}>
            <Popup>המיקום שלך</Popup>
          </Marker>
        )}

        {markers.map((m, i) => (
          <Marker key={m.id ?? i} position={[m.lat, m.lng]} icon={defaultIcon}>
            {m.title && <Popup>{m.title}</Popup>}
          </Marker>
        ))}
      </MapContainer>
      {caption && <div className="hint">{caption}</div>}
    </div>
  );
}