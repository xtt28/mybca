# myBCA

## Deployment

The production deployment of this app is located at <https://mybca.link>.

## Features

### Buses

- myBCA scrapes the bus spreadsheet to determine the location of each bus at
  dismissal.
  - Bus locations are cached in a basic in-memory cache. The TTL for the cache
    is shortened during the dismissal time period.
- A bus list app is available at `/Bus/List`. It includes a PWA manifest and
  supports favorites.
- Bus information is also served via a REST API endpoint and the new tab page.

### Lunch

- myBCA uses the Nutrislice backend API to fetch lunch menus for the week.
  - Lunch menus are cached in a basic in-memory cache.
- Lunch information is served via a REST API endpoint and the new tab page.

### Quick Links

- Quick links to BCA-related services (PowerSchool, Schoology, etc.) are served
  in the new tab page and via a REST API endpoint.

### New Tab

- myBCA summarizes bus info, lunch menus, and quick links all into a single page
  which can be set as the user's browser new tab page for quick access.

## Usage

We use Docker Compose.

```shell
git clone https://github.com/xtt28/mybca
cd mybca
docker compose up --build -d
```

## Licenses

> [!WARNING]
> Multiple licenses apply to this repository.
> Please see the [LICENSE](LICENSE) file for details.