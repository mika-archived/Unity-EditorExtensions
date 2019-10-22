#!/usr/bin/env bash
set -eu

xvfb-run --auto-servernum --server-args='-screen 0 640x480x24' \
  /opt/Unity/Editor/Unity \
  -logFile /dev/stdout \
  -batchmode \
  -username "$UNITY_USERNAME" \
  -password "$UNITY_PASSWORD" | tee >> /var/log/unity3d.log
grep -e "LICENSE SYSTEM \[.*\] Posting" /var/log/unity3d.log | \
  sed -r "s/^.*(<\?xml.*>).*$/\1/" >> /root/project/docker/Unity_$UNITY_VERSION.alf