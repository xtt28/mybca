package app

import (
	"errors"
	"log"
	"net/http"

	"github.com/labstack/echo-contrib/echoprometheus"
	"github.com/labstack/echo/v4"
	"github.com/labstack/echo/v4/middleware"
	"github.com/xtt28/mybca/internal/bus"
	"github.com/xtt28/mybca/internal/handler"
	"github.com/xtt28/mybca/internal/nutrislice"
	"github.com/xtt28/mybca/internal/provider"
)

type Env struct {
	ServerAppPort        string
	ServerPrometheusPort string
	BusSheetURL          string
	NutrisliceAPIURL     string
}

type App struct {
	echo           *echo.Echo
	prometheusEcho *echo.Echo
	lunchProvider  provider.Provider[*nutrislice.MenuWeek]
	busProvider    provider.Provider[bus.BusLocations]
	env            Env
}

func (a *App) registerRoutes() {
	a.echo.GET("/", handler.Onboarding(&handler.OnboardingHandlerCtx{BusProvider: a.busProvider}))
	a.echo.GET("/a", handler.AddToBrowser())
	a.echo.GET("/h", handler.Home(&handler.HomeHandlerCtx{LunchProvider: a.lunchProvider, BusProvider: a.busProvider}))
}

func (a *App) addMiddleware() {
	a.echo.Use(echoprometheus.NewMiddleware("mybca"))
	a.echo.Use(middleware.Logger())
	a.echo.Use(middleware.Recover())
}

func (a *App) setupPrometheusServer() {
	go func() {
		a.prometheusEcho = echo.New()
		a.prometheusEcho.GET("/metrics", echoprometheus.NewHandler())
		a.prometheusEcho.HideBanner = true
		log.Println("starting prometheus server")
		if err := a.prometheusEcho.Start(":" + a.env.ServerPrometheusPort); err != nil && !errors.Is(err, http.ErrServerClosed) {
			log.Fatal(err)
		}
	}()
}

func (a *App) Run() error {
	addr := a.env.ServerAppPort
	if addr == "" {
		log.Println("server will use random port as no address was specified")
	}
	return a.echo.Start(":" + a.env.ServerAppPort)
}

func New(
	lunchProvider provider.Provider[*nutrislice.MenuWeek],
	busProvider provider.Provider[bus.BusLocations],
	env Env,
) *App {
	a := &App{
		echo:          echo.New(),
		lunchProvider: lunchProvider,
		busProvider:   busProvider,
		env:           env,
	}
	a.echo.HideBanner = true
	a.addMiddleware()
	a.registerRoutes()
	if env.ServerPrometheusPort != "" {
		a.setupPrometheusServer()
	}

	return a
}
