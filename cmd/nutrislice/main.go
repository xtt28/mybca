package main

import (
	"fmt"
	"slices"
	"time"

	"github.com/xtt28/mybca/internal/features/nutrislice"
)

func main() {
	prov := nutrislice.NewMockProvider()
	data, _ := prov.Get()
	day := data.Days[time.Now().Weekday()]
	fmt.Println(day)

	for _, item := range day.MenuItems {
		if !slices.Contains([]nutrislice.StationID{
			nutrislice.StationCreate,
			nutrislice.StationGrill,
			nutrislice.StationVegOut,
		}, item.StationID) {
			continue
		}

		if item.IsSectionTitle || item.IsStationHeader {
			fmt.Printf("\n== %s ==\n", item.Text)
			continue
		}
		food := item.Food
		if item.Category != "entree" && item.Category != "meat" {
			fmt.Print("\t")
		}
		fmt.Println(food.Name)
	}
}
