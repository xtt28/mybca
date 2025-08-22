package helpers

import "time"

const GreetingGoodMorning = "Good morning."
const GreetingGoodAfternoon = "Good afternoon."
const GreetingGoodEvening = "Good evening."

func GetGreeting(t time.Time) string {
	timeDayFormatted := t.Format("15:04")
	if IsWithinTimePeriod(timeDayFormatted, "05:00", "12:00") {
		return GreetingGoodMorning
	} else if IsWithinTimePeriod(timeDayFormatted, "11:59", "18:00") {
		return GreetingGoodAfternoon
	} else {
		return GreetingGoodEvening
	}
}