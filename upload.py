#!/usr/bin/env python3

import os
import hashlib
import github3
from pathlib import Path
import sys
import mimetypes

GITHUB_TOKEN = Path(".github_token").read_text().strip()
REPO_NAME = "aproposmath/StationeersStationpediaExtractor"
RELEASE_TAG = sys.argv[1]
UPLOAD_DIR = f"data-{RELEASE_TAG}"

finished_file = Path(UPLOAD_DIR) / "finished"

if not finished_file.exists():
    print(f"Skipping upload to {RELEASE_TAG} due to unfinished data extraction")
    sys.exit(0)

if not GITHUB_TOKEN:
    raise ValueError("Please set the GITHUB_TOKEN environment variable")

def sha256(filepath):
    h = hashlib.sha256()
    with open(filepath, "rb") as f:
        for chunk in iter(lambda: f.read(4096), b""):
            h.update(chunk)
    return h.hexdigest()

gh = github3.login(token=GITHUB_TOKEN)
repo = gh.repository(*REPO_NAME.split("/"))

release = repo.release_from_tag(RELEASE_TAG)
if release is None:
    raise ValueError(f"Release with tag {RELEASE_TAG} not found")

local_files = {}
for f in os.listdir(UPLOAD_DIR):
    if f in ['finished', 'server.log']:
        continue
    filepath = os.path.join(UPLOAD_DIR, f)
    if not os.path.isfile(filepath):
        continue
    file_hash = sha256(filepath)
    local_files[f] = file_hash

remote_assets = {}
for asset in release.assets():
    remote_assets[asset.name] = (asset, asset.digest.split("sha256:")[1])

for name, (asset, remote_hash) in remote_assets.items():
    local_hash = local_files.get(name)
    if local_hash != remote_hash:
        print("Deleting remote asset:", name)
        asset.delete()

for filename, file_hash in local_files.items():
    if filename not in remote_assets or remote_assets[filename][1] != file_hash:
        filepath = os.path.join(UPLOAD_DIR, filename)
        content_type, _ = mimetypes.guess_type(filepath)
        if content_type is None:
            content_type = "application/octet-stream"
        print(f"Uploading: {filename} to {RELEASE_TAG}")
        with open(filepath, "rb") as f:
            release.upload_asset(
                name=filename,
                asset=f,
                content_type=content_type,
                label=filename,
            )

print(f"Release sync for {RELEASE_TAG} complete!")
