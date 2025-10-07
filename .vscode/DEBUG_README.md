# Debug Configuration for Assignment3

This workspace has been configured with debug settings for your C# project.

## Available Debug Configurations

### 1. Debug Console App (Program.cs)

- **Description**: Debug the main console application in `Program.cs`
- **Entry Point**: `Assignment3.Program.Main()`
- **Use Case**: Testing CategoryService functionality

### 2. Debug Server (ServerProgram.cs)

- **Description**: Debug the TCP server application in `ServerProgram.cs`
- **Entry Point**: `Assignment3.Server.ServerProgram.Main()`
- **Use Case**: Testing the EchoServer functionality
- **Port**: 5000

### 3. Attach to Process

- **Description**: Attach debugger to a running process
- **Use Case**: Debug already running applications

## Setup Instructions

If the debug configurations are not working properly:

1. **Install C# Extension** (if not already installed):

   - Press `Ctrl+Shift+X` to open Extensions
   - Search for "C#" by Microsoft
   - Install the "C#" extension

2. **Reload VS Code**:

   - Press `Ctrl+Shift+P`
   - Type "Developer: Reload Window"
   - Press Enter

3. **Alternative Debug Methods**:

   ```powershell
   # Run console app
   dotnet run --project Assignment3/Assignment3.csproj

   # Run server (modify project to use ServerProgram)
   dotnet build Assignment3/Assignment3.csproj /property:StartupObject=Assignment3.Server.ServerProgram
   dotnet run --project Assignment3/Assignment3.csproj
   ```

## Project Structure

- **Main Console App**: `Assignment3/Program.cs`
- **Server App**: `Assignment3/Server/ServerProgram.cs`
- **EchoServer Class**: `Assignment3/Server/Server.cs`
- **Models**: `Assignment3/Models/`
- **Services**: `Assignment3/Services/`
- **Tests**: `Assignment3TestSuite/`

## Build Tasks Available

- `build`: Build the main project
- `build-tests`: Build the test project
- `build-server`: Build project with ServerProgram as entry point
- `watch`: Build and watch for changes

## Usage

1. Press `F5` or go to Run and Debug panel (`Ctrl+Shift+D`)
2. Select desired configuration from dropdown
3. Click the green play button or press `F5`

## Troubleshooting

If debugger doesn't work:

- Check that the C# extension is installed and enabled
- Try reloading VS Code window
- Ensure project builds successfully: `dotnet build Assignment3/Assignment3.csproj`
- Check that .NET SDK is installed: `dotnet --version`
