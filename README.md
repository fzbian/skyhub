# SkyHub

Backend API Monolithic image hosting and authentication built in .NET and MongoDB

## Table of Contents

- [Getting Started](#getting-started)
- [Usage](#usage)
- [Endpoints](#endpoints)
- [Contributing](#contributing)
- [License](#license)

## Getting Started

### Prerequisites

 - MongoDB
 - .NET 7.0
 - Cloudinry Account

### Installing

Clone the repository
```bash 
$ git clone https://github.com/fzbian/skyhub
```

Download the packages
```bash
$ dotnet restore
```

Config `.env` for database and cloudinary connection 

```bash
MONGODB_CONNECTION_STRING=
MONGODB_DATABASE=

CLOUDINARY_CLOUD_NAME=
CLOUDINARY_API_KEY=
CLOUDINARY_API_SECRET=

JWT_AUTH_SECRET=
JWT_AUTH_AUDIENCE=
JWT_AUTH_ISSUER=
```

Run program
```bash
$ dotnet run
```

## Usage

Use HTTP methods (GET, POST, PUT, DELETE) to interact with the API endpoints.

## Endpoints

## Authentication

### Login
- **Method:** `POST`
- **Endpoint:** `/api/Auth/login`
- **Tags:** `Auth`
- **Description:** Authenticate a user.
- **Request Body:**
  - `LoginModel` schema
- **Responses:**
  - `200 OK`: Successful authentication.

## Admin Operations

### Get All Users
- **Method:** `GET`
- **Endpoint:** `/api/Admin/GetAllUsers`
- **Tags:** `Admin`
- **Description:** Retrieve all users.
- **Responses:**
  - `200 OK`: Successful retrieval.

### Create User
- **Method:** `POST`
- **Endpoint:** `/api/Admin/CreateUser`
- **Tags:** `Admin`
- **Description:** Create a new user.
- **Request Body:**
  - `User` schema
- **Responses:**
  - `200 OK`: Successful user creation.

### Update User
- **Method:** `PUT`
- **Endpoint:** `/api/Admin/UpdateUser/{email}`
- **Tags:** `Admin`
- **Description:** Update a user by email.
- **Parameters:**
  - `email` (path)
- **Request Body:**
  - `User` schema
- **Responses:**
  - `200 OK`: Successful user update.

### Delete User
- **Method:** `DELETE`
- **Endpoint:** `/api/Admin/DeleteUser/{email}`
- **Tags:** `Admin`
- **Description:** Delete a user by email.
- **Parameters:**
  - `email` (path)
- **Responses:**
  - `200 OK`: Successful user deletion.

### Get User by Email
- **Method:** `GET`
- **Endpoint:** `/api/Admin/GetUserByEmail/{email}`
- **Tags:** `Admin`
- **Description:** Retrieve a user by email.
- **Parameters:**
  - `email` (path)
- **Responses:**
  - `200 OK`: Successful retrieval.

### Get All Images
- **Method:** `GET`
- **Endpoint:** `/api/Admin/GetAllImages`
- **Tags:** `Admin`
- **Description:** Retrieve all images.
- **Responses:**
  - `200 OK`: Successful retrieval.

## User Operations

### Upload Image
- **Method:** `POST`
- **Endpoint:** `/api/User/UploadImage`
- **Tags:** `User`
- **Description:** Upload an image for a user.
- **Parameters:**
  - `Email` (query)
  - `Password` (query)
- **Request Body:**
  - Multipart Form Data
- **Responses:**
  - `200 OK`: Successful image upload.

### Get Image
- **Method:** `GET`
- **Endpoint:** `/api/User/GetImage/{publicId}`
- **Tags:** `User`
- **Description:** Retrieve an image by public ID.
- **Parameters:**
  - `publicId` (path)
- **Responses:**
  - `200 OK`: Successful image retrieval.

### Delete Image
- **Method:** `DELETE`
- **Endpoint:** `/api/User/DeleteImage`
- **Tags:** `User`
- **Description:** Delete an image for a user.
- **Request Body:**
  - Multipart Form Data
- **Responses:**
  - `200 OK`: Successful image deletion.

### Get User Images
- **Method:** `GET`
- **Endpoint:** `/api/User/GetUserImages`
- **Tags:** `User`
- **Description:** Retrieve all images for a user.
- **Parameters:**
  - `Email` (query)
  - `Password` (query)
- **Responses:**
  - `200 OK`: Successful image retrieval.

## Health Check

### Database
- **Method:** `GET`
- **Endpoint:** `/api/Health/Database`
- **Tags:** `Database`
- **Description:** Check the database server's status.
- **Responses:**
  - `200 OK`: Database is accessible.

### Server
- **Method:** `GET`
- **Endpoint:** `/api/Health/Server`
- **Tags:** `Server`
- **Description:** Check the overall server status.
- **Responses:**
  - `200 OK`: Server is running successfully.

## Data Models

### Image
- Represents an image with properties like `id`, `publicId`, `url`, `userId`, `createdAt`, and `updatedAt`.

### LoginModel
- Represents the login model with properties `email` and `password`.

### ObjectId
- Represents an ObjectId with properties like `timestamp`, `machine`, `pid`, `increment`, and `creationTime`.

### User
- Represents a user with properties like `id`, `name`, `password`, `email`, `isAdmin`, `createdAt`, `updatedAt`, and `images`.

---

**Note:** Make sure to replace `{email}` and `{publicId}` with actual values when making requests.


## Contributing

Feel free to contribute to this project.

## License

This project is licensed under the [MIT license] - see the [LICENSE FILE](LICENSE) file for details.