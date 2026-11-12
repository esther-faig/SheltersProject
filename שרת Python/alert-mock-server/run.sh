#!/usr/bin/env bash
set -e
[ -d .venv ] || { python3 -m venv .venv; . .venv/bin/activate; pip install -r requirements.txt; }
. .venv/bin/activate
echo "Swagger: http://localhost:8000/docs"
uvicorn app.main:app --reload --port 8000
