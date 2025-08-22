package helpers_test

import (
	"testing"

	"github.com/stretchr/testify/suite"
	"github.com/xtt28/mybca/internal/helpers"
)

type TimeTestSuite struct {
	suite.Suite
}

func (suite *TimeTestSuite) TestIsWithinTimePeriod() {
	suite.True(helpers.IsWithinTimePeriod("15:30", "9:30", "16:20"))
	suite.False(helpers.IsWithinTimePeriod("09:00", "10:00", "21:00"))
	suite.True(helpers.IsWithinTimePeriod("12:00", "11:00", "13:00"))
	suite.False(helpers.IsWithinTimePeriod("12:00", "11:00", "1:00"))
}

func TestProviderSheetTestSuite(t *testing.T) {
	suite.Run(t, new(TimeTestSuite))
}
