package app

import (
	"github.com/labstack/echo/v4"
	"github.com/xtt28/mybca/internal/bus"
	"github.com/xtt28/mybca/internal/handler"
	"github.com/xtt28/mybca/internal/nutrislice"
	"github.com/xtt28/mybca/internal/provider"
)

type App struct {
	echo          *echo.Echo
	addr          string
	lunchProvider provider.Provider[*nutrislice.MenuWeek]
	busProvider   provider.Provider[bus.BusLocations]
}

func (a *App) registerRoutes() {
	a.echo.GET("/", handler.Onboarding(&handler.OnboardingHandlerCtx{BusProvider: a.busProvider}))
	a.echo.GET("/a", handler.AddToBrowser())
	a.echo.GET("/h", handler.Home(&handler.HomeHandlerCtx{LunchProvider: a.lunchProvider, BusProvider: a.busProvider}))
}

func (a *App) Run() error {
	return a.echo.Start(a.addr)
}

func New(
	addr string,
	lunchProvider provider.Provider[*nutrislice.MenuWeek],
	busProvider provider.Provider[bus.BusLocations],
) *App {
	a := &App{
		echo:          echo.New(),
		addr:          addr,
		lunchProvider: lunchProvider,
		busProvider:   busProvider,
	}
	a.echo.HideBanner = true
	a.registerRoutes()

	return a
}
