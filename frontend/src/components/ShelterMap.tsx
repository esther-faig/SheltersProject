/** מפה סכמטית — תואמת את mapSVG מקובץ ה-HTML */
export const PIN_POSITIONS = [
  { x: 395, y: 112 },
  { x: 172, y: 104 },
  { x: 440, y: 232 },
  { x: 128, y: 248 },
  { x: 250, y: 262 },
];

export const pinAt = (index: number) => PIN_POSITIONS[index % PIN_POSITIONS.length];

const BLOCKS = [
  [20, 20, 80, 40], [140, 22, 130, 32], [310, 18, 140, 40], [490, 22, 90, 38],
  [20, 92, 80, 60], [140, 92, 130, 60], [312, 90, 140, 64], [492, 92, 90, 60],
  [22, 190, 76, 56], [140, 198, 130, 52], [490, 190, 92, 56],
  [22, 284, 80, 40], [142, 286, 126, 40], [310, 286, 142, 40], [492, 286, 88, 40],
];
const STREETS = ['M0 72 H600', 'M0 172 H600', 'M0 266 H600', 'M120 0 V340', 'M290 0 V340', 'M470 0 V340', 'M0 330 L600 20'];
const PIN_PATH = 'M0 0 C-10 -12 -16 -20 -16 -30 A16 16 0 0 1 16 -30 C16 -20 10 -12 0 0 Z';

export type MapPin = { id: number | string; x: number; y: number };

type Props = {
  user?: boolean;
  pins?: MapPin[];
  route?: string | null;
  hl?: number | string | null;
  caption?: React.ReactNode;
  id?: string;
};

export default function ShelterMap({ user = false, pins = [], route = null, hl = null, caption, id }: Props) {
  return (
    <div className="mapbox" id={id}>
      <svg className="map" viewBox="0 0 600 340" role="img" aria-label="מפה סכמטית">
        <g strokeLinecap="round" strokeLinejoin="round">
          <rect width="600" height="340" fill="#eef3f4" />
          {BLOCKS.map((b, i) => (
            <rect key={i} x={b[0]} y={b[1]} width={b[2]} height={b[3]} rx="10" fill="#dfe9eb" />
          ))}
          {STREETS.map((d, i) => <path key={'a' + i} d={d} stroke="#c3d2d6" strokeWidth="18" fill="none" />)}
          {STREETS.map((d, i) => <path key={'b' + i} d={d} stroke="#ffffff" strokeWidth="13" fill="none" />)}
          <rect x="318" y="184" width="130" height="66" rx="22" fill="#cfe0c4" stroke="#263d49" strokeWidth="1.6" strokeDasharray="5 5" />
          {[[345, 208], [380, 222], [415, 206], [430, 230]].map((t, i) => (
            <circle key={i} cx={t[0]} cy={t[1]} r="9" fill="#7da57f" stroke="#263d49" strokeWidth="1.8" />
          ))}
          {route && (
            <>
              <path d={route} fill="none" stroke="#fff" strokeWidth="10" />
              <path d={route} fill="none" stroke="#d9862b" strokeWidth="5" strokeDasharray="2 11" />
            </>
          )}
          {user && (
            <g transform="translate(300 170)">
              <circle r="19" fill="#fff" stroke="#263d49" strokeWidth="2.6" />
              <circle r="8.5" fill="none" stroke="#3d6b7a" strokeWidth="3" />
              <circle r="3.2" fill="#3d6b7a" />
            </g>
          )}
          {pins.map((p) => {
            const big = hl === p.id;
            return (
              <g key={p.id} transform={`translate(${p.x} ${p.y}) scale(${big ? 1.3 : 1})`}>
                <path d={PIN_PATH} fill={big ? '#3d6b7a' : '#ffffff'} stroke="#3d6b7a" strokeWidth="3" strokeLinejoin="round" />
                <circle cx="0" cy="-30" r="6" fill={big ? '#ffffff' : 'none'} stroke={big ? '#ffffff' : '#3d6b7a'} strokeWidth="3" />
              </g>
            );
          })}
        </g>
      </svg>
      {caption && <div className="mapcap">{caption}</div>}
    </div>
  );
}
