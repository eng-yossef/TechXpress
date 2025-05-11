# TechXpress - E-Commerce Platform

## 🌟 Overview
TechXpress is a sophisticated e-commerce platform built with ASP.NET Core MVC (.NET 8.0), offering a modern shopping experience for technology and electronics products with AI-powered features.

## 🚀 Core Features

### 💻 Customer Features
- **Product Management**
  - Advanced product search and filtering 
  - Category-based navigation
  - Product specifications
  - Rating and review system
  - Product image galleries
  - Related products suggestions

- **Shopping Experience**
  - Secure shopping cart
  - Wishlist functionality
  - Real-time stock updates
  - Multiple payment methods
  - Order tracking
  - Email notifications

- **User Account**
  - Profile management
  - Address book
  - Order history
  - Review management
  - Secure authentication

### 👨‍💼 Admin Features
- **Dashboard**
  - Sales analytics
  - Customer insights
  - Inventory tracking
  - Performance metrics

- **Product Management**
  - Product CRUD operations
  - Category management
  - Inventory control
  - Price management
  - Image handling

- **Order Management**
  - Order processing
  - Status updates
  - Payment tracking
  - Shipping integration

### 🤖 AI Features
- AI-powered product descriptions
- Customer sentiment analysis
- Personalized promotions
- Inventory predictions
- Smart chatbot assistant

## 🛠️ Technical Architecture

### Technology Stack
- **Backend**
  - ASP.NET Core MVC (.NET 8.0)
  - Entity Framework Core
  - SQL Server
  - Identity Framework

- **Frontend**
  - Bootstrap 5
  - jQuery
  - JavaScript
  - AJAX
  - Font Awesome

- **Additional Tools**
  - Stripe Payment Integration
  - Toastr Notifications
  - CKEditor
  - DataTables

### Project Structure
```
TechXpress/
├── TechXpress.Data/           # Data Layer
│   ├── Models/                # Entity Models
│   ├── Repositories/          # Data Access
│   └── UnitOfWork/           # Transaction Management
├── TechXpress.Services/       # Business Layer
│   ├── CartItemsService/      # Shopping Cart Logic
│   ├── OrdersService/         # Order Processing
│   ├── ProductsService/       # Product Management
│   └── CustomersService/      # Customer Management
└── TechXpress.Web/           # Presentation Layer
    ├── Areas/                 # Feature Areas
    │   └── Admin/            # Admin Dashboard
    ├── Controllers/          # Request Handling
    ├── Views/                # UI Templates
    └── wwwroot/             # Static Files
```

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022/VS Code
- Node.js (for frontend tools)

### Installation
1. Clone the repository
```bash
git clone https://github.com/eng-yossef/TechXpress.git
```

2. Update database connection in `appsettings.json`

3. Apply migrations
```bash
dotnet ef database update
```

4. Run the application
```bash
dotnet run
```

## 🔒 Security Features
- HTTPS enforcement
- CSRF protection
- SQL injection prevention
- Password hashing
- Role-based authorization
- Two-factor authentication

## 📱 Responsive Design
- Mobile-first approach
- Responsive navigation
- Touch-friendly interfaces
- Optimized images
- Flexible layouts

## 🎨 UI Features
- Dark/Light mode
- Toast notifications
- Loading indicators
- Image zoom
- Rating stars
- Form validation

## 📈 Analytics & Reporting
- Sales reports
- Inventory analytics
- Customer behavior tracking
- Performance metrics
- Export functionality

## 🔧 Configuration
Key files:
- `appsettings.json`: Application settings
- `Program.cs`: Startup configuration
- `_Layout.cshtml`: Main template

## 📞 Contact
For support or inquiries:
- Email: yossefkhaled551@gmail.com
- Phone: 01150648044

---
Built with ❤️ by the TechXpress Team
