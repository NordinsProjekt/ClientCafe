# ? Cozy Café - Client Application Project

A full-stack café management application built with C# .NET 10 and vanilla JavaScript. This project includes both a RESTful API backend and a frontend web application for managing products, orders, and shopping cart functionality.

## ?? Table of Contents

- [Project Structure](#project-structure)
- [Technologies](#technologies)
- [Getting Started](#getting-started)
- [Architecture](#architecture)
- [Known Issues](#known-issues)
- [Testing](#testing)
- [Contributing](#contributing)

## ??? Project Structure

```
ClientCafe/
??? ClientApi/              # Main API project
??? Entities/               # Domain models (Order, Product, OrderItem)
??? EntityFramework/        # Database context and configurations
??? Services/               # Business logic layer
??? Services.Tests/         # Unit tests for services
??? Requests/              # Request DTOs
??? CafePublicHtml/        # Frontend HTML/CSS/JS files (if present)
```

### Projects

| Project | Description | Framework |
|---------|-------------|-----------|
| **ClientApi** | ASP.NET Core Web API | .NET 10 |
| **Entities** | Domain entities and models | .NET 10 |
| **EntityFramework** | EF Core DbContext and migrations | .NET 10 |
| **Services** | Business logic services | .NET 10 |
| **Services.Tests** | Unit tests using xUnit | .NET 10 |
| **Requests** | Request/Response DTOs | .NET 10 |

## ??? Technologies

### Backend
- **.NET 10** - Latest .NET framework
- **C# 14.0** - Latest C# language features
- **Entity Framework Core** - ORM for database access
- **ASP.NET Core Web API** - RESTful API framework
- **xUnit** - Unit testing framework

### Frontend
- **HTML5**
- **CSS3** (Grid, Flexbox, Media Queries)
- **Vanilla JavaScript** - No frameworks required
- **Live Server** - Development server

## ?? Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Live Server Extension](https://marketplace.visualstudio.com/items?itemName=ritwickdey.LiveServer) for VS Code (for frontend)

### Backend Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/NordinsProjekt/ClientCafe.git
   cd ClientCafe
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database** (if using migrations)
   ```bash
   cd EntityFramework
   dotnet ef database update
   ```

4. **Run the API**
   ```bash
   cd ClientApi
   dotnet run
   ```
   The API will be available at `https://localhost:5001`

5. **Verify API is running**
   Open browser and navigate to: `https://localhost:5001/products`

### Frontend Setup

1. **Install Live Server Extension in VS Code**
   - Open VS Code
   - Go to Extensions (Ctrl+Shift+X)
   - Search for "Live Server" by Ritwick Dey
   - Click "Install"

2. **Open the frontend**
   - Open the `CafePublicHtml` folder in VS Code
   - Right-click on `index.html`
   - Select "Open with Live Server"
   - Website opens at `http://127.0.0.1:5500`

> **Note**: Live Server is required because opening HTML files directly (file:///) causes browsers to block API calls for security reasons.

## ??? Architecture

### Backend Architecture

```
???????????????
?  ClientApi  ?  ? Controllers & Program.cs
???????????????
       ?
???????????????
?  Services   ?  ? Business Logic (OrderService, ProductService)
???????????????
       ?
???????????????????
? EntityFramework ?  ? AppDbContext, Repositories
???????????????????
       ?
???????????????
?  Entities   ?  ? Domain Models (Order, Product, OrderItem)
???????????????
```

### Key Components

#### Entities
- `Product` - Product information
- `Order` - Customer orders
- `OrderItem` - Line items in orders

#### Request DTOs
- `CreateOrderRequest` - Request to create a new order
- `CreateOrderItemRequest` - Product and quantity for order items

#### Services
- `ProductService` - Product management logic
- `OrderService` - Order processing logic

## ?? Known Issues

This project contains intentional problems for educational purposes. Below are the issues to be resolved:

### ?? Critical Backend Bugs

#### Bug 1: Product Service Problem ??
- **Symptom**: GET `/products` endpoint returns incorrect data or throws exceptions
- **Location**: ProductService or Product Repository
- **Fix Required**: Review database calls, null-handling, and LINQ queries

#### Bug 2: Order Service Problem ??
- **Symptom**: POST `/orders` endpoint fails or creates incorrect orders
- **Location**: OrderService or Order Repository
- **Fix Required**: Review validation, database inserts, and relationship handling

### ?? Frontend Issues (HTML & CSS)

> **IMPORTANT**: JavaScript files (`products.js`, `product-detail.js`, `cart.js`, `api.js`) work correctly. Do NOT modify them!

#### Products Page (`products.html`)

1. **Poor Layout** ?? HIGH PRIORITY
   - Issue: Products display in single column instead of grid
   - Fix: Implement CSS Grid/Flexbox with 3+ columns

2. **Missing Price Formatting** ?? HIGH PRIORITY
   - Issue: Prices display without "kr" currency
   - Fix: Add CSS `::after` pseudo-element

3. **Missing Product Images** ?? MEDIUM PRIORITY
   - Issue: No product images displayed
   - Fix: Create CSS placeholder with `::before` pseudo-element

4. **Poor Button Styling** ?? MEDIUM PRIORITY
   - Issue: "View Details" and "Add to Cart" buttons lack styling
   - Fix: Apply `.btn` and `.btn-primary` styles

5. **No Responsive Design** ?? LOW PRIORITY
   - Issue: Poor mobile experience
   - Fix: Add media queries for mobile (max-width: 768px)

#### Product Detail Page (`product-detail.html`)

6. **No Two-Column Layout** ?? HIGH PRIORITY
   - Issue: Content stacks vertically
   - Fix: Implement 2-column layout (image left, info right)

7. **Ugly Image Placeholder** ?? MEDIUM PRIORITY
   - Issue: Unprofessional placeholder appearance
   - Fix: Improve with proper sizing (400px+), colors, border-radius

8. **Missing Price Formatting** ?? HIGH PRIORITY
   - Issue: Price lacks "kr" and prominence
   - Fix: Add `::after` with "kr", increase size (1.5rem+)

9. **Poor Button/Link Styling** ?? MEDIUM PRIORITY
   - Issue: "Add to Cart" button and "Back" link unstyled
   - Fix: Apply `.btn .btn-primary` styling

10. **Poor Quantity Selector** ?? LOW PRIORITY
    - Issue: Input field too small and unformatted
    - Fix: Add padding, border-radius, better styling

11. **No Responsive Design** ?? LOW PRIORITY
    - Issue: Poor mobile layout
    - Fix: Add media query for single column on mobile

## ? Testing

### Running Unit Tests

```bash
cd Services.Tests
dotnet test
```

### Manual Testing Checklist

- [ ] Product list displays in grid layout
- [ ] Prices show with "kr" suffix
- [ ] Product images/placeholders visible
- [ ] "View Details" navigates correctly
- [ ] Product details show in 2-column layout
- [ ] "Add to Cart" works and updates cart counter
- [ ] Cart page displays items correctly
- [ ] Order creation succeeds
- [ ] Responsive design works on mobile

### Testing Tools

1. **API Testing**: Use `test-api.html` for debugging API issues
2. **Browser DevTools**: Press F12 to inspect elements and test CSS
3. **Mobile View**: Toggle Device Toolbar in DevTools for responsive testing

## ?? Helpful Resources

### CSS Concepts Needed

- **CSS Grid**: `display: grid`, `grid-template-columns`, `gap`
- **Flexbox**: `display: flex`, `justify-content`, `align-items`
- **Pseudo-elements**: `::before`, `::after`, `content`
- **Media Queries**: `@media (max-width: 768px) { }`
- **Responsive Design**: Viewport units, percentage widths, mobile-first

### Development Tips

- ?? Use Developer Tools (F12) to inspect and test CSS live
- ?? Read comments in HTML/CSS files carefully
- ?? Compare with working pages (`index.html`, `cart.html`) for reference
- ?? Use `test-api.html` to debug API problems
- ?? Start with high-priority issues first
- ?? Work in pairs and help each other!

## ?? Contributing

This is an educational project. When fixing issues:

1. Create a feature branch
2. Fix one issue at a time
3. Test thoroughly
4. Commit with clear messages
5. Push to your branch

## ?? Notes

- **CORS is already configured** - API allows browser requests
- **JavaScript is correct** - Do not modify JS files
- All fixes should be in **HTML, CSS, and C# files only**

## ?? License

This is an educational project for school purposes.

## ?? Summary

**Total Issues to Fix:**
- ?? Critical: 2 backend bugs
- ?? High Priority: 5 frontend issues
- ?? Medium Priority: 5 frontend issues  
- ?? Low Priority: 4 frontend issues

**Total: 13 issues + 2 critical bugs**

---

Good luck! ?? Lycka till!
