# BUSSIGO 3D Simulator Container
FROM python:3.11-slim

WORKDIR /app

# Copy dependency manifests
COPY requirements.txt requirements.lock ./
RUN pip install --no-cache-dir -r requirements.txt

# Copy application source code
COPY . .

# Expose WebGL server port
EXPOSE 8080

# Run entry point
CMD ["python", "main.py", "8080"]
