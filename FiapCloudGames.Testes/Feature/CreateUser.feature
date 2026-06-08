Feature: Create User

  Scenario: Create a common user successfully
    Given a valid user request
    When I create the user
    Then the user should be created successfully
    And the response should contain user data