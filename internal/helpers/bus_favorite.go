package helpers

import (
	"fmt"
	"net/http"
	"slices"
	"strings"
	"time"

	"github.com/labstack/echo/v4"
)

const BusFavoritesCookieName = "mybca_busapp_favorites"

func GetFavoriteBuses(c echo.Context) ([]string, error) {
	favCookie, err := c.Cookie(BusFavoritesCookieName)
	if err == http.ErrNoCookie {
		return []string{}, nil
	}
	if err != nil {
		return []string{}, err
	}

	var favTownCommaSep string
	if favCookie != nil {
		favTownCommaSep = favCookie.Value
	}
	favTowns := strings.Split(favTownCommaSep, ",")
	if len(favTowns) == 1 && favTowns[0] == "" {
		return []string{}, nil
	} else {
		return favTowns, nil
	}
}

func SetFavoriteBuses(c echo.Context, favorites []string) {
	favsStr := strings.Join(favorites, ",")
	c.SetCookie(&http.Cookie{
		Name:     BusFavoritesCookieName,
		Value:    favsStr,
		Path:     "/",
		SameSite: http.SameSiteLaxMode,
		Expires:  time.Now().Add(20 * 365 * 24 * time.Hour),
		MaxAge:   20 * 365 * 24 * 60 * 60,
	})
}

func AddFavoriteBus(c echo.Context, town string) {
	favorites, _ := GetFavoriteBuses(c)

	favorites = append(favorites, town)
	SetFavoriteBuses(c, favorites)
}

func RemoveFavoriteBus(c echo.Context, town string) error {
	favs, err := GetFavoriteBuses(c)

	if err != nil {
		return err
	}

	fmt.Printf("removing %s\n", town)
	n := slices.DeleteFunc(favs, func(fav string) bool { return fav == town })

	SetFavoriteBuses(c, n)
	return nil
}
