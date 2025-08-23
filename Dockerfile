# 1. Build stage
FROM golang:1.25-alpine AS builder

# Install git for module fetching
RUN apk add --no-cache git

# Set working directory inside container
WORKDIR /app

# Copy go.mod and go.sum first (for caching)
COPY go.mod go.sum ./

# Download dependencies
RUN go mod download

# Copy the rest of the source code
COPY . .

# Build the Go binary
RUN go build -o mybca-server ./cmd/server

# 2. Final stage
FROM alpine:latest

# Optional: add ca-certificates for HTTPS
RUN apk add --no-cache ca-certificates

# Add tzdata
RUN apk add --no-cache tzdata

WORKDIR /root/

# Copy binary from builder
COPY --from=builder /app/mybca-server .

# Expose default ports (optional, just for info; you can also rely on Compose)
# EXPOSE 1323 1324

# Run the binary
CMD ["./mybca-server"]
