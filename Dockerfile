# ========================
# Stage 1: Build Angular frontend
# ========================
FROM node:20-alpine AS frontend-build
WORKDIR /app/frontend

# Copy package.json and install dependencies
COPY intergalactic-logistics-frontend/package*.json ./
RUN npm ci

# Copy all frontend files and build production
COPY intergalactic-logistics-frontend/ ./
RUN npm run build -- --configuration production

# ========================
# Stage 2: Build .NET backend
# ========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app



# Step 1: Copy solution file for caching
COPY IntergalacticLogisticsApp/IntergalacticLogisticsApp.sln ./

# Step 2: Copy individual .csproj files for caching
COPY IntergalacticLogisticsApp/IntergalacticLogisticsApp/*.csproj ./IntergalacticLogisticsApp/
COPY IntergalacticLogisticsApp/IntergalacticLogistics.Api/*.csproj ./IntergalacticLogistics.Api/
COPY IntergalacticLogisticsApp/IntergalacticLogistics.Application/*.csproj ./IntergalacticLogistics.Application/
COPY IntergalacticLogisticsApp/IntergalacticLogistics.Domain/*.csproj ./IntergalacticLogistics.Domain/
COPY IntergalacticLogisticsApp/IntergalacticLogistics.Infrastructure/*.csproj ./IntergalacticLogistics.Infrastructure/


# Step 3: Restore dependencies
RUN dotnet restore *.sln

# Step 4: Copy the rest of the backend source code
COPY IntergalacticLogisticsApp/ ./

# Step 5: Copy Angular build into API's wwwroot folder
RUN mkdir -p IntergalacticLogistics.Api/wwwroot
COPY --from=frontend-build /app/frontend/dist/intergalactic-logistics-frontend/browser ./IntergalacticLogistics.Api/wwwroot/

# Rename index.csr.html to index.html for .NET fallback
RUN mv IntergalacticLogistics.Api/wwwroot/index.csr.html IntergalacticLogistics.Api/wwwroot/index.html

# Step 6: Publish backend
RUN dotnet publish IntergalacticLogistics.Api/IntergalacticLogistics.Api.csproj \
    -c Release -o /out

# ========================
# Stage 3: Runtime
# ========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published backend + wwwroot from previous stage
COPY --from=backend-build /out ./

# Expose ports
EXPOSE 80
EXPOSE 443

# Start the API
ENTRYPOINT ["dotnet", "IntergalacticLogistics.Api.dll"]
