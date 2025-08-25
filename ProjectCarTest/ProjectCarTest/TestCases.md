# Tests
Before testing, ensure that if there is an existing file in the root called "carinfo.db" to remove, so it can generate a new database with default values for testing.

## Login
### Username
 - Fail and throws Code 401 Error: Unauthorized when invalid character entered (example: "*")
 - Fail and throws Code 401 Error: Unauthorized when input does not exist (example: "")
 - Fail and throws Code 401 Error: Unauthorized when input is over character limit of 50 (example: "iZAEXUSCrUa062p8QUVC8taQOG59DlIcyE5EkGdGHSJWIRKTLPM")
 - Fail and throws Code 401 Error: Unauthorized when input has a space (example: "my username")
### Password
 - Fail and throws Code 401 Error: Unauthorized when invalid character entered (example: "%")
 - Fail and throws Code 401 Error: Unauthorized when input does not exist (example: "")
 - Fail and throws Code 401 Error: Unauthorized when input is over character limit of 50 (example: "xRZdRqi8mMPb74fkly0d5sPgtTZe6o3rxEirw1ellIcyE5EkGdG")
 - Fail and throws Code 401 Error: Unauthorized when input has a space (example: "my password")
### Should pass for:
 - if username and password matches an account and should not give the user's password as a response (example: username="audi_owner", password="Pass1234$")
 - if username and password matches an account and password is exactly 50 characters (example: username="mercWork", password="xRZdRqi8mMPb74fkly0d5sPgtTZe6o3rxEirw1ellIcyE5EkGd")

## CarInfo
Note, this requires you to have the token for a particular user.
These tests will take the position of UserID=1, which is the username="audi_owner", password="Pass1234$".
Once you enter the valid log in details, in the Response body copy the "token" text only, then at the top of the Swagger UI there should be an "Authorize" button, enter the token as the value.

### CarInfo/user
 - Check the execution has the following response body (should not include Token, UserID) and no other car
[
  {
    "carID": 1,
    "make": "Audi",
    "model": "A4",
    "year": 2018,
    "stockLevel": 3
  },
  {
    "carID": 2,
    "make": "Audi",
    "model": "A4",
    "year": 2019,
    "stockLevel": 5
  },
  {
    "carID": 3,
    "make": "Audi",
    "model": "Q5",
    "year": 2019,
    "stockLevel": 4
  },
  {
    "carID": 8,
    "make": "Audi",
    "model": "A6",
    "year": 2020,
    "stockLevel": 2
  }
]
### Filter by Make
 - Search for make="Audi" or "audi" gives
[
  {
    "carID": 1,
    "make": "Audi",
    "model": "A4",
    "year": 2018,
    "stockLevel": 3
  },
  {
    "carID": 2,
    "make": "Audi",
    "model": "A4",
    "year": 2019,
    "stockLevel": 5
  },
  {
    "carID": 3,
    "make": "Audi",
    "model": "Q5",
    "year": 2019,
    "stockLevel": 4
  },
  {
    "carID": 8,
    "make": "Audi",
    "model": "A6",
    "year": 2020,
    "stockLevel": 2
  }
]
 - Fail and throws code 400 Error: Bad Request when Search for invalid character entered (example: make="%")
 - Fail and throws code 400 Error: Bad Request when Search exceeds character limit (example: make="xRZdRqi8mMPb74fkly0d5sPgtTZe6o3rxEirw1ellIcyE5EkGdxRZdRqi8mMPb74fkly0d5sPgtTZe6o3rxEirw1ellIcyE5EkGda")
 - Returns empty set when Search for not owned car (example: make="BMW")
### Filter by Model
 - Search for model="A4" or "a4" gives:
[
  {
    "carID": 1,
    "make": "Audi",
    "model": "A4",
    "year": 2018,
    "stockLevel": 3
  },
  {
    "carID": 2,
    "make": "Audi",
    "model": "A4",
    "year": 2019,
    "stockLevel": 5
  }
]
 - Fail and throws code 400 Error: Bad Request when Search for invalid character entered (example: model="%")
 - Fail and throws code 400 Error: Bad Request when Search exceeds character limit (example: model="xRZdRqi8mMPb74fkly0d5sPgtTZe6o3rxEirw1ellIcyE5EkGdxRZdRqi8mMPb74fkly0d5sPgtTZe6o3rxEirw1ellIcyE5EkGda")
 - Returns empty set when Search for not owned car (example: model="C-Class")
### Filter by Model and Make
 - Search for make="Audi" and model="A4" gives:
[
  {
    "carID": 1,
    "make": "Audi",
    "model": "A4",
    "year": 2018,
    "stockLevel": 3
  },
  {
    "carID": 2,
    "make": "Audi",
    "model": "A4",
    "year": 2019,
    "stockLevel": 5
  }
]
 - Fail and throws code 400 Error: Bad Request when Search for invalid character entered (example: make or model="%")
 - Fail and throws code 400 Error: Bad Request when Search exceeds character limit (example: make or model="xRZdRqi8mMPb74fkly0d5sPgtTZe6o3rxEirw1ellIcyE5EkGdxRZdRqi8mMPb74fkly0d5sPgtTZe6o3rxEirw1ellIcyE5EkGda")
 - Returns empty set when Search for not owned car (example: make="BMW", model="C-Class")
### Add New Car
 - Add a valid new entry which appears on Car List (/api/CarInfo/user)
Input in add:
{
  "make": "Audi",
  "model": "Q6",
  "year": 2013,
  "stockLevel": 5
}
Car list output:
[
 ...
 {
  "carID": 9,
  "make": "Audi",
  "model": "Q6",
  "year": 2013,
  "stockLevel": 5
 }
]
 - Fail if Make or Model contains illegal characters, or exceeds defined character limit, year is within 1855-present, and stock level is between 0-defined range limit
### Delete Car
 - Delete Car that Was Added in Car Add
 {
  "carID": 9
 }
 - Fail to delete unowned Car
  {
  "carID": 4
  }
### Update Stock for Existing Car
 - Updates Stock when input is within 0-defined range limit (example: carID="1", stockLevel=5) and CarList now appears
   [
    {
    "carID": 1,
    "make": "Audi",
    "model": "A4",
    "year": 2018,
    "stockLevel": 5
    },
    ...]
 - Does not update Stock when trying to modify unowned car (example: carID="4", stockLevel=10)
 - Does not update Stock when input is outside 0-defined range limit (example: carID="1", stockLevel=-1)
### Token Invalid
 - Change token to be blank, any query should throw "Token is missing or invalid."
 - Change token to be a non-existing key example "a", any query should throw "Token is missing or invalid."