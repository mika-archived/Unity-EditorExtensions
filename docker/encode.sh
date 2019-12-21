#!/usr/bin/env bash
set -eu

UNITY_VERSION=2018.x

cat "./Unity_v$UNITY_VERSION.ulf" | base64 -w 0