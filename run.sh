dotnet build
cp ./bin/Debug/net46/StationeersDataExtractor.dll ./BepInEx/plugins/

mkdir -p data-beta
mkdir -p data-stable

docker-compose build
docker-compose up --remove-orphans

python upload.py beta
python upload.py stable
