# Cafe API - Debugging Exercise

## Overview
This is a Cafe API application built with ASP.NET Core that manages products and orders. However, there are **three bugs** in the code that you need to find and fix!

## Project Structure
- **ClientApi** - Web API with Swagger UI
- **Entities** - Domain models (Product, Order, OrderItem)
- **EntityFramework** - Database context and configuration
- **Services** - Business logic (ProductService, OrderService)
- **Requests** - Request models
- **Services.Tests** - xUnit tests (these will pass once you fix all bugs!)

## Getting Started

1. **Run the API**
   ```bash
   dotnet run --project ClientApi
   ```
   The browser should automatically open to Swagger UI at `https://localhost:5001`

2. **Run the Tests**
   ```bash
   cd Services.Tests
   dotnet test
   ```
   Currently, **8 tests are failing**. Your goal is to make all tests pass!

## The Bugs ??

There are **3 bugs** hidden in the Services layer:

### Bug #1: ProductService - GetAllProductsAsync
- **Symptom**: Products are returned with incorrect prices
- **Hint**: Check what happens to the price before returning the products

### Bug #2: ProductService - GetProductByIdAsync
- **Symptom**: The wrong product is returned
- **Hint**: Look at which ID is being used to query the database

### Bug #3: OrderService - CreateOrderAsync
- **Symptom**: The total amount is calculated incorrectly
- **Hint**: Check the math operation used in the calculation

## Your Task

1. **Find the bugs** using:
   - Visual Studio Debugger
   - Reading the test failures
   - Examining the code logic
   - Testing with Swagger UI

2. **Fix the bugs** in the appropriate service files

3. **Verify your fixes** by:
   - Running `dotnet test` - all tests should pass ?
   - Testing the API endpoints in Swagger UI
   - Using breakpoints to verify correct values

## Test Data

The database is seeded with these products:
- **Espresso** (ID: 1) - $2.50 - Stock: 100
- **Cappuccino** (ID: 2) - $3.50 - Stock: 100
- **Latte** (ID: 3) - $4.00 - Stock: 100

## Example Test Request

Create an order using Swagger UI:
```json
{
  "customerName": "John Doe",
  "items": [
    {
      "productId": 1,
      "quantity": 2
    },
    {
      "productId": 2,
      "quantity": 3
    }
  ]
}
```

**Expected result after fixes**:
- Total Amount: $15.50 (2.50 × 2 + 3.50 × 3)

## Success Criteria

? All 16 unit tests pass  
? Products show correct prices in GET /products  
? GET /products/{id} returns the correct product  
? Order total amounts are calculated correctly  

## Learning Objectives

- Practice using the debugger to trace code execution
- Understand how to read and interpret test failures
- Learn to identify logic errors in business calculations
- Experience fixing bugs in a multi-project solution

## Tips

- Use **breakpoints** to step through the code
- Check the **test output** to see expected vs actual values
- Compare the **service implementation** with the **test expectations**
- Use Swagger UI to manually test your fixes

Good luck! ??
