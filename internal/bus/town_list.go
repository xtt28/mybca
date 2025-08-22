package bus

import (
	_ "embed"
	"maps"
	"slices"
	"strings"

	"github.com/xtt28/mybca/internal/provider"
)

//go:embed towns_fallback.txt
var townsFallbackFile string

func GetFallbackTownList() []string {
	return strings.Split(townsFallbackFile, "\n")
}

func GetTownList(prov provider.Provider[BusLocations]) ([]string, error) {
	locs, err := prov.Get()
	if err != nil {
		return nil, err
	}

	return slices.Collect(maps.Keys(locs)), nil
}
