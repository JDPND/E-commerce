#!/bin/bash

echo "🧪 Running tests..."
dotnet test

RESULTS_DIR="bin/Debug/net6.0/allure-results"
REPORT_DIR="allure-report"

if [ -d "$RESULTS_DIR" ]; then
    echo "📊 Generating Allure report..."
    allure generate "$RESULTS_DIR" --clean -o "$REPORT_DIR"
    echo "🌐 Opening Allure report..."
    allure open "$REPORT_DIR"
else
    echo "❌ No allure-results found at $RESULTS_DIR"
fi
