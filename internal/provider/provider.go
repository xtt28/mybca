package provider

import "time"

type Provider[T any] interface {
	Get() (T, error)
	Expiry() *time.Time
}