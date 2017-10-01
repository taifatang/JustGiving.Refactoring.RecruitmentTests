@GiftAid @Calculator
Feature: GiftAidCalculator
	Calcualte GiftAid value base on donation amount and event type

Scenario Outline: Calculate Gift aid with different tax rates
	Given I want to donate 100
	And Tax rate is <taxRate>
	When I calculate gift aid amount
	Then <giftAidAmount> is returned

Examples: 
	| taxRate | giftAidAmount |
	| 1       | 1.01          |
	| 10      | 11.11         |
	| 20      | 25.00         |
	| 30      | 42.86         |
	| 99      | 9900          |

Scenario Outline: Calculate gift aid with different event type at 10% tax rate
	Given I want to donate 1000
	And Tax rate is 10
	And Donation type is <typeName>
	When I calculate gift aid amount
	Then <giftAidAmount> is returned

Examples: 
	 | typeName | giftAidAmount |
    | swimming | 114.44      |
     | running  | 116.67    |
	  | default  | 111.11     |

