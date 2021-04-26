#/bin/bash
grep -Po "\d*\.\d*\.\d*" README.md | tail -n 1
