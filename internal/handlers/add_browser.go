package handlers

import (
	"github.com/labstack/echo/v4"
	"github.com/xtt28/mybca/internal/frontend/newtab/components"
)

type AddToBrowser struct{}

func (*AddToBrowser) GET() echo.HandlerFunc {
	return func(c echo.Context) error {
		components.AddToBrowserPage().Render(c.Request().Context(), c.Response().Writer)
		return nil
	}
}
