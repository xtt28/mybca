package bus

import (
	"testing"
	"time"

	"github.com/stretchr/testify/suite"
)

type ProviderSheetTestSuite struct {
	suite.Suite
}

func (suite *ProviderSheetTestSuite) TestIsWithinTimePeriod() {
	suite.True(isWithinTimePeriod("15:30", "9:30", "16:20"))
	suite.False(isWithinTimePeriod("09:00", "10:00", "21:00"))
	suite.True(isWithinTimePeriod("12:00", "11:00", "13:00"))
	suite.False(isWithinTimePeriod("12:00", "11:00", "1:00"))
}

func (suite *ProviderSheetTestSuite) mustParseTime(s string) time.Time {
	t, err := time.Parse("15:04", s)
	suite.Require().NoError(err)
	suite.Require().NotNil(t)

	return t
}

func (suite *ProviderSheetTestSuite) TestGetCacheTTL() {
	suite.Equal(5*time.Minute, getCacheTTL(suite.mustParseTime("15:00")))
	suite.Equal(1*time.Minute, getCacheTTL(suite.mustParseTime("12:40")))
	suite.Equal(1*time.Minute, getCacheTTL(suite.mustParseTime("16:15")))
	suite.Equal(5*time.Minute, getCacheTTL(suite.mustParseTime("4:15")))
}

func TestProviderSheetTestSuite(t *testing.T) {
	suite.Run(t, new(ProviderSheetTestSuite))
}
