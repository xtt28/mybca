package bus

import (
	"time"

	"github.com/xtt28/mybca/internal/provider"
)

type BusMockProvider struct{}

// Require compliance with interface.
var _ provider.Provider[BusLocations] = &BusMockProvider{}

func (p *BusMockProvider) Get() (BusLocations, error) {
	return map[string]string{
		"Mercury": "A1",
		"Venus":   "B2",
		"Earth":   "C3",
		"Mars":    "D4",
		"Jupiter": "E5",
		"Saturn":  "F6",
		"Uranus":  "G7",
		"Neptune": "H8",
	}, nil
}

func (p *BusMockProvider) Expiry() *time.Time {
	return nil
}

func NewMockProvider() *BusMockProvider {
	return &BusMockProvider{}
}
