package nutrislice

import (
	"fmt"
	"time"

	"github.com/xtt28/mybca/internal/provider"
)

type NutrisliceAPIProvider struct {
	urlFormat string
	data      *MenuWeek
	expiry    time.Time
}

const nutrisliceDataTTL = 24 * time.Hour

// Require compliance with interface.
var _ provider.Provider[*MenuWeek] = &NutrisliceAPIProvider{}

func (p *NutrisliceAPIProvider) Get() (*MenuWeek, error) {
	now := time.Now()
	if !p.expiry.IsZero() && now.Before(p.expiry) && p.data != nil {
		return p.data, nil
	}

	url := fmt.Sprintf(p.urlFormat, now.Year(), now.Month(), now.Day())
	data, err := getWeekData(url)
	if err != nil {
		return nil, err
	}

	p.data = data
	p.expiry = now.Add(nutrisliceDataTTL)

	return data, nil
}

func (p *NutrisliceAPIProvider) Expiry() *time.Time {
	return &p.expiry
}

func NewAPIProvider(apiURLFormat string) *NutrisliceAPIProvider {
	return &NutrisliceAPIProvider{urlFormat: apiURLFormat}
}
