Aareon Technical Test - Ticketing System

We need you to extend the current ticketing system that will allow additional resources to be included.

1. Implement endpoint(s) that will allow all CRUD actions on a Ticket.
Implemented as /Tickets with a DI service class to perform the business logic and database access

2. Now amend the solution to allow the addition of Notes to a Ticket.
Amended by adding a Notes property to the Ticket Model for speed of implementation. In a real-life 
system I would consider adding a Notes model backed by a Notes table in the database and then have
the possibility of keeping a history deleted/amended notes or multiple notes per Ticket etc.

3. Amend the solution to track any data manipulation, actions need to be tracked.
Here I have chosen to add logging support (Ilogger) for this, I have just added one call to 
LogInformation() to demonstrate. There are other ways to achieve tracking, auditing and 
monitoring/alerting. Filters can be considered for logging on the WebAPI controllers, 
Triggers can be considered in the database.

4. Create a Pull Request on github.
I cannot do this becase the test was supplied as a .ZIP file from a DropBox account

Requirements:
- A note is created by a Person to log additional information against a ticket.
This is supported by POST -> /Tickets

- A note can be created, updated, or removed by anyone, but only an Administrator may delete an existing note.
Separate routes for POST/PUT/DELETE to /Tickets/{id}/Person{personId}/Notes have been added to the Tickets Controller
Similar routes could have been added to the /Person endpoint
A new endpoint for /Notes could have been created

- Any actions that are taken against records in the ticketing system are subject to monitoring and auditing.
See my comments about ILogger. 

- This application will be deployed automatically using a CI/CD pipeline.
Yes and usually with unit tests, code quality and coverage tests

These tasks should take no longer than 4 hours

Here are my thoughts:
=====================

Obviously there are other ways to accomplish solutions but with just a 4 hour target I didn't want to get 
stuck coding a complex and custom solution and then not be able to provide something that works.

There is code that looks very similar for the Ticket and Person services, this could be written as a Generic 
class for the (DRY) repeated parts.

The controllers have many 'if' tests to establish the correct ActionResult, this could be hidden away inside
another service that 'knows' about HTTP/RESTful and perhaps prepares hyperlinks etc. Again for speed I have
put this code in the controllers.

Unit testing with a dependency on DbContext is troublesome, Microsoft acknowledge this themselves
(https://docs.microsoft.com/en-us/ef/core/testing/). I started to code an in-memory DbContext and ran into
configuration problems. Testing WebAPI controllers is also not easy, In the past I have used WebClient and also
SpecFlow to integration test controllers where the whole processing pipeline is used so that model validation,
Authorisation and Authentication, etc can work.

I saw that a 'ProblemDetails' class was available, if I knew more about it I might have tried to use it for any
errors reported by the controllers but I was not clear on its purpose so I left it alone.

The simple addition of a Note for a Ticket is interesting, REST allows for more than one 'representation' of
information and so the Note could appear under the Person and Ticket resources, it could even have its own
endpoint, the obvious simple choice is for a Note to be part of a Ticket in this coding test but I acknowledge
that Notes could be implemented in other ways and could be a model of their own and a table of Notes in the
database, Tickets could have multiple Notes and/or a Note history, Notes could be deleted by setting a deleted
flag rather than wiped clean and audited.

I enjoyed the test, the implementation and requirements are simple and this leads to many possibilities in
the solutions that candidates present. For myself, given the time, I have tried to keep it simple to keep
it working and my testing has been limited to exercising the API through the Swagger UI. In a production
system I would expect to have produced more unit and integration tests.




