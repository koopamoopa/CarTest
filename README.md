# CarTest
Just a dumbed down personal notes:
- If DB isn't updating and I updated Seed.cs, make sure to delete carinfo.db, this is cause the following line:
        if (_context.Users.Any())
            return;  // DB already seeded
    this basically just sees if the file exists, if so then a database is being used. In simple terms it just allows testing to be done. 
    If I want to do proper testing then fuck around with it more :) remove the db during testing or use a temp.
- to start program, just do `dotnet run`


CHECKLIST:
- database DONE hopefully
- system arch
    - Login Controller (calls functions in AuthServ, mainly handling data conversion into transferable data (json))
    - Car Controller (calls functions in CarServ, mainly handling data conversion into transferable data (json))
    - AuthService
        - "Consider how you will authenticate users to support this, e.g. Cookies or JWT."
    - CarService (handles Car sql database manip / retrieval)
        - Add/remove car
        - List cars and stock levels
        - update car stock level
        - serach car by make and model


Stuff I need to consider
- Testing
    - Input isn't too large. Set a input length restriction, char restriction. Maybe don't allow particular characters that might be known to mess up system
        - '
        - other lang ?
        - make sure no queries are done as an input
        - Longest Model Name found online is up to <100 characters. To be safe there has been put a 150 character limit for Car Model.
        - Company Name has a 100 characters
        - Username and Password cannot exceed 50 characters
- Flexible input
    - Made it so any searches has it that the input is set to all lower, just so it isn't as annoying when it has been saved as case sensitive


DTO is add security, say theres a particular data you need to read, but there is private info (eg. password is shown in the account, but when querying for the account you dont want to return the actual password)
UserDto for returning user info (profile details, whatnot)

