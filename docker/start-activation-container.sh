#!/usr/bin/env bash
set -eu

UNITY_VERSION=2018.4.14f1

docker run --rm -it \
  -e "UNITY_VERSION=$UNITY_VERSION" \
  -e "UNITY_USERNAME=$UNITY_USERNAME" \
  -e "UNITY_PASSWORD=$UNITY_PASSWORD" \
  -e "TEST_PLATFORM=linux" \
  -e "WORKDIR=/root/project" \
  -v "/$(pwd):/root/project" \
  -w '//root/project/docker' \
  gableroux/unity3d:$UNITY_VERSION \
  bash ./create-license-file.sh