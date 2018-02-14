Feature: TestPressReleasesFeature
Check page 'Press Releases' for functional testing.

Background: Go to page with list of press-releases.
            Given I go to page with list of Press Releases

Scenario: To count press-releases on the list after click 'Load more'.
          When Click to button 'Load more'
	      Then The amount of press-releases on list less than max amount
		  		  
Scenario: Check title of press-release.
          When I get press-releases with title on the list
	      Then Title is not null for each press-releases

Scenario: Check link to image of press-releases.
          When I get press-releases with link to image
	      Then Link to image for each press-releases is correct

Scenario: Check announcement of press-release.
          When I get press-releases with announcement
	      Then Announcement is not null for each press-releases

Scenario: Check link to watch PDF of press-releases.
          When I get press-releases with link to watch PDF of press-releases
	      Then Link to watch PDF of press-release is correct

Scenario: Check link to download PDF of press-releases.
          When I get press-releases with link to download PDF of press-releases
	      Then Link to download PDF of press-release is correct

Scenario: Match title of press-release on the list and on the page of press-pelease.
          When I get press-releases with title on the list
		  And I get title of press-release on the page
	      Then Title of press-release on the list matches up title on the page of press-pelease

		  # Format of dateFrom and dateTo must matches up current culture.
Scenario Outline: Check filtering by date of press-releases.
		  When I have filtered press-releases by date start <dateFrom> and date end <dateTo>
		  Then Date of press-release on the list matches up range of date into filter
Examples:
| dateFrom	 | dateTo        |
| 01.12.2017 |  31.12.2017   |
| 01.01.2018 |  31.01.2018   |
| 01.02.2018 |  28.02.2018   |


		  