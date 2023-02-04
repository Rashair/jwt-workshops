#!/bin/bash

dd if=/dev/urandom bs=124 count=1 status=none | base64
