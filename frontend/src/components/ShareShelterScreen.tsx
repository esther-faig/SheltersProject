import { useState, useEffect, type FormEvent, type ChangeEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { fetchPreferences } from '../store/preferences/preferencesThunks';
import { addShelter } from '../store/shelters/sheltersThunks';
import Icon, { featureIcon } from './Icon';
import ShelterMap from './ShelterMap';

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
    <section className="screen active">
      <div className="wrap inner" style={{ maxWidth: 980 }}>
        <div className="phead">
          <button
            className="iconbtn"
            type="button"
            onClick={() => navigate(-1)}
            aria-label="חזרה"
          >
            <Icon name="arrow-r" />
          </button>
          <div>
            <h2>שיתוף מקלט פרטי</h2>
            <div className="sub">
              מציעים את הממ״ד או המרחב המוגן שלכם לאנשים בסביבה.
            </div>
          </div>
        </div>

        <form onSubmit={handleSubmit} noValidate>
          <div className="cols">
            <div className="stack">
              <div className="card stack">
                <div>
                  <label className="lbl" htmlFor="shName">שם המקלט</label>
                  <div className="field">
                    <Icon name="home" />
                    <input
                      id="shName"
                      type="text"
                      value={shelterName}
                      onChange={(e) => setShelterName(e.target.value)}
                      placeholder="למשל: ממ״ד ברחוב התמר 4"
                    />
                  </div>
                </div>

                <div>
                  <span className="lbl">סוג המקום</span>
                  {['ממ"ד פרטי', 'מקלט פרטי', 'מרחב מוגן אחר'].map((type) => (
                    <label className="chk radio" key={type}>
                      <input
                        type="radio"
                        name="shType"
                        value={type}
                        checked={shelterType === type}
                        onChange={() => setShelterType(type)}
                      />
                      <span className="box"></span>
                      <span className="lt">{type}</span>
                    </label>
                  ))}
                </div>

                <div>
                  <label className="lbl" htmlFor="shCap">
                    קיבולת מקסימלית (מספר אנשים)
                  </label>
                  <div className="field">
                    <Icon name="users" />
                    <input
                      id="shCap"
                      type="number"
                      min="0"
                      value={maxCapacity}
                      onChange={(e) => setMaxCapacity(e.target.value)}
                      placeholder="10"
                    />
                  </div>
                </div>
              </div>

              <div className="card">
                <span className="lbl">מיקום / כתובת</span>

                <div className="field" style={{ marginBottom: 12 }}>
                  <Icon name="pin" />
                  <input
                    id="shAddr"
                    type="text"
                    placeholder="כתובת (רחוב ומספר בית)"
                    value={address}
                    onChange={(e) => setAddress(e.target.value)}
                  />
                </div>

                <div className="field" style={{ marginBottom: 12 }}>
                  <Icon name="pin" />
                  <input
                    id="shCity"
                    type="text"
                    placeholder="עיר"
                    value={city}
                    onChange={(e) => setCity(e.target.value)}
                  />
                </div>

                <ShelterMap id="shareMap" caption="מפה להמחשה בלבד" />

                <div className="hint">מיקום המקלט על המפה (קו רוחב וקו אורך):</div>

                <div className="field" style={{ marginTop: 10 }}>
                  <Icon name="locate" />
                  <input
                    type="text"
                    value={lat}
                    onChange={(e) => setLat(e.target.value)}
                    placeholder="קו רוחב (Latitude) — למשל 32.080000"
                  />
                </div>
                <div className="field" style={{ marginTop: 10 }}>
                  <Icon name="locate" />
                  <input
                    type="text"
                    value={lng}
                    onChange={(e) => setLng(e.target.value)}
                    placeholder="קו אורך (Longitude) — למשל 34.780000"
                  />
                </div>

                <button
                  type="button"
                  className="btn small soft"
                  style={{ marginTop: 12 }}
                  onClick={handleLocate}
                  disabled={locating}
                >
                  <span className="sh"><Icon name="locate" /></span>
                  {locating ? 'מאתר...' : 'השתמש במיקום שלי'}
                </button>
              </div>
            </div>

            <div className="stack">
              <div className="card">
                <span className="lbl">מאפיינים</span>
                {prefsLoading && <div className="hint">טוען מאפיינים...</div>}
                {prefsError && (
                  <div className="err">שגיאה בטעינת מאפיינים: {prefsError}</div>
                )}
                <div id="shFeats">
                  {prefs.map((p) => (
                    <label key={p.preferenceId} className="chk">
                      <input
                        type="checkbox"
                        checked={available[p.preferenceId] ?? false}
                        onChange={() => togglePref(p.preferenceId)}
                      />
                      <span className="box"><Icon name="check" /></span>
                      <span className="lt">
                        <Icon name={featureIcon(p.name)} />
                        {p.name}
                      </span>
                    </label>
                  ))}
                </div>
              </div>

              <div className="card">
                <span className="lbl">
                  תמונות (עד {MAX_IMAGES}, jpg / png / webp)
                </span>
                <label className="upload" id="upLabel">
                  <input
                    type="file"
                    multiple
                    accept=".jpg,.jpeg,.png,.webp"
                    onChange={handleFiles}
                  />
                  <span id="upIn">
                    <Icon name="camera" style={{ width: 34, height: 34 }} />
                    <br />
                    העלאת תמונות
                  </span>
                </label>
                {files.length > 0 && (
                  <div className="chips file-chips">
                    {files.map((f) => (
                      <span key={f.name} className="chip">{f.name}</span>
                    ))}
                  </div>
                )}
              </div>

              {message && (
                <div className="err" id="shErr" role="alert">{message}</div>
              )}

              <button
                type="submit"
                className="btn primary block"
                disabled={
                  sharing ||
                  shelterName.trim() === '' ||
                  address.trim() === '' ||
                  city.trim() === '' ||
                  shelterType === '' ||
                  !coordsValid
                }
              >
                <span className="sh"><Icon name="heart" /></span>
                {sharing ? 'שולח...' : 'שיתוף המקלט'}
              </button>

              <div className="info">
                <Icon name="info" />
                המקלט יופיע בחיפוש רק אחרי אישור של מנהל המערכת.
              </div>
            </div>
          </div>
        </form>
      </div>
    </section>
  );
}

export default ShareShelterScreen;
