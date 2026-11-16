
import csv
import json
from pathlib import Path

# נתיבים לקבצים
BASE_DIR = Path(__file__).parent
DATA_DIR = BASE_DIR / "DATA"

INPUT_FILE = DATA_DIR / "PetahTikvaShelters.tsv"
OUTPUT_FILE = DATA_DIR / "PetahTikvaShelters.geojson"

# שמות העמודות שאמורות להופיע בקובץ המקור
REQUIRED_COLUMNS = [
    "address",
    "type",
    "area",
    "open",
    "neighborhood",
    "note",
    "lat",
    "lng"
]


def empty_to_none(value):
    """המרת תא ריק ל-None."""
    if value is None or value.strip() == "":
        return None
    return value.strip()


def create_geojson():

    features = []

    # קריאת קובץ הנתונים
    with open(INPUT_FILE, "r", encoding="utf-8-sig", newline="") as file:

        reader = csv.DictReader(file, delimiter="\t")

        # ניקוי רווחים משמות העמודות
        reader.fieldnames = [
            name.strip() for name in reader.fieldnames
        ]

        # בדיקה שכל העמודות הדרושות קיימות
        missing_columns = [
            column for column in REQUIRED_COLUMNS
            if column not in reader.fieldnames
        ]

        if missing_columns:
            raise ValueError(
                f"חסרות עמודות בקובץ: {missing_columns}"
            )

        for row_number, row in enumerate(reader, start=2):

            # דילוג על שורות ריקות
            if not row.get("address") and not row.get("lat"):
                continue

            try:
                latitude = float(row["lat"].strip())
                longitude = float(row["lng"].strip())

            except (ValueError, AttributeError):
                raise ValueError(
                    f"שגיאה בקואורדינטות בשורה {row_number}"
                )

            # שמירת כל המאפיינים של המקלט
            properties = {
                "address": empty_to_none(row.get("address")),
                "type": empty_to_none(row.get("type")),
                "area": empty_to_none(row.get("area")),
                "open": empty_to_none(row.get("open")),
                "neighborhood": empty_to_none(
                    row.get("neighborhood")
                ),
                "note": empty_to_none(row.get("note")),
                "lat": latitude,
                "lng": longitude
            }

            # יצירת Feature מסוג Point
            feature = {
                "type": "Feature",
                "properties": properties,
                "geometry": {
                    "type": "Point",

                    # ב-GeoJSON הסדר הוא longitude ואז latitude
                    "coordinates": [
                        longitude,
                        latitude
                    ]
                }
            }

            features.append(feature)

    # יצירת מבנה GeoJSON מלא
    geojson = {
        "type": "FeatureCollection",
        "features": features
    }

    # כתיבת הקובץ
    with open(OUTPUT_FILE, "w", encoding="utf-8") as file:
        json.dump(
            geojson,
            file,
            ensure_ascii=False,
            indent=4
        )

    print("הקובץ נוצר בהצלחה!")
    print(f"מספר המקלטים שיובאו: {len(features)}")
    print(f"מיקום הקובץ: {OUTPUT_FILE}")


if __name__ == "__main__":
    create_geojson()
