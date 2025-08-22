package handler

import (
	"net/http"
	"time"

	"github.com/labstack/echo/v4"
	"github.com/xtt28/mybca/internal/bus"
	"github.com/xtt28/mybca/internal/components"
	"github.com/xtt28/mybca/internal/nutrislice"
	"github.com/xtt28/mybca/internal/provider"
)

type HomeHandlerCtx struct {
	LunchProvider provider.Provider[*nutrislice.MenuWeek]
	BusProvider   provider.Provider[bus.BusLocations]
}

func Home(ctx *HomeHandlerCtx) echo.HandlerFunc {
	return func(c echo.Context) error {
		town := c.QueryParam("town")
		busLocs, err := ctx.BusProvider.Get()
		if err != nil {
			return err
		}

		loc, ok := busLocs[town]
		if !ok {
			return echo.NewHTTPError(http.StatusNotFound, "town not found")
		}

		menuWeek, err := ctx.LunchProvider.Get()
		if err != nil {
			return err
		}

		todayItems := menuWeek.GetTodayData().MenuItems
		templCtx := components.HomePageCtx{
			Town:            town,
			Now:             time.Now(),
			BusLocation:     loc,
			BusExpiryTime:   ctx.BusProvider.Expiry(),
			Greeting:        "Hello there.",
			LunchMenuItems:  todayItems,
			LunchExpiryTime: ctx.LunchProvider.Expiry(),
		}

		components.HomePage(templCtx).Render(c.Request().Context(), c.Response().Writer)
		return nil
	}
}
