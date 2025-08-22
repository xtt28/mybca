package nutrislice

import (
	"fmt"
	"time"

	"github.com/xtt28/mybca/internal/provider"
)

type NutrisliceMockProvider struct{}

// Require compliance with interface.
var _ provider.Provider[*MenuWeek] = &NutrisliceMockProvider{}

func (p *NutrisliceMockProvider) Get() (*MenuWeek, error) {
	now := time.Now()
	weekday := int(now.Weekday())
	daysSinceSunday := (weekday + 7) % 7
	sunday := now.AddDate(0, 0, -daysSinceSunday)
	sunday = time.Date(sunday.Year(), sunday.Month(), sunday.Day(), 0, 0, 0, 0, sunday.Location())

	week := &MenuWeek{
		StartDate: sunday.Format("2006-01-02"),
		Days:      make([]MenuDay, 7),
	}
	for i := range 7 {
		date := sunday.Add(time.Duration(i) * 24 * time.Hour)
		dateFormatted := date.Format("2006-01-02")
		day := MenuDay{Date: dateFormatted, MenuItems: make([]MenuItem, 9)}

		for j, station := range []StationID{StationCreate, StationGrill, StationVegOut} {
			day.MenuItems[j*3] = MenuItem{
				IsStationHeader: true,
				Text:            fmt.Sprintf("Sect. %d / %s", station, dateFormatted),
				StationID:       station,
			}
			day.MenuItems[j*3+1] = MenuItem{
				Food: FoodItem{
					Name:        fmt.Sprintf("%d 1. item %s", station, dateFormatted),
					Description: "Description",
				},
				Category:  "entree",
				StationID: station,
			}
			day.MenuItems[j*3+2] = MenuItem{
				Food: FoodItem{
					Name:        fmt.Sprintf("%d 2. item %s", station, dateFormatted),
					Description: "Description",
				},
				StationID: station,
			}
		}

		week.Days[i] = day
	}
	return week, nil
}

func (p *NutrisliceMockProvider) Expiry() *time.Time {
	return nil
}

func NewMockProvider() *NutrisliceMockProvider {
	return &NutrisliceMockProvider{}
}
