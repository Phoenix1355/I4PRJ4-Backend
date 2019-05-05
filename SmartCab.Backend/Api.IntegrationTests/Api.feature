Feature: Api
	In order to distrubute a service
	As a owner
	I want to be able to test the backend separetely

@mytag
Scenario: Register Customer
	Given The server is online
	When I have registered a customer with email 'test12@gmail.com'
	Then the database contains a customer with email 'test12@gmail.com'

Scenario: Login on customer
	Given The server is online
	When I have registered a customer with email 'test12@gmail.com'
	Then I can login with the email 'test12@gmail.com'


Scenario: Create ride for customer
	Given The server is online
	And I have registered a customer with email 'test12@gmail.com'
	When I have logged in with email 'test12@gmail.com'
	And I have deposited 1000 kr
	Then I can create a ride