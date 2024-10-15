# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj and restore as distinct layers
COPY ["StudentsMinimalApi.csproj", "./"]
RUN dotnet restore "StudentsMinimalApi.csproj"

# Copy the remaining source code and build the application
COPY . .
RUN dotnet publish "StudentsMinimalApi.csproj" -c Release -o /app/publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# (Optional) Create a non-root user for security
# RUN adduser --disabled-password --no-create-home appuser
# USER appuser

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port 5000
EXPOSE 5000

# Define the entry point
ENTRYPOINT ["dotnet", "StudentsMinimalApi.dll"]
