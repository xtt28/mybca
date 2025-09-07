# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY MyBCA.Server/MyBCA.Server.csproj ./ 
RUN dotnet restore

# Copy the rest of the project
COPY . .

# Build the project
WORKDIR /src/MyBCA.Server
RUN dotnet publish -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app ./

# Expose the port your app runs on
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "MyBCA.Server.dll"]
