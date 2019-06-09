#!/bin/bash

dotnet publish -r win-x64 -c Release -f netcoreapp2.1 --self-contained
dotnet publish -r linux-x64 -c Release -f netcoreapp2.1 --self-contained
dotnet publish -r linux-arm -c Release -f netcoreapp2.1 --self-contained

