package helpers

import "time"

func IsWithinTimePeriod(subject string, first string, last string) bool {
	subTime, _ := time.Parse("15:04", subject)
	firstTime, _ := time.Parse("15:04", first)
	lastTime, _ := time.Parse("15:04", last)

	return firstTime.Before(subTime) && lastTime.After(subTime)
}
