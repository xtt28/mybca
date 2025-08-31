package nutrislice

import "time"

type StationID uint

const (
	StationCreate StationID = 3085
	StationGrill  StationID = 3087
	StationVegOut StationID = 48840
)

type MenuWeek struct {
	StartDate   string    `json:"start_date"`
	DisplayName string    `json:"display_name"`
	Days        []MenuDay `json:"days"`
}

func (w *MenuWeek) GetTodayData() MenuDay {
	if len(w.Days) < int(time.Now().Weekday()) + 1 {
		return MenuDay{}
	}
	return w.Days[time.Now().Weekday()]
}

type MenuDay struct {
	Date      string     `json:"date"`
	MenuItems []MenuItem `json:"menu_items"`
}

type MenuItem struct {
	Date            time.Time `json:"date"`
	Position        int       `json:"position"`
	IsSectionTitle  bool      `json:"is_section_title"`
	Text            string    `json:"text"`
	Food            FoodItem  `json:"food"`
	StationID       StationID `json:"station_id"`
	IsStationHeader bool      `json:"is_station_header"`
	Image           string    `json:"image"`
	Category        string    `json:"category"`
}

type FoodItem struct {
	ID          int    `json:"id"`
	Name        string `json:"name"`
	Description string `json:"description"`
	Subtext     string `json:"subtext"`
	ImageURL    string `json:"image_url"`
}
