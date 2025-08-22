package bus_test

import (
	"testing"

	"github.com/stretchr/testify/suite"
	"github.com/xtt28/mybca/internal/bus"
)

type TownListTestSuite struct {
	suite.Suite
}

func (suite *TownListTestSuite) TestMockProvider() {
	prov := bus.NewMockProvider()
	towns, err := bus.GetTownList(prov)
	suite.Require().NoError(err)
	suite.Require().NotNil(towns)

	suite.Len(towns, 8)
	suite.ElementsMatch(towns, []string{"Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"})
}

func (suite *TownListTestSuite) TestFallback() {
	towns := bus.GetFallbackTownList()

	suite.Len(towns, 45)
	suite.ElementsMatch(towns, []string{
		"Allendale",
		"Alpine/Bergenfield",
		"Becton/Carlstadt/East Rutherford/Wood Ridge",
		"Cliffside Park/Fairview/Pal Park",
		"Closter/Demarest",
		"Cresskill/Dumont",
		"Elmwood Park",
		"Emerson/River Edge/Oradell",
		"Englewood/Englewood Cliffs",
		"Fair Lawn",
		"Fort Lee",
		"Franklin Lakes/Wyckoff BA9",
		"Franklin Lakes/Wyckoff BA10",
		"Garfield",
		"Glen Rock",
		"Hasbrouck Heights/Wallington",
		"Harrington Park/Haworth",
		"Hillsdale/River Vale",
		"Hohokus",
		"Leonia/Edgewater",
		"Little Ferry",
		"Lodi/Saddle Brook",
		"Lyndhurst/North Arlington",
		"Mahwah East BA6",
		"Mahwah West BA7",
		"Maywood/Rochelle Park",
		"Midland Park/Waldwick",
		"Montvale",
		"Moonachie/So Hackensack/Bogota",
		"New Milford",
		"Northvale/Norwood/Old Tappan",
		"Oakland/Fr Lakes/Wyck BA8",
		"Paramus East",
		"Paramus West",
		"Park Ridge/Woodcliff Lake/Hillsdale",
		"Ramsey",
		"Ridgefield",
		"Ridgefield Park",
		"Ridgewood",
		"Rutherford",
		"Saddle River",
		"Teaneck",
		"Tenafly",
		"Upper Saddle River",
		"Washington Township/Westwood",
	})
}

func TestTownListTestSuite(t *testing.T) {
	suite.Run(t, new(TownListTestSuite))
}
