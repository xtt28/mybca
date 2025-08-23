package main

import (
	"log"
	"os"

	"github.com/joho/godotenv"
	"github.com/xtt28/mybca/internal/app"
	"github.com/xtt28/mybca/internal/bus"
	"github.com/xtt28/mybca/internal/nutrislice"
	"github.com/xtt28/mybca/internal/provider"
)

func main() {
	err := godotenv.Load()
	if err != nil {
		log.Println("couldn't load .env file: ", err.Error())
	}

	env := app.Env{
		ServerAppPort:        os.Getenv("SERVER_APP_PORT"),
		ServerPrometheusPort: os.Getenv("SERVER_PROMETHEUS_PORT"),
		BusSheetURL:          os.Getenv("BUS_SHEET_URL"),
		NutrisliceAPIURL:     os.Getenv("NUTRISLICE_API_URL"),
	}

	var nutrisliceProvider provider.Provider[*nutrislice.MenuWeek]
	var busProvider provider.Provider[bus.BusLocations]

	if env.NutrisliceAPIURL == "" {
		log.Println("using Nutrislice mock provider as no API URL was given")
		nutrisliceProvider = nutrislice.NewMockProvider()
	} else {
		log.Printf("using Nutrislice provider at API URL %s\n", env.NutrisliceAPIURL)
		nutrisliceProvider = nutrislice.NewAPIProvider(env.NutrisliceAPIURL)
	}

	if env.BusSheetURL == "" {
		log.Println("using bus mock provider as no sheet URL was given")
		busProvider = bus.NewMockProvider()
	} else {
		log.Printf("using bus provider at sheet URL %s\n", env.BusSheetURL)
		busProvider = bus.NewSheetProvider(env.BusSheetURL)
	}

	app := app.New(nutrisliceProvider, busProvider, env)
	log.Fatal(app.Run())
}
