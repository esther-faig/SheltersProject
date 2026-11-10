import { useState, useEffect, type FormEvent, type ChangeEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { fetchPreferences } from '../store/preferences/preferencesThunks';
import { addShelter } from '../store/shelters/sheltersThunks';

const MAX_IMAGES = 5;
const MAX_IMAGE_BYTES = 5 * 1024 * 1024;

function ShareShelterScreen() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  // נתונים מהשרת: ב-Redux
  const { items: prefs, loading: prefsLoading, error: prefsError } = useAppSelector(
    (state) => state.preferences
  );
  const sharing = useAppSelector((state) => state.shelters.loading);

  // ערכי הטופס: state רגיל
  const [shelterName, setShelterName] = useState('');
  const [shelterType, setShelterType] = useState('');
  const [address, setAddress] = useState('');
  const [city, setCity] = useState('');
  const [maxCapacity, setMaxCapacity] = useState('10');
  const [lat, setLat] = useState('');
  const [lng, setLng] = useState('');
  const [available, setAvailable] = useState<Record<number, boolean>>({});
  const [files, setFiles] = useState<File[]>([]);
  const [message, setMessage] = useState('');
  const [locating, setLocating] = useState(false);

  useEffect(() => {
    document.title = 'שיתוף מקלט';
    dispatch(fetchPreferences());
  }, [dispatch]);

  function togglePref(id: number) {
    setAvailable({ ...available, [id]: !available[id] });
  }

  function handleLocate() {
    if (!navigator.geolocation) {
      setMessage('הדפדפן לא תומך באיתור מיקום');
      return;
    }
    setLocating(true);
    setMessage('');
    navigator.geolocation.getCurrentPosition(
      (pos) => {
        setLat(pos.coords.latitude.toFixed(6));
        setLng(pos.coords.longitude.toFixed(6));
        setLocating(false);
      },
      () => {
        setMessage('לא הצלחנו לאתר מיקום. אפשר להקליד קווי אורך ורוחב ידנית');
        setLocating(false);
      },
      { enableHighAccuracy: true, timeout: 10000 }
    );
  }

  function handleFiles(e: ChangeEvent<HTMLInputElement>) {
    const chosen = Array.from(e.target.files ?? []);
    if (chosen.length > MAX_IMAGES) {
      setMessage(`אפשר להעלות עד ${MAX_IMAGES} תמונות`);
      return;
    }
    if (chosen.some((f) => f.size > MAX_IMAGE_BYTES)) {
      setMessage('כל תמונה יכולה להיות עד 5MB');
      return;
    }
    setMessage('');
    setFiles(chosen);
  }

  const latNum = parseFloat(lat);
  const lngNum = parseFloat(lng);
  const coordsValid =
    !isNaN(latNum) && !isNaN(lngNum) &&
    latNum >= -90 && latNum <= 90 &&
    lngNum >= -180 && lngNum <= 180;

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setMessage('');

    // השרת מצפה ל-multipart/form-data, ושמות השדות כמו ב-ShelterRequestDTO
    const form = new FormData();
    form.append('Name', shelterName);
    form.append('ShelterType', shelterType);
    form.append('Address', address);
    form.append('City', city);
    form.append('Latitude', String(latNum));
    form.append('Longitude', String(lngNum));
    form.append('MaxCapacity', String(parseInt(maxCapacity) || 0));
    prefs.forEach((p, i) => {
      form.append(`Features[${i}].PreferenceId`, String(p.preferenceId));
      form.append(`Features[${i}].IsAvailable`, String(available[p.preferenceId] ?? false));
    });
    files.forEach((file) => form.append('ImageFiles', file));

    try {
      await dispatch(addShelter(form)).unwrap();
      navigate('/profile');
    } catch (err) {
      setMessage((err as { message: string }).message);
    }
  }

  return (
    <div className="screen">
      <h2 className="title">שיתוף מקלט פרטי</h2>

      <form className="form" onSubmit={handleSubmit}>
        <label className="field">
          שם המקלט
          <input value={shelterName} onChange={(e) => setShelterName(e.target.value)} />
        </label>

        <label className="field">
          סוג המקום
          <select value={shelterType} onChange={(e) => setShelterType(e.target.value)}>
            <option value="">בחר סוג...</option>
            <option value='ממ"ד פרטי'>ממ"ד פרטי</option>
            <option value="מקלט פרטי">מקלט פרטי</option>
            <option value="מרחב מוגן אחר">מרחב מוגן אחר</option>
          </select>
        </label>

        <label className="field">
          כתובת
          <input value={address} onChange={(e) => setAddress(e.target.value)} />
        </label>

        <label className="field">
          עיר
          <input value={city} onChange={(e) => setCity(e.target.value)} />
        </label>

        <label className="field">
          קיבולת מקסימלית (מספר אנשים)
          <input
            type="number"
            min="0"
            value={maxCapacity}
            onChange={(e) => setMaxCapacity(e.target.value)}
          />
        </label>

        <h3>מיקום המקלט על המפה</h3>
        <label className="field">
          קו רוחב (Latitude)
          <input value={lat} onChange={(e) => setLat(e.target.value)} placeholder="למשל 32.080000" />
        </label>
        <label className="field">
          קו אורך (Longitude)
          <input value={lng} onChange={(e) => setLng(e.target.value)} placeholder="למשל 34.780000" />
        </label>
        <button type="button" className="btn small" onClick={handleLocate} disabled={locating}>
          {locating ? 'מאתר...' : '📍 השתמש במיקום שלי'}
        </button>

        <h3>מאפיינים</h3>
        {prefsLoading && <p>טוען מאפיינים...</p>}
        {prefsError && <p>שגיאה בטעינת מאפיינים: {prefsError}</p>}
        <div className="prefs">
          {prefs.map((p) => (
            <label key={p.preferenceId} className="pref-row">
              <input
                type="checkbox"
                checked={available[p.preferenceId] ?? false}
                onChange={() => togglePref(p.preferenceId)}
              />{' '}
              {p.name}
            </label>
          ))}
        </div>

        <label className="field">
          תמונות (עד {MAX_IMAGES}, jpg / png / webp)
          <input type="file" multiple accept=".jpg,.jpeg,.png,.webp" onChange={handleFiles} />
        </label>
        {files.length > 0 && (
          <div className="tags">
            {files.map((f) => (
              <span key={f.name} className="chip">{f.name}</span>
            ))}
          </div>
        )}

        {message && <p>{message}</p>}

        <button
          type="submit"
          className="btn signal"
          disabled={
            sharing ||
            shelterName.trim() === '' ||
            address.trim() === '' ||
            city.trim() === '' ||
            shelterType === '' ||
            !coordsValid
          }
        >
          {sharing ? 'שולח...' : 'שיתוף המקלט'}
        </button>
      </form>
    </div>
  );
}

export default ShareShelterScreen;