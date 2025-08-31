package handlers

import (
	"time"

	"github.com/labstack/echo/v4"
	"github.com/xtt28/mybca/internal/features/bus"
	"github.com/xtt28/mybca/internal/features/nutrislice"
	"github.com/xtt28/mybca/internal/frontend/newtab/components"
	"github.com/xtt28/mybca/internal/helpers"
	"github.com/xtt28/mybca/internal/model"
)

type Home struct {
	LunchProvider model.Provider[*nutrislice.MenuWeek]
	BusProvider   model.Provider[bus.BusLocations]
}

func (ctx *Home) GET() echo.HandlerFunc {
	return func(c echo.Context) error {
		town := c.QueryParam("town")
		busLocs, err := ctx.BusProvider.Get()
		var loc string
		if err != nil {
			c.Logger().Error(err)
			loc = "ERROR (could not get from BusProvider)"
		} else {
			var ok bool
			loc, ok = busLocs[town]
			if !ok {
				loc = "ERROR (not found)"
			}
		}

		menuWeek, err := ctx.LunchProvider.Get()
		if err != nil {
			menuWeek = &nutrislice.MenuWeek{}
		}

		todayItems := menuWeek.GetTodayData().MenuItems
		now := time.Now()
		templCtx := components.HomePageCtx{
			Town:            town,
			Now:             now,
			BusLocation:     loc,
			BusExpiryTime:   ctx.BusProvider.Expiry(),
			Greeting:        helpers.GetGreeting(now),
			LunchMenuItems:  todayItems,
			LunchExpiryTime: ctx.LunchProvider.Expiry(),
		}

		components.HomePage(templCtx).Render(c.Request().Context(), c.Response().Writer)
		return nil
	}
}
