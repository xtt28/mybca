package nutrislice

import (
	"encoding/json"
	"fmt"
	"net/http"
)

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

	body := make([]byte, res.ContentLength)
	res.Body.Read(body)

	if res.StatusCode != http.StatusOK {
		return nil, NutrisliceAPIError{res.StatusCode, string(body)}
	}

	menuWeek := &MenuWeek{}
	err = json.Unmarshal(body, menuWeek)

	return menuWeek, err
}
