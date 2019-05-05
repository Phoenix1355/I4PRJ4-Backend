Feature: Api
	In order to distrubute a service
	As a owner
	I want to be able to test the backend separetely

@mytag
Scenario: Register Customer
	Given The server is online
	When I register a customer with email 'test12@gmail.com'
	Then the database contains a customer with email 'test12@gmail.com'
