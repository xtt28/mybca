package app

import (
	"errors"
	"log"
	"net/http"

	"github.com/labstack/echo-contrib/echoprometheus"
	"github.com/labstack/echo/v4"
	"github.com/labstack/echo/v4/middleware"
	"github.com/xtt28/mybca"
	"github.com/xtt28/mybca/internal/features/bus"
	"github.com/xtt28/mybca/internal/features/nutrislice"
	"github.com/xtt28/mybca/internal/handler"
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
	a.echo.GET("/", handler.Onboarding(&handler.OnboardingHandlerCtx{BusProvider: a.busProvider}))
	a.echo.GET("/a", handler.AddToBrowser())
	a.echo.GET("/h", handler.Home(&handler.HomeHandlerCtx{LunchProvider: a.lunchProvider, BusProvider: a.busProvider}))
	a.echo.GET("/busapp", func(c echo.Context) error {
		return c.Redirect(http.StatusMovedPermanently, "/busapp/")
	})

	busApp := a.echo.Group("/busapp")
	{
		busApp.GET("/", handler.BusApp(&handler.BusAppHandlerCtx{BusProvider: a.busProvider}))
		busApp.POST("/favorites", handler.BusAppFavoriteAdd())
		busApp.POST("/favorites/remove", handler.BusAppFavoriteRemove())
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
	a.addMiddleware()
	a.registerRoutes()
	if env.ServerPrometheusPort != "" {
		a.setupPrometheusServer()
	}

	return a
}
