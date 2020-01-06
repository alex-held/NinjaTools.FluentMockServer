#!/usr/bin/env bash

tag="$(git describe --abbrev=0 --exact-match --tags)"

if [[ $tag == *.*.*-* ]]
then
    echo "##vso[task.setvariable variable=prerelease;isOutput=true]true" 
	echo "git tag '$tag' is prerelease"
else
    echo "##vso[task.setvariable variable=prerelease;isOutput=true]false" 
	echo "git tag '$tag' is stable"
fi