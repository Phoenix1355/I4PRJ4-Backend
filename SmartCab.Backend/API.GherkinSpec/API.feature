Feature: API
	In order to access and manipulate data
	As a customer/client
	I want to be able to manipulate/add data through the API endpoints

@mytag
Scenario: Register customer
	Given The server is onlines
	When I register a customer with email 'test12@gmail.com'
	Then the database should contain a customer with email 'test12@gmail.com'
