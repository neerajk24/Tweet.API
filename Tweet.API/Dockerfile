# Use the official .NET 5 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# Set the working directory in the container to /app
WORKDIR /app

# Copy the .csproj and restore any dependencies (via dotnet restore)
COPY ["Tweet.API.csproj", "./"]
RUN dotnet restore

# Copy everything else and build the app
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official .NET 5 runtime image as the base image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:5.0

# Set the working directory in the container to /app
WORKDIR /app

# Expose port 80 for the application
EXPOSE 80

# Copy the build app from the build-env container to the current container
COPY --from=build-env /app/out .

# Define the entry point of the application
ENTRYPOINT ["dotnet", "Tweet.API.dll"]
