#!/bin/bash

echo "Running migrations..."
dotnet ef database update

echo "Starting application..."
dotnet gau_blog.dll
