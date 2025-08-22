package links

import (
	_ "embed"
	"encoding/json"
)

//go:embed links.json
var linksJSONRaw []byte

var links []Link

type Link struct {
	Name        string `json:"name"`
	Destination string `json:"dest"`
}

func GetLinks() []Link {
	if links != nil {
		return links
	}

	var target struct {
		Links []Link `json:"links"`
	}

	err := json.Unmarshal(linksJSONRaw, &target)
	if err != nil {
		panic(err)
	}

	links = target.Links
	return links
}
