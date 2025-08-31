package helpers

import "github.com/labstack/echo/v4"

func RedirectHandler(code int, target string) echo.HandlerFunc {
	return func(c echo.Context) error {
		return c.Redirect(code, target)
	}
}
