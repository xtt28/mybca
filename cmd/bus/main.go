package main

import (
	"fmt"
	"os"

	"github.com/xtt28/mybca/internal/bus"
)

func main() {
	prov := bus.NewSheetProvider(bus.BCABusSheetURL)
	data, err := prov.Get()
	if err != nil {
		fmt.Println(err)
		os.Exit(1)
	}

	for k, v := range data {
		fmt.Printf("%s\t%s\n", k, v)
	}
}