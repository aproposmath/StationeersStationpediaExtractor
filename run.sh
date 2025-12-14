#! /bin/bash
set -e
dotnet build -c Release
cp bin/Release/net46/StationeersDataExtractor.dll docker
docker build docker -t stationeers-extractor-beta
docker run -v $(pwd)/data-beta:/opt/data stationeers-extractor-beta
