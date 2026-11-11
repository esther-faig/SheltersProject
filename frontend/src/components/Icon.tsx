import type { SVGProps, ReactNode } from 'react';

type IconName =
  | 'home' | 'pin' | 'user' | 'heart' | 'search'
  | 'shield' | 'check' | 'mail' | 'lock' | 'walk'
  | 'car' | 'arrow' | 'logout' | 'locate' | 'sliders'
  | 'users' | 'wheelchair' | 'wind';

const paths: Record<IconName, ReactNode> = {
  home: <>
    <path d="M3.4 11.2 12 4l8.6 7.2M6 9.6V20h12V9.6M10 20v-5.6h4V20" />
  </>,
  pin: <>
    <path d="M12 21.2C8 16.6 5.4 13.4 5.4 10a6.6 6.6 0 0 1 13.2 0c0 3.4-2.6 6.6-6.6 11.2Z" />
    <circle cx="12" cy="10" r="2.4" />
  </>,
  user: <>
    <circle cx="12" cy="8" r="3.6" />
    <path d="M5 20.4c.5-4 3.5-6.2 7-6.2s6.5 2.2 7 6.2" />
  </>,
  heart: <path d="M12 20.2C5 14.6 3.4 11.2 3.4 8.6a4.3 4.3 0 0 1 8.6-1.2 4.3 4.3 0 0 1 8.6 1.2C20.6 11.2 19 14.6 12 20.2Z" />,
  search: <>
    <circle cx="10.8" cy="10.8" r="6.4" />
    <path d="m16 16 4.5 4.5" />
  </>,
  shield: <>
    <path d="M12 2.8 19.6 5.8v5.6c0 4.6-3 8.2-7.6 9.8-4.6-1.6-7.6-5.2-7.6-9.8V5.8Z" />
    <path d="m8.6 12.2 2.5 2.6 4.4-5" />
  </>,
  check: <path d="m5 12.5 4.3 4.2L19 7" />,
  mail: <>
    <rect x="3" y="5" width="18" height="14" rx="2" />
    <path d="m4 7 8 6 8-6" />
  </>,
  lock: <>
    <rect x="4.5" y="10" width="15" height="11" rx="2" />
    <path d="M8 10V7a4 4 0 0 1 8 0v3" />
  </>,
  walk: <>
    <circle cx="13.2" cy="4.4" r="1.9" />
    <path d="m12 8.6-2.6 4 2.6 2.2v5.6m0-11.8 3 3 2.6.4m-8.2 1.6-2 3.2m5.2-1 3 2 .6 3.6" />
  </>,
  car: <>
    <path d="M3.4 16.4v-4.2l2-5h13.2l2 5v4.2Z M3.4 12.2h17.2" />
    <circle cx="7.6" cy="17.4" r="1.7" />
    <circle cx="16.4" cy="17.4" r="1.7" />
  </>,
  arrow: <path d="M19 12H5m7 7-7-7 7-7" />,
  logout: <path d="M10 4.6H5.4v14.8H10m-.4-7.4h10m-4-4 4 4-4 4" />,
  locate: <>
    <circle cx="12" cy="12" r="7" />
    <circle cx="12" cy="12" r="2" />
    <path d="M12 2v3m0 14v3M2 12h3m14 0h3" />
  </>,
  sliders: <>
    <path d="M4 6h16M4 12h16M4 18h16" />
    <circle cx="8" cy="6" r="2" fill="currentColor" />
    <circle cx="15" cy="12" r="2" fill="currentColor" />
    <circle cx="10" cy="18" r="2" fill="currentColor" />
  </>,
  users: <>
    <circle cx="9" cy="8.4" r="3" />
    <path d="M3.2 19.6c.4-3.4 2.8-5.2 5.8-5.2s5.4 1.8 5.8 5.2M16 5.6a3 3 0 0 1 0 5.8m1.6 3.4c2 .6 3.2 2.2 3.4 4.8" />
  </>,
  wheelchair: <>
    <circle cx="10" cy="4.4" r="1.8" />
    <path d="M10 7.6v6h5.2l2.6 5.6M10 11h4.6M7.4 11.6a5 5 0 1 0 7.8 6.6" />
  </>,
  wind: <path d="M3 9h10.4a2.6 2.6 0 1 0-2.6-2.6M3 13h15a2.6 2.6 0 1 1-2.6 2.6M3 17h6.6" />,
};

export default function Icon({
  name,
  ...props
}: { name: IconName } & SVGProps<SVGSVGElement>) {
  return (
    <svg
      className={`ic ${props.className ?? ''}`}
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
      aria-hidden="true"
      {...props}
    >
      {paths[name]}
    </svg>
  );
}