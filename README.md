# AspJwtBearerDemo

This simple .NET8 demo shows how to have both

* MVC controllers with cookie authentication
* API controllers with Jwt Bearer authentication

in a single app. 

Also, there are no other external dependencies other than the **Microsoft.AspNetCore.Authentication.JwtBearer** package.

Couple of details:

* the JWT authentication scheme is symmetric HmacSha256, the key is stored in settings
* the `Home/Index` and `Account/Logon` handle the MVC part, authentication cookie is issued in `Account/Logon`
* Jwt token is issued in `Token/Acquire`
* the `Home/Index` has two buttons
	* first button calls the `Token/Acquire` with the cookie and gets the Jwt token back
	* second button calls the `Api/Data` with the Jwt token and gets some api data back