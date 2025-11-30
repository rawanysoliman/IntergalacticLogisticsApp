# Intergalactic Logistics App

A Star Wars-themed intergalactic shipment management system built with Clean Architecture, .NET 8, and Angular.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

- **Domain**: Core business entities and domain logic
- **Application**: Use cases, commands, queries, and interfaces
- **Infrastructure**: External services, persistence (Marten/PostgreSQL), and SWAPI integration
- **Api**: ASP.NET Core Web API with Wolverine for message handling

## 🚀 Quick Start

### Prerequisites

- .NET 8 SDK
- Node.js 18+ and npm
- Docker and Docker Compose (for database)
- PostgreSQL 14+ (if not using Docker)

### Option 1: Using Docker Compose (Recommended)

1. **Start the database and API:**
   ```bash
   docker-compose up -d
   ```

2. **Run the frontend:**
   ```bash
   cd intergalactic-logistics-frontend
   npm install
   npm start
   ```

3. **Access the application:**
   - Frontend: http://localhost:4200
   - API: https://localhost:5001
   - Swagger: https://localhost:5001/swagger

### Option 2: Manual Setup

1. **Set up PostgreSQL:**
   ```bash
   # Create database
   createdb IntergalacticLogistics
   ```

2. **Update connection string** in `IntergalacticLogistics.Api/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=IntergalacticLogistics;Username=postgres;Password=yourpassword"
     }
   }
   ```

3. **Run the backend:**
   ```bash
   cd IntergalacticLogistics.Api
   dotnet run
   ```

4. **Run the frontend:**
   ```bash
   cd intergalactic-logistics-frontend
   npm install
   npm start
   ```


## 🎯 Features

### Backend
- **Clean Architecture**: Separated into Domain, Application, Infrastructure, and API layers
- **Wolverine**: Message handling for Commands and Queries
- **Marten**: Document persistence with PostgreSQL
- **Strategy Pattern**: Modular shipping cost calculation (StandardSpeed, HyperdriveExpress, SmugglerRoute)
- **Decorator Pattern**: Cross-cutting concerns (cargo validation)
- **SWAPI Integration**: Fetches starship and planet data from Star Wars API
- **Structured Logging**: Serilog with request correlation
- **Global Error Handling**: RFC 7807 ProblemDetails responses

### Frontend
- **Angular (Latest)**: Standalone components exclusively
- **Signals**: State management with `signal`, `computed`, and `effect`
- **BEM CSS**: Block Element Modifier naming convention
- **Bootstrap**: Layout and styling
- **Reactive Forms**: Signal-based form validation
- **Starships Page**: View available starships
- **Book Shipment Form**: Create shipments with validation

## 🔧 Configuration

### Backend Configuration



**CORS**: Configured for `http://localhost:4200` and `https://localhost:4200`

### Frontend Configuration

**API URL** (`starship.service.ts`):
```typescript
private readonly apiUrl = 'https://localhost:5001/api';
```

Update this if your API runs on a different port.

## 🧪 Testing

### Backend API Testing

Use Swagger UI at `https://localhost:5001/swagger` 

## 📊 Shipping Cost Calculation

The system uses the **Strategy Pattern** to calculate shipping costs:

1. **Base Cost**: `CargoWeight × RatePerKg`
   - StandardSpeed: 10 credits/kg
   - HyperdriveExpress: 20 credits/kg
   - SmugglerRoute: 2 credits/kg

2. **Hyperdrive Extra Fee**: Added if `hyperdrive_rating > 2`
   - StandardSpeed: +50 credits
   - HyperdriveExpress: +100 credits
   - SmugglerRoute: +25 credits

## 🐳 Docker Compose

The `docker-compose.yml` file includes:
- **PostgreSQL**: Database for Marten document storage
- **API**: ASP.NET Core Web API

To start:
```bash
docker-compose up -d
```

To stop:
```bash
docker-compose down
```

To view logs:
```bash
docker-compose logs -f
```

## 🛠️ Development

### Backend Development

```bash
# Restore packages
dotnet restore

# Build
dotnet build

# Run
cd IntergalacticLogistics.Api
dotnet run
```

### Frontend Development

```bash
cd intergalactic-logistics-frontend

# Install dependencies
npm install

# Start development server
npm start

# Build for production
npm run build
```

## 📝 Key Design Patterns

1. **Strategy Pattern**: Shipping cost calculation strategies
2. **Decorator Pattern**: Cargo validation decorator
3. **Repository Pattern**: Shipment data access abstraction
4. **Factory Pattern**: Strategy selection factory






## 🔍 Troubleshooting

### Hyperdrive fees not being added

1. Check that `swapiId` is being sent from frontend (check browser console)
2. Verify backend logs show starship lookup and hyperdrive rating
3. Ensure SWAPI is accessible and returning starship data

### Database connection issues

1. Verify PostgreSQL is running
2. Check connection string in `appsettings.json`
3. Ensure database exists: `createdb IntergalacticLogistics`

### CORS errors

1. Verify API CORS configuration in `Program.cs`
2. Check frontend API URL matches allowed origins
3. Ensure credentials are handled correctly

## 📚 Technologies

- **Backend**: .NET 8, ASP.NET Core, Wolverine, Marten, Serilog
- **Database**: PostgreSQL
- **Frontend**: Angular (Latest), TypeScript, RxJS, Bootstrap
- **External API**: SWAPI (Star Wars API)

## 📄 License

This project is for demonstration purposes.

## 👥 Contributing

This is a learning/demonstration project showcasing Clean Architecture, SOLID principles, and modern .NET/Angular patterns.

