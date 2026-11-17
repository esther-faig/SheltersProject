import { useEffect, useRef, useState, type ChangeEvent, type FormEvent } from 'react';
import Icon, { featureIcon } from './Icon';
import RealMap from './RealMap';
import { SHELTER_TYPES } from '../shelterTypes';
import { useAppSelector } from '../store/hooks';
import { getMyShelter, updateMyShelter } from '../api/shelters';

const IMAGES_URL = import.meta.env.VITE_IMAGES_URL as string;
const MAX_IMAGES = 5;
const MAX_IMAGE_BYTES = 5 * 1024 * 1024;

type Props = {
  shelterId: number;
  onClose: () => void;
  onSaved: () => void;
};

function EditShelterDialog({ shelterId, onClose, onSaved }: Props) {
  const ref = useRef<HTMLDialogElement>(null);
  const prefs = useAppSelector((s) => s.preferences.items);

  const [ready, setReady] = useState(false);
  const [name, setName] = useState('');
  const [type, setType] = useState('');
  const [address, setAddress] = useState('');
  const [city, setCity] = useState('');
  const [capacity, setCapacity] = useState('');
  const [lat, setLat] = useState('');
  const [lng, setLng] = useState('');
  const [available, setAvailable] = useState<Record<number, boolean>>({});
  const [keepFiles, setKeepFiles] = useState<string[]>([]);
  const [newFiles, setNewFiles] = useState<File[]>([]);
  const [message, setMessage] = useState('');
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    ref.current?.showModal();
  }, []);

  useEffect(() => {
    getMyShelter(shelterId)
      .then((s) => {
        setName(s.name);
        setType(s.shelterType);
        setAddress(s.address ?? '');
        setCity(s.city);
        setCapacity(String(s.maxCapacity || ''));
        setLat(String(s.latitude));
        setLng(String(s.longitude));
        setKeepFiles(s.files);

        const map: Record<number, boolean> = {};

        s.features.forEach((f) => {
          map[f.preferenceId] = f.isAvailable === true;
        });

        setAvailable(map);
        setReady(true);
      })
      .catch((err) => setMessage((err as Error).message));
  }, [shelterId]);

  const latNum = parseFloat(lat);
  const lngNum = parseFloat(lng);

  const coordsValid =
    !isNaN(latNum) &&
    !isNaN(lngNum) &&
    latNum >= -90 &&
    latNum <= 90 &&
    lngNum >= -180 &&
    lngNum <= 180;

  // אם הסוג הנוכחי לא ברשימה (למשל סוג ישן), מוסיפים אותו כדי שלא יאבד
  const typeOptions =
    SHELTER_TYPES.includes(type) || !type
      ? SHELTER_TYPES
      : [type, ...SHELTER_TYPES];

  function handleFiles(e: ChangeEvent<HTMLInputElement>) {
    const chosen = Array.from(e.target.files ?? []);

    if (keepFiles.length + chosen.length > MAX_IMAGES) {
      setMessage(`אפשר עד ${MAX_IMAGES} תמונות בסך הכול`);
      return;
    }

    if (chosen.some((f) => f.size > MAX_IMAGE_BYTES)) {
      setMessage('כל תמונה יכולה להיות עד 5MB');
      return;
    }

    setMessage('');
    setNewFiles(chosen);
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setMessage('');
    setSaving(true);

    const form = new FormData();

    form.append('Name', name);
    form.append('ShelterType', type);
    form.append('Address', address);
    form.append('City', city);
    form.append('Latitude', String(latNum));
    form.append('Longitude', String(lngNum));
    form.append('MaxCapacity', String(parseInt(capacity) || 1));

    prefs.forEach((p, i) => {
      form.append(`Features[${i}].PreferenceId`, String(p.preferenceId));
      form.append(
        `Features[${i}].IsAvailable`,
        String(available[p.preferenceId] ?? false)
      );
    });

    keepFiles.forEach((f) => form.append('ExistingFiles', f));
    newFiles.forEach((f) => form.append('ImageFiles', f));

    try {
      await updateMyShelter(shelterId, form);
      onSaved();
      onClose();
    } catch (err) {
      setMessage((err as Error).message || 'השמירה נכשלה');
    } finally {
      setSaving(false);
    }
  }

  const canSave =
    ready &&
    !saving &&
    name.trim() !== '' &&
    city.trim() !== '' &&
    type !== '' &&
    coordsValid;

  return (
    <dialog
      ref={ref}
      onClose={onClose}
      style={{ maxWidth: 'min(720px, 94vw)' }}
    >
      <form className="dlg" onSubmit={handleSubmit} noValidate>
        <h3>עריכת מקלט</h3>

        <div className="info" style={{ marginBottom: 14 }}>
          <Icon name="info" />
          <span>
            שינוי שם או מאפיינים נשמר מיד. שינוי כתובת, מיקום או תמונה יחזיר
            את המקלט להמתנה לאישור מנהל.
          </span>
        </div>

        {!ready ? (
          <div className="hint">{message || 'טוען...'}</div>
        ) : (
          <div className="dlg-scroll stack">
            <div>
              <label className="lbl" htmlFor="ed-name">
                שם המקלט
              </label>

              <div className="field">
                <Icon name="home" />
                <input
                  id="ed-name"
                  type="text"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                />
              </div>
            </div>

            <div>
              <label className="lbl" htmlFor="ed-type">
                סוג המקום
              </label>

              <select
                id="ed-type"
                className="select"
                value={type}
                onChange={(e) => setType(e.target.value)}
              >
                {typeOptions.map((t) => (
                  <option key={t} value={t}>
                    {t}
                  </option>
                ))}
              </select>
            </div>

            <div className="cols">
              <div>
                <label className="lbl" htmlFor="ed-address">
                  כתובת
                </label>

                <div className="field">
                  <Icon name="pin" />
                  <input
                    id="ed-address"
                    type="text"
                    value={address}
                    onChange={(e) => setAddress(e.target.value)}
                  />
                </div>
              </div>

              <div>
                <label className="lbl" htmlFor="ed-city">
                  עיר
                </label>

                <div className="field">
                  <Icon name="pin" />
                  <input
                    id="ed-city"
                    type="text"
                    value={city}
                    onChange={(e) => setCity(e.target.value)}
                  />
                </div>
              </div>
            </div>

            <div>
              <label className="lbl" htmlFor="ed-cap">
                קיבולת מקסימלית
              </label>

              <div className="field">
                <Icon name="users" />
                <input
                  id="ed-cap"
                  type="number"
                  min="1"
                  value={capacity}
                  onChange={(e) => setCapacity(e.target.value)}
                />
              </div>
            </div>

            <div>
              <span className="lbl">מיקום על המפה</span>

              <RealMap
                pickable
                height={220}
                picked={coordsValid ? { lat: latNum, lng: lngNum } : null}
                center={coordsValid ? { lat: latNum, lng: lngNum } : null}
                onPick={(la, ln) => {
                  setLat(la.toFixed(6));
                  setLng(ln.toFixed(6));
                }}
                caption="לחצו על המפה או גררו את הסמן כדי לשנות מיקום"
              />
            </div>

            <div>
              <span className="lbl">מאפיינים</span>

              {prefs.map((p) => (
                <label key={p.preferenceId} className="chk">
                  <input
                    type="checkbox"
                    checked={available[p.preferenceId] ?? false}
                    onChange={() =>
                      setAvailable({
                        ...available,
                        [p.preferenceId]: !available[p.preferenceId],
                      })
                    }
                  />

                  <span className="box">
                    <Icon name="check" />
                  </span>

                  <span className="lt">
                    <Icon name={featureIcon(p.name)} />
                    {p.name}
                  </span>
                </label>
              ))}
            </div>

            <div>
              <span className="lbl">תמונות (עד {MAX_IMAGES})</span>

              {keepFiles.length > 0 && (
                <div className="chips" style={{ marginBottom: 10 }}>
                  {keepFiles.map((f) => (
                    <span className="thumb-edit" key={f}>
                      <img src={`${IMAGES_URL}/${f}`} alt="" />

                      <button
                        type="button"
                        className="iconx"
                        aria-label="הסרת תמונה"
                        onClick={() =>
                          setKeepFiles(keepFiles.filter((x) => x !== f))
                        }
                      >
                        <Icon name="x" />
                      </button>
                    </span>
                  ))}
                </div>
              )}

              <label className="upload">
                <input
                  type="file"
                  multiple
                  accept=".jpg,.jpeg,.png,.webp"
                  onChange={handleFiles}
                />

                <Icon name="camera" style={{ width: 28, height: 28 }} />
                <br />
                הוספת תמונות
              </label>

              {newFiles.length > 0 && (
                <div className="chips file-chips">
                  {newFiles.map((f) => (
                    <span key={f.name} className="chip">
                      {f.name}
                    </span>
                  ))}
                </div>
              )}
            </div>
          </div>
        )}

        {message && ready && (
          <div className="err" role="alert">
            {message}
          </div>
        )}

        <div className="actions" style={{ marginTop: 18 }}>
          <button
            className="btn primary small"
            type="submit"
            disabled={!canSave}
          >
            <span className="sh">
              <Icon name="check" />
            </span>
            {saving ? 'שומר...' : 'שמירה'}
          </button>

          <button
            className="btn small"
            type="button"
            onClick={() => ref.current?.close()}
          >
            ביטול
          </button>
        </div>
      </form>
    </dialog>
  );
}

export default EditShelterDialog;