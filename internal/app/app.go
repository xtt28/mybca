package app

import (
	"errors"
	"net/http"

	"github.com/labstack/echo-contrib/echoprometheus"
	"github.com/labstack/echo/v4"
	"github.com/labstack/echo/v4/middleware"
	"github.com/labstack/gommon/log"
	"github.com/xtt28/mybca"
	"github.com/xtt28/mybca/internal/features/bus"
	"github.com/xtt28/mybca/internal/features/nutrislice"
	"github.com/xtt28/mybca/internal/handlers"
	"github.com/xtt28/mybca/internal/helpers"
	"github.com/xtt28/mybca/internal/model"
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
	lunchProvider  model.Provider[*nutrislice.MenuWeek]
	busProvider    model.Provider[bus.BusLocations]
	env            Env
}

func (a *App) registerRoutes() {
	a.echo.GET("/", (&handlers.Onboarding{BusProvider: a.busProvider}).GET())
	a.echo.GET("/a", (&handlers.AddToBrowser{}).GET())
	a.echo.GET("/h", (&handlers.Home{LunchProvider: a.lunchProvider, BusProvider: a.busProvider}).GET())
	a.echo.GET("/busapp", helpers.RedirectHandler(http.StatusMovedPermanently, "/busapp/"))

	busApp := a.echo.Group("/busapp")
	{
		busApp.GET("/", (&handlers.BusApp{BusProvider: a.busProvider}).GET())
		busApp.POST("/favorites", (&handlers.BusAppFavoriteAdd{}).GET())
		busApp.POST("/favorites/remove", (&handlers.BusAppFavoriteRemove{}).GET())
		busApp.Use(middleware.StaticWithConfig(middleware.StaticConfig{
			Filesystem: http.FS(mybca.StaticAssets),
		}))
	}
}

func (a *App) addMiddleware() {
	a.echo.Use(echoprometheus.NewMiddleware("mybca"))
	a.echo.Use(middleware.Logger())
	a.echo.Use(middleware.Recover())
}

func (a *App) setupPrometheusServer() {
	a.prometheusEcho = echo.New()
	a.prometheusEcho.GET("/metrics", echoprometheus.NewHandler())
	a.prometheusEcho.HideBanner = true
	a.prometheusEcho.Logger.SetLevel(log.INFO)
	a.prometheusEcho.Logger.Info("starting Prometheus server")

	go func() {
		if err := a.prometheusEcho.Start(":" + a.env.ServerPrometheusPort); err != nil && !errors.Is(err, http.ErrServerClosed) {
			a.echo.Logger.Fatal(err)
		}
	}()
}

func (a *App) Run() error {
	addr := a.env.ServerAppPort
	if addr == "" {
		a.echo.Logger.Warn("server will use random port as no address was specified in env")
	}
	return a.echo.Start(":" + a.env.ServerAppPort)
}

func New(
	lunchProvider model.Provider[*nutrislice.MenuWeek],
	busProvider model.Provider[bus.BusLocations],
	env Env,
) *App {
	a := &App{
		echo:          echo.New(),
		lunchProvider: lunchProvider,
		busProvider:   busProvider,
		env:           env,
	}
	a.echo.HideBanner = true
	a.echo.Logger.SetLevel(log.INFO)
	a.addMiddleware()
	a.registerRoutes()
	if env.ServerPrometheusPort != "" {
		a.setupPrometheusServer()
	}

	return a
}
