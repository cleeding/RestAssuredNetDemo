# RestAssuredNetDemo

This project contains basic API tests written in C# using the [RestAssured.Net](https://github.com/rest-assured-net/RestAssured.Net) library and [xUnit](https://xunit.net/) test framework.

## Overview

The tests target the public JSONPlaceholder API (`https://jsonplaceholder.typicode.com`) and cover common CRUD operations:

- Get all posts
- Get posts by userId
- Create a new post
- Update a post
- Delete a post

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio Code or Visual Studio
- Internet connection (for hitting JSONPlaceholder API)

## Getting Started

1. Clone this repository:

   ```bash
   git clone https://github.com/yourusername/RestAssuredNetDemo.git
   cd RestAssuredNetDemo

2. Restore dependencies:

   ```bash
   dotnet restore

3. Run the tests:

   ```bash
   dotnet test


