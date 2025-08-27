# myBCA

myBCA is the ultimate new tab page for BCA students. You can see your bus
location, read the lunch menu for the day, and access common links for BCA
students, all within one convenient page.

## Production deployment

At the moment, there is no official deployment of this software.

## Features

### External data

- Shows daily lunch data, sourced from Nutrislice REST API
- Shows bus location data scraped from BCA bus location Google Sheet
- Server-side caching of data sourced from external providers

### User interface

- Includes shortcuts to PowerSchool, Schoology, teacher absence listing, BCA
  dashboard, and counselor booking site
- Includes search box for fast Google search

### Deployment/instrumentation

- Serves metrics at Prometheus metrics endpoint
- Docker Compose application includes Prometheus and Grafana services

## Usage

### Setting up your environment

This project uses Docker Compose. You will need Docker.

Clone the repository and enter the application directory:
```shell
git clone https://github.com/xtt28/mybca.git
cd mybca
```

Rename the <template.env> file to ".env" and insert your desired values into it.

### Running

Run with Docker Compose:
```shell
docker compose up -d
```

### Accessing Grafana

Grafana server will be served at the port specified in `GRAFANA_PORT`
environment variable. The username and password are specified by environment
variables `GF_SECURITY_ADMIN_USER` and `GF_SECURITY_ADMIN_PASSWORD`.

Due to the Grafana provisioning setup, there will already be a dashboard ready
for you when you log in.

## License

This project is licensed under:

    SPDX-License-Identifier: AGPL-3.0-or-later

being in concordance with the terms in the LICENSE file in the root of this
repository.