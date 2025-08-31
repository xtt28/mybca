package handler

import (
	"slices"

	"github.com/labstack/echo/v4"
	"github.com/xtt28/mybca/internal/components"
	"github.com/xtt28/mybca/internal/features/bus"
	"github.com/xtt28/mybca/internal/model"
)

type OnboardingHandlerCtx struct {
	BusProvider model.Provider[bus.BusLocations]
}

func Onboarding(ctx *OnboardingHandlerCtx) echo.HandlerFunc {
	return func(c echo.Context) error {
		towns, err := bus.GetTownList(ctx.BusProvider)
		usingFallback := false
		if err != nil {
			c.Logger().Error(err)
			usingFallback = true
			towns = bus.GetFallbackTownList()
		}
		slices.Sort(towns)

		templCtx := components.OnboardingPageCtx{
			Towns:         towns,
			UsingFallback: usingFallback,
		}
		components.OnboardingPage(templCtx).Render(c.Request().Context(), c.Response().Writer)
		return nil
	}
}
