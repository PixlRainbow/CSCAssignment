# API Documentation (Task 2)
## V1 API
| Method  | URI                     | Parameters                     | Returns                                              |
|---------|-------------------------|--------------------------------|------------------------------------------------------|
| **GET** | `/api/v1/products`      | None                           | Array of all Products                                |
| **GET** | `/api/v1/products/{id}` | Product ID (int) **Minimum 2** | Product matching given ID, or 404 if no such product |
## V2 API
| Method     | URI                                    | Parameters                                       | Returns                                                                                               |
|------------|----------------------------------------|--------------------------------------------------|-------------------------------------------------------------------------------------------------------|
| **GET**    | `/api/v2/products`                     | None                                             | Array of all Products                                                                                 |
| **GET**    | `/api/v2/products/{id}`                | Product ID (int)                                 | Product matching given ID<br>HTTP 404 if no such product                                              |
| **GET**    | `/api/v2/products?category={category}` | Category Name (string)                           | Array of Products matching given category                                                             |
| **POST**   | `/api/v2/products`                     | Product definition (Product)                     | Adds a new Product<br>HTTP 201 Created with `Location` header                                         |
| **PUT**    | `/api/v2/products/{id}`                | Product ID (int)<br>Product definition (Product) | Updates an existing Product<br>HTTP 204 No Content on success<br>HTTP 404 if no Product with given ID |
| **DELETE** | `/api/v2/products/{id}`                | Product ID (int)                                 | Deletes an existing Product<br>HTTP 204 No Content on success<br>HTTP 404 if no Product with given ID |
## V3 API
| Method     | URI                                    | Parameters                                                                                                                    | Returns                                                                                                  |
|------------|----------------------------------------|-------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------|
| **GET**    | `/api/v3/products`                     | None                                                                                                                          | Array of all Products                                                                                    |
| **GET**    | `/api/v3/products/{id}`                | Product ID (int)                                                                                                              | Product matching given ID<br>HTTP 404 if no such product                                                 |
| **GET**    | `/api/v3/products?category={category}` | Category Name (string)                                                                                                        | Array of Products matching given category                                                                |
| **POST**   | `/api/v3/products`                     | Product definition (Product)<blockquote>**Mandatory** name (string)<br>**Mandatory** price (int) **Maximum 100**</blockquote> | Adds a new Product<br>HTTP 201 Created with `Location` header<br>HTTP 400 Bad Request with error message |
| **PUT**    | `/api/v3/products/{id}`                | Product ID (int)<br>Product definition (Product)                                                                              | Updates an existing Product<br>HTTP 204 No Content on success<br>HTTP 404 if no Product with given ID    |
| **DELETE** | `/api/v3/products/{id}`                | Product ID (int)                                                                                                              | Deletes an existing Product<br>HTTP 204 No Content on success<br>HTTP 404 if no Product with given ID    |
