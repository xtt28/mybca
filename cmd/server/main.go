package main

import (
	"log"

	"github.com/xtt28/mybca/internal/app"
)

func main() {
	app := app.New(":8080")
	log.Fatal(app.Run())
}