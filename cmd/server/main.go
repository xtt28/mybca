package main

import (
	"log"

	"github.com/xtt28/mybca/internal/app"
	"github.com/xtt28/mybca/internal/bus"
	"github.com/xtt28/mybca/internal/nutrislice"
)

func main() {
	app := app.New(":8080", nutrislice.NewAPIProvider(nutrislice.BCANutrisliceURLFormat), bus.NewSheetProvider(bus.BCABusSheetURL))
	log.Fatal(app.Run())
}
