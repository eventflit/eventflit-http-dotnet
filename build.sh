#!/bin/bash

# exit if any command fails
set -e

dotnet restore ./EventflitServer.Core/project.json

# Instead, run directly with mono for the full .net version
dotnet build ./EventflitServer.Core/project.json -c Release