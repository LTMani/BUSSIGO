# BUSSIGO: South India Bus & Travel Empire Simulator Makefile

.PHONY: all install build run test audit verify clean

all: test run

install:
	pip install -r requirements.txt
	npm install

build:
	python Assets/Tools/build_webgl_local.py

run:
	python main.py 8080

test:
	python Assets/Tools/test_runner.py

audit:
	python Assets/Tools/loc_audit.py

verify:
	python Assets/Tools/verify_environment.py

clean:
	@echo "Cleaning temporary files..."
