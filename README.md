# Ticket Management
 
Unfortunately the commit history has been lost.

# How run project

Publish TicketManagement.Database project

Change the DefaultConnection field in the appsettings.json file in the following projects
- TicketManagement.VenueApi\appsettings.json
- TicketManagement.EventApi\appsettings.json
- TicketManagement.PurchaseApi\appsettings.json
- TicketManagement.UserApi\appsettings.json

Select properties in the solution and select the following projects to run
- TicketManagement.VenueApi
- TicketManagement.EventApi
- TicketManagement.PurchaseApi
- TicketManagement.UserApi
- TicketManagement.UserInterface

# How to run integration tests

Publish TicketManagement.Database project


# Used technologes

- MS SQL .dacpac project to deploy database
- Entity Framework to database access
- Fluent assertion for unit and integration tests
- Jwt token for user authorization. Configured with middleware
- Identity Framework for managment user data
- MOQ for unit testing
- Serilog for logging
- StyleCop and SonarAnalyzer for code analysis