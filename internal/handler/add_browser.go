package handler

import (
	"github.com/labstack/echo/v4"
	"github.com/xtt28/mybca/internal/components"
)

func AddToBrowser() echo.HandlerFunc {
	return func(c echo.Context) error {
		components.AddToBrowserPage().Render(c.Request().Context(), c.Response().Writer)
		return nil
	}
}
