# CarTest
Made by Clarence O'Toole
Last Updated: 25/08/2025

## Dependencies
Ran on Visual Studio 2022
Using the following packages:
- BCrypt.Net-Next Version 4.0.3
- Dapper Version 2.1.66
- Microsoft.AspNetCore.Authentication.JwtBearer Version 8.0.19
- Microsoft.EntityFrameworkCore.Design Version 9.0.8
- Microsoft.EntityFrameworkCore.Sqlite Version 9.0.8
- Swashbuckle.AspNetCore Version 6.6.2

## Project Execution & Test Cases
Clone the repository to your local device and open it on your preferred IDE (personally I used Visual Studio 2022).

Before executing, if you want a clean database that's set to the default values (default values listed below), remove th "carinfo.db" file.

To run the program, open a terminal and redirect it to the ProjectCarTest folder, then run "dotnet run -- seeddata".

When a valid user is logged in, it will return a token in the response body. Copy this token and paste it into the Swagger "Authorize" button then in the "Value" text field.

## Project Structure
Has the following folders:
- Controllers: Handles HTTP requests and responses for a given functionality of the web API.
- Data: Connects the code to the defined database.
- Dto: To restrict and protect sensitive information by controlling the data that leaves my API.
- Interfaces: Defines methods without implementing them, aka the middleman.
- Models: The Database Blueprints.
- Repository: Handles database interactions for specific API functions.
- Utilities: Contains a Helper class to have global access to any commonly used methods across the project.
And the following files:
- Program.cs: What runs the program.
- Seed.cs: Sets up the schema for the database and fills it with default values
- TestCases.md: A file that explains the execution and expected result for each test case.

## Database Design

### Users Table

| userID | username    | password                                           | companyName          |
| ------ | ----------- | -------------------------------------------------- | -------------------- |
| 1      | audi\_owner | Pass1234\$                                         | Audi Lovers Inc.     |
| 2      | thebmw      | b7M#@w13                                           | BMW Enthusiasts Ltd. |
| 3      | mercWork    | xRZdRqi8mMPb74fkly0d5sPgtTZe6o3rxEirw1ellIcyE5EkGd | Mercedes Fans Co.    |

Filled the users with different valid usernames and passwords, with user 1 being the main focus of the Test cases, and 2 being the minimum character count for a password and user 3 being the maximum character count for a password.


### CarInfos Table

| carID | userID | make          | model   | year | stockLevel |
| ----- | ------ | ------------- | ------- | ---- | ---------- |
| 1     | 1      | Audi          | A4      | 2018 | 3          |
| 2     | 1      | Audi          | A4      | 2019 | 5          |
| 3     | 1      | Audi          | Q5      | 2019 | 4          |
| 4     | 2      | BMW           | X5      | 2020 | 5          |
| 5     | 2      | BMW           | X6      | 2013 | 4          |
| 6     | 3      | Mercedes      | C-Class | 2021 | 2          |
| 7     | 3      | Mercedes-Benz | A-Class | 2019 | 7          |
| 8     | 1      | Audi          | A6      | 2020 | 2          |

Uses userID as a foreign key to allow identification of who owns which car.

## Specific Design Choices
### Why I chose specific default parameters / limitors
The max length for a username and password is set to 50 each, with the additional valid characters including "A-Z, a-z, 0-9, @ & $ ! # ? - _". This creates a combination of `1.65 x 10^92` amount of passwords.

For the Car Info, the default parameters as follows:
- private int parameterMaxLength = 100; 
- private int maxStockCount = 999999;
- private int minYear = 1855;
- private int currentYear = DateTime.Now.Year;

This allows them to be modified if they want to be changed at a later stage of development to reach new requirements. 
In particular, minYear is set to 1855 as the first car was made in that year, whilst cars probably don't even exist that date back to then, just as a temporary minimum for now it was deemed as a safe number.
The currentYear also considers whatever the year is currently by not hard coding it so it dynamically updates upon a new year occurring.

All input parameters have a max length set in both Car Info and Login to ensure the user does not overload the input with an extremely long input, which may be used for malicious intent to overload the database with a query.