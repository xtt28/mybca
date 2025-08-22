package app

import "github.com/labstack/echo/v4"

type App struct {
	echo *echo.Echo
	addr string
}

func (a *App) registerRoutes() {
	a.echo.GET("/", func(c echo.Context) error {
		return c.String(200, "hello")
	})
}

func (a *App) Run() error {
	return a.echo.Start(a.addr)
}

func New(addr string) *App {
	a := &App{
		echo: echo.New(),
		addr: addr,
	}
	a.echo.HideBanner = true
	a.registerRoutes()

	return a
}
