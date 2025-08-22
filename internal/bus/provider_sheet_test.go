package bus

import (
	"testing"
	"time"

	"github.com/stretchr/testify/suite"
)

type ProviderSheetTestSuite struct {
	suite.Suite
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
