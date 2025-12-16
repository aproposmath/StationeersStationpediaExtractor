FROM cm2network/steamcmd:latest
USER root
RUN apt update && apt install -y file jq
USER steam
ADD bepinex /opt/bepinex
ADD run.sh /opt/run.sh
