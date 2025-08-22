package handler

import (
	"github.com/labstack/echo/v4"
	"github.com/xtt28/mybca/internal/bus"
	"github.com/xtt28/mybca/internal/components"
	"github.com/xtt28/mybca/internal/provider"
)

type OnboardingHandlerCtx struct {
	BusProvider provider.Provider[bus.BusLocations]
}

func Onboarding(ctx *OnboardingHandlerCtx) echo.HandlerFunc {
	return func(c echo.Context) error {
		towns, err := bus.GetTownList(ctx.BusProvider)
		if err != nil {
			return err
		}

		templCtx := components.OnboardingPageCtx{
			Towns: towns,
		}
		components.OnboardingPage(templCtx).Render(c.Request().Context(), c.Response().Writer)
		return nil
	}
}
