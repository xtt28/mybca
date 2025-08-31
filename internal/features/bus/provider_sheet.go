package bus

import (
	"log"
	"time"

	"github.com/xtt28/mybca/internal/helpers"
	"github.com/xtt28/mybca/internal/model"
)

const BCABusSheetURL = "https://docs.google.com/spreadsheets/d/1S5v7kTbSiqV8GottWVi5tzpqLdTrEgWEY4ND4zvyV3o/edit"

func getCacheTTL(t time.Time) time.Duration {
	timeDayFormatted := t.Format("15:04")
	if helpers.IsWithinTimePeriod(timeDayFormatted, "12:25", "12:50") || helpers.IsWithinTimePeriod(timeDayFormatted, "16:05", "16:30") {
		return 1 * time.Minute
	}

	return 5 * time.Minute
}

type BusSheetProvider struct {
	sheetURL string
	data     BusLocations
	expiry   time.Time
}

// Require compliance with interface.
var _ model.Provider[BusLocations] = &BusSheetProvider{}

func (p *BusSheetProvider) Get() (BusLocations, error) {
	now := time.Now()

	if !p.expiry.IsZero() && now.Before(p.expiry) && p.data != nil {
		return p.data, nil
	}

	log.Println("fetching new bus data")
	data, err := getBusSheetData(p.sheetURL)
	if err != nil {
		return nil, err
	}

	p.data = data
	p.expiry = now.Add(getCacheTTL(now))
	return data, nil
}

func (p *BusSheetProvider) Expiry() *time.Time {
	return &p.expiry
}

func NewSheetProvider(sheetURL string) *BusSheetProvider {
	return &BusSheetProvider{sheetURL: sheetURL}
}
