package bus

import (
	"errors"

	"github.com/nfx/go-htmltable"
)

func getBusSheetData(sheetURL string) (BusLocations, error) {
	page, err := htmltable.NewFromURL(sheetURL)
	if err != nil {
		return nil, err
	}

	if page.Len() < 1 {
		return nil, errors.New("table not found on page")
	}
	table := page.Tables[0]
	rows := table.Rows[2:]

	locs := BusLocations{}

	for _, row := range rows {
		for i := 1; i < 5; i += 2 {
			if row[i] == "" {
				continue
			}
			locs[row[i]] = row[i+1]
		}
	}

	return locs, nil
}
