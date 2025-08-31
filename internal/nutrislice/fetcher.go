package nutrislice

import (
	"encoding/json"
	"fmt"
	"io"
	"net/http"
)

const BCANutrisliceURLFormat = "https://bergen.api.nutrislice.com/menu/api/weeks/school/bergen-academy/menu-type/lunch/%d/%d/%d"

type NutrisliceAPIError struct {
	Code int
	Body string
}

func (err NutrisliceAPIError) Error() string {
	return fmt.Sprintf("nutrislice api request failed: code %d: body %s\n", err.Code, err.Body)
}

// Require compliance with interface.
var _ error = NutrisliceAPIError{}

func getWeekData(apiURL string) (*MenuWeek, error) {
	res, err := http.Get(apiURL)
	if err != nil {
		return nil, err
	}
	defer res.Body.Close()

	body, err := io.ReadAll(res.Body)
	if err != nil {
		return nil, err
	}

	if res.StatusCode != http.StatusOK {
		return nil, NutrisliceAPIError{res.StatusCode, string(body)}
	}

	menuWeek := &MenuWeek{}
	err = json.Unmarshal(body, menuWeek)
	fmt.Printf("%+v\n", menuWeek)
	if err != nil { fmt.Println(err) }

	return menuWeek, err
}
