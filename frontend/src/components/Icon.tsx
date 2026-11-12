import type { SVGProps } from 'react';

export type IconName =
  | 'shield' | 'shieldcheck' | 'pin' | 'walk' | 'car' | 'home' | 'user' | 'users'
  | 'heart' | 'wheelchair' | 'ramp' | 'wind' | 'expand' | 'chair' | 'camera'
  | 'check' | 'arrow-l' | 'arrow-r' | 'nav' | 'star' | 'edit' | 'clock' | 'info'
  | 'lock' | 'mail' | 'sliders' | 'locate' | 'x' | 'phone' | 'alert' | 'plus'
  | 'list' | 'logout' | 'key';

/** מחזיר אייקון מתאים למאפיין לפי שמו (עיצוב בלבד) */
export function featureIcon(name: string = ''): IconName {
  if (name.includes('מדרגות')) return 'ramp';
  if (name.includes('גלגלים') || name.includes('נגיש')) return 'wheelchair';
  if (name.includes('אוורור')) return 'wind';
  if (name.includes('צפיפות')) return 'users';
  if (name.includes('ישיבה')) return 'chair';
  if (name.includes('מרחב') || name.includes('מ״ר')) return 'expand';
  return 'check';
}

export function IconSprite() {
  return (
    <svg width="0" height="0" style={{ position: 'absolute' }} aria-hidden="true" focusable="false">
  <defs>
    <filter id="wobI" x="-10%" y="-10%" width="120%" height="120%"><feTurbulence type="fractalNoise" baseFrequency=".07" numOctaves="2" seed="4"/><feDisplacementMap in="SourceGraphic" scale="1.6"/></filter>
    <filter id="wobB" x="-3%" y="-3%" width="106%" height="106%"><feTurbulence type="fractalNoise" baseFrequency=".022" numOctaves="2" seed="8"/><feDisplacementMap in="SourceGraphic" scale="4"/></filter>
    <filter id="wob3" x="-2%" y="-2%" width="104%" height="104%"><feTurbulence type="fractalNoise" baseFrequency=".035" numOctaves="2" seed="2"/><feDisplacementMap in="SourceGraphic" scale="4"/></filter>

    <symbol id="i-shield" viewBox="0 0 24 24"><path d="M12 2.8 L19.6 5.8 V11.4 C19.6 16 16.6 19.6 12 21.2 C7.4 19.6 4.4 16 4.4 11.4 V5.8 Z"/></symbol>
    <symbol id="i-shieldcheck" viewBox="0 0 24 24"><path d="M12 2.8 L19.6 5.8 V11.4 C19.6 16 16.6 19.6 12 21.2 C7.4 19.6 4.4 16 4.4 11.4 V5.8 Z"/><path d="M8.6 12.2 l2.5 2.6 l4.4 -5"/></symbol>
    <symbol id="i-pin" viewBox="0 0 24 24"><path d="M12 21.2 C8 16.6 5.4 13.4 5.4 10 a6.6 6.6 0 0 1 13.2 0 c0 3.4 -2.6 6.6 -6.6 11.2 Z"/><circle cx="12" cy="10" r="2.4"/></symbol>
    <symbol id="i-walk" viewBox="0 0 24 24"><circle cx="13.2" cy="4.4" r="1.9"/><path d="M12 8.6 l-2.6 4 l2.6 2.2 v5.6 M12 8.6 l3 3 l2.6 .4 M9.4 12.6 l-2 3.2 M12 14.8 l3 2 l.6 3.6"/></symbol>
    <symbol id="i-car" viewBox="0 0 24 24"><path d="M3.4 16.4 v-4.2 l2 -5 h13.2 l2 5 v4.2 z M3.4 12.2 h17.2"/><circle cx="7.6" cy="17.4" r="1.7"/><circle cx="16.4" cy="17.4" r="1.7"/></symbol>
    <symbol id="i-home" viewBox="0 0 24 24"><path d="M3.4 11.2 L12 4 l8.6 7.2 M6 9.6 V20 h12 V9.6 M10 20 v-5.6 h4 V20"/></symbol>
    <symbol id="i-user" viewBox="0 0 24 24"><circle cx="12" cy="8" r="3.6"/><path d="M5 20.4 c.5 -4 3.5 -6.2 7 -6.2 s6.5 2.2 7 6.2"/></symbol>
    <symbol id="i-users" viewBox="0 0 24 24"><circle cx="9" cy="8.4" r="3"/><path d="M3.2 19.6 c.4 -3.4 2.8 -5.2 5.8 -5.2 s5.4 1.8 5.8 5.2 M16 5.6 a3 3 0 0 1 0 5.8 M17.6 14.8 c2 .6 3.2 2.2 3.4 4.8"/></symbol>
    <symbol id="i-heart" viewBox="0 0 24 24"><path d="M12 20.2 C5 14.6 3.4 11.2 3.4 8.6 a4.3 4.3 0 0 1 8.6 -1.2 a4.3 4.3 0 0 1 8.6 1.2 C20.6 11.2 19 14.6 12 20.2 Z"/></symbol>
    <symbol id="i-wheelchair" viewBox="0 0 24 24"><circle cx="10" cy="4.4" r="1.8"/><path d="M10 7.6 v6 h5.2 l2.6 5.6 M10 11 h4.6 M7.4 11.6 a5 5 0 1 0 7.8 6.6"/></symbol>
    <symbol id="i-ramp" viewBox="0 0 24 24"><path d="M3.6 19.4 H20.4 L3.6 9.6 Z M14 19.4 V14.8"/></symbol>
    <symbol id="i-wind" viewBox="0 0 24 24"><path d="M3 9 h10.4 a2.6 2.6 0 1 0 -2.6 -2.6 M3 13 h15 a2.6 2.6 0 1 1 -2.6 2.6 M3 17 h6.6"/></symbol>
    <symbol id="i-expand" viewBox="0 0 24 24"><path d="M4 9 V4 h5 M20 9 V4 h-5 M4 15 v5 h5 M20 15 v5 h-5"/></symbol>
    <symbol id="i-chair" viewBox="0 0 24 24"><path d="M8 3.6 v8.4 h8.4 v8 M8 12 v8 M8 7.8 h2.8"/></symbol>
    <symbol id="i-camera" viewBox="0 0 24 24"><path d="M3.6 8 h3.4 l1.6 -2.6 h6.8 l1.6 2.6 h3.4 v11.2 h-16.8 z"/><circle cx="12" cy="13.2" r="3.6"/></symbol>
    <symbol id="i-check" viewBox="0 0 24 24"><path d="M5 12.6 l4.6 4.6 l9.6 -10.4"/></symbol>
    <symbol id="i-arrow-l" viewBox="0 0 24 24"><path d="M20.4 12 H4.8 M10.8 6 l-6 6 l6 6"/></symbol>
    <symbol id="i-arrow-r" viewBox="0 0 24 24"><path d="M3.6 12 H19.2 M13.2 6 l6 6 l-6 6"/></symbol>
    <symbol id="i-nav" viewBox="0 0 24 24"><path d="M20.4 3.8 L3.8 11 l7.2 2.6 L13.6 20.4 z"/></symbol>
    <symbol id="i-star" viewBox="0 0 24 24"><path d="M12 3.4 l2.7 5.5 6 .9 -4.4 4.2 1 6 -5.3 -2.9 -5.3 2.9 1 -6 -4.4 -4.2 6 -.9 z"/></symbol>
    <symbol id="i-edit" viewBox="0 0 24 24"><path d="M4 20.2 l1 -4.2 L16.2 4.8 l3.2 3.2 L8.2 19.2 z M14 7 l3.2 3.2"/></symbol>
    <symbol id="i-clock" viewBox="0 0 24 24"><circle cx="12" cy="12" r="8.6"/><path d="M12 7.4 V12 l3.2 2"/></symbol>
    <symbol id="i-info" viewBox="0 0 24 24"><circle cx="12" cy="12" r="9"/><path d="M12 11 v5.4 M12 7.8 v.2"/></symbol>
    <symbol id="i-lock" viewBox="0 0 24 24"><rect x="5" y="10.6" width="14" height="10" rx="2.4"/><path d="M8 10.6 V8 a4 4 0 0 1 8 0 v2.6"/></symbol>
    <symbol id="i-mail" viewBox="0 0 24 24"><rect x="3.4" y="5.6" width="17.2" height="13" rx="2.4"/><path d="M4 7.2 l8 6.4 l8 -6.4"/></symbol>
    <symbol id="i-sliders" viewBox="0 0 24 24"><path d="M4 7 h8.4 M17.6 7 H20 M4 17 h2.4 M11.6 17 H20"/><circle cx="15" cy="7" r="2.4"/><circle cx="9" cy="17" r="2.4"/></symbol>
    <symbol id="i-locate" viewBox="0 0 24 24"><circle cx="12" cy="12" r="6"/><circle cx="12" cy="12" r="1.6"/><path d="M12 2.4 v3.2 M12 18.4 v3.2 M2.4 12 h3.2 M18.4 12 h3.2"/></symbol>
    <symbol id="i-x" viewBox="0 0 24 24"><path d="M6 6 l12 12 M18 6 l-12 12"/></symbol>
    <symbol id="i-phone" viewBox="0 0 24 24"><path d="M6 4 h3.6 l1.6 4 -2 1.6 a11 11 0 0 0 5.2 5.2 l1.6 -2 4 1.6 v3.4 a2 2 0 0 1 -2 2 C9.4 19.8 4 14.4 4 6 a2 2 0 0 1 2 -2 z"/></symbol>
    <symbol id="i-alert" viewBox="0 0 24 24"><path d="M12 4 L21 19.6 H3 Z M12 10 v4.6 M12 17 v.2"/></symbol>
    <symbol id="i-plus" viewBox="0 0 24 24"><path d="M12 5 v14 M5 12 h14"/></symbol>
    <symbol id="i-list" viewBox="0 0 24 24"><path d="M9 6.4 h11 M9 12 h11 M9 17.6 h11 M4.6 6.4 h.2 M4.6 12 h.2 M4.6 17.6 h.2"/></symbol>
    <symbol id="i-logout" viewBox="0 0 24 24"><path d="M10 4.6 H5.4 v14.8 H10 M9.6 12 h10 M15.6 8 l4 4 l-4 4"/></symbol>
    <symbol id="i-key" viewBox="0 0 24 24"><circle cx="8" cy="12" r="3.6"/><path d="M11.6 12 H21 M17 12 v3.4 M20.6 12 v2.4"/></symbol>
  </defs>
</svg>
  );
}

export default function Icon({
  name,
  className,
  ...props
}: { name: IconName } & SVGProps<SVGSVGElement>) {
  return (
    <svg className={`ic${className ? ' ' + className : ''}`} aria-hidden="true" {...props}>
      <use href={`#i-${name}`} />
    </svg>
  );
}
