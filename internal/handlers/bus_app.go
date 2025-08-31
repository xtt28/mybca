package handlers

import (
	"net/http"
	"slices"
	"time"

	"github.com/labstack/echo/v4"
	"github.com/xtt28/mybca/internal/frontend/busapp/components"
	"github.com/xtt28/mybca/internal/features/bus"
	"github.com/xtt28/mybca/internal/helpers"
	"github.com/xtt28/mybca/internal/model"
)

type BusAppFavoriteAdd struct{}

func (*BusAppFavoriteAdd) GET() echo.HandlerFunc {
	return func(c echo.Context) error {
		town := c.FormValue("town")
		if town == "" {
			return c.String(http.StatusBadRequest, "Habibi you need to specify a town")
		}
		helpers.AddFavoriteBus(c, town)

		return c.Redirect(http.StatusSeeOther, "/busapp/")
	}
}

type BusAppFavoriteRemove struct{}

func (*BusAppFavoriteRemove) GET() echo.HandlerFunc {
	return func(c echo.Context) error {
		town := c.FormValue("town")
		if town == "" {
			return c.String(http.StatusBadRequest, "Habibi you need to specify a town")
		}
		helpers.RemoveFavoriteBus(c, town)

		return c.Redirect(http.StatusSeeOther, "/busapp/")
	}
}

type BusApp struct {
	BusProvider model.Provider[bus.BusLocations]
}

func (ctx *BusApp) GET() echo.HandlerFunc {
	return func(c echo.Context) error {
		busLocs, err := ctx.BusProvider.Get()
		if err != nil {
			c.Logger().Error(err)
			busLocs = bus.BusLocations{}
		}

		favTowns, _ := helpers.GetFavoriteBuses(c)

		entries := []components.BusEntry{}
		favorites := []components.BusEntry{}

		for key, val := range busLocs {
			if !slices.Contains(favTowns, key) {
				entries = append(entries, components.BusEntry{Town: key, Position: val})
			} else {
				favorites = append(favorites, components.BusEntry{Town: key, Position: val})
			}
		}

		comparator := func(a, b components.BusEntry) int {
			if a.Town < b.Town {
				return -1
			} else if a.Town > b.Town {
				return 1
			} else {
				return 0
			}
		}
		slices.SortFunc(favorites, comparator)
		slices.SortFunc(entries, comparator)

		templCtx := components.BusAppCtx{
			Greeting:  helpers.GetGreeting(time.Now()),
			Entries:   entries,
			Favorites: favorites,
			Expiry:    ctx.BusProvider.Expiry(),
		}

		components.BusApp(templCtx).Render(c.Request().Context(), c.Response().Writer)
		return nil
	}
}
