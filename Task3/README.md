# API Documentation (Task 3)
|Method|URI|Parameters|Returns|
|---|---|---|---|---|
|**GET**|`/api/Values`|Bearer Token (header)|Hello, {Username}<br>HTTP 401 Unauthorized if no Token provided|
|**POST**|`/api/Account/Register`|Registration info (RegisterModel)<blockquote>**Mandatory** email (string)<br>**Mandatory** password (string)<br>**Mandatory** confirmPassword (string) **Matches password**</blockquote>|Register new user account<br>HTTP 200 on success<br>HTTP 400 Bad Request with error message|
|**POST**|`/connect/Token`|Login Info (JWT password grant)<blockquote>**Mandatory** grant_type (string)<br>**Mandatory** username (string)<br>**Mandatory** password (string)<br>**Mandatory** scope (string)<br>**Mandatory** client_id (string)<br>**Mandatory** client_secret (string)</blockquote>|Register new user account<br>HTTP 200 on success<br>HTTP 400 Bad Request with error message|
|**POST**|`/api/Account/Logout`|Bearer Token (header)|Sign out<blockquote>**Does not invalidate JWT Tokens server side!**<br>This is because JWT Tokens do not support revokation, though they can expire</blockquote>HTTP 200 OK on success<br>HTTP 401 Unauthorized if no Token provided|