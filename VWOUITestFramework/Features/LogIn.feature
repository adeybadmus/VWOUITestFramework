Feature: LogIn
	Simple calculator for adding two numbers

Background: 
	Given that VWO is loaded
	

Scenario: LogIn_01_Verify that user can login with a valid username and Password
	When a user fill valid Username and Password data
	And a user clicks on login button
	Then the user should login 


Scenario: LogIn_02_Verify that user can login for 30 days with a valid username and Password
	When a user fill valid Username and Password data
	And a user checks the remember me checkbox
	And a user clicks on login button


Scenario Outline: LogIn_03_Verify that user cannot login with an invalid credentials
	When a user fill Login page with <Username> and <Password> data
	And a user clicks on login button
	Then a error message <ExpectedErrorMessage> should be displayed
	Examples:
	| Username | Password | ExpectedErrorMessage                                       |
	| valid    | invalid  | Your email, password, IP address or location did not match |
	| invalid  | valid    | Your email, password, IP address or location did not match |
	|          | valid    | Your email, password, IP address or location did not match |
	| valid    |          | Your email, password, IP address or location did not match |
	| invalid  | invalid  | Your email, password, IP address or location did not match |
	| invalid  | invalid  | Your email, password, IP address or location did not match |
