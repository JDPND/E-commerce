#!/bin/bash

echo "🧪 Running tests..."
dotnet test

RESULTS_DIR="bin/Debug/net6.0/allure-results"
REPORT_DIR="Report"

if [ -d "$RESULTS_DIR" ]; then
    echo "📊 Generating single-file Allure report..."
    allure generate "$RESULTS_DIR" --single-file --clean -o "$REPORT_DIR"
    
    INDEX="$REPORT_DIR/index.html"
    if [ -f "$INDEX" ]; then
        echo "🌐 Opening report: $INDEX"
        xdg-open "$INDEX" 2>/dev/null || open "$INDEX" || start "$INDEX"
    else
        echo "⚠️ Report not found at $INDEX"
    fi
else
    echo "❌ No allure-results found at $RESULTS_DIR"
fi
