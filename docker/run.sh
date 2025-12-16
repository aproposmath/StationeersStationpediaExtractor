#!/bin/bash
set -e

export PATH=$PATH:/home/steam/steamcmd
APPID=600760
BRANCH=$1

curl https://github.com/aproposmath/StationeersStationpediaExtractor/releases/download/stable/version.json > /tmp/last_extracted.json

echo "Latest extracted version:"
cat /tmp/last_extracted.json

curl https://api.steamcmd.net/v1/info/600760 | jq -S ".data[\"600760\"][\"depots\"][\"branches\"][\"${BRANCH}\"]" > /tmp/new_version.json
echo "Current version:"
cat /tmp/new_version.json

diff /tmp/new_version.json data/version.json && echo "No new build available" && exit 0

echo "Update branch $BRANCH"

for i in 1 2 3 4 5; do
    steamcmd.sh +login anonymous \
             +app_update $APPID -beta "${BRANCH}" validate \
             +quit && break; \
    sleep 5; \
done

cd "/home/steam/Steam/steamapps/common/Stationeers Dedicated Server"

rm -f data || true
rm -rf data || true
ln -s /opt/data data

rm -f BepInEx || true
rm -rf BepInEx || true
ln -s /opt/BepInEx BepInEx

rm -rf data/*
cp /tmp/new_version.json data/version.json
touch data/server.log

cp -r /opt/bepinex/* .
sed -i 's|^executable_name=""$|executable_name="./rocketstation_DedicatedServer.x86_64"|' run_bepinex.sh
chmod +x run_bepinex.sh

timeout 60s ./run_bepinex.sh \
  -file start \
  MyLunarMap Lunar \
  -logFile ./data/server.log \
  -settings \
  StartLocalHost true \
  ServerVisible true \
  GamePort 27016 \
  UPNPEnabled false \
  ServerName MyLunarServer \
  ServerPassword MySuperSecurePassword \
  ServerMaxPlayers 1 \
  AutoSave false \
  SaveInterval 300 \
  ServerAuthSecret MySuperSecureSecret \
  UpdatePort 27015 \
  AutoPauseServer true \
  UseSteamP2P false \
  LocalIpAddress 0.0.0.0

if [ $? -eq 124 ]; then
  echo "Command timed out" && exit 1
fi

touch data/finished

echo "Finished"
