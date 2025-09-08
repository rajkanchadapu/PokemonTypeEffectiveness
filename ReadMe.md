# Pokemon Type Effectiveness Console App

A C# console application that uses the PokeAPI(https://pokeapi.co/) to determine a Pokemon's strengths and weaknesses against other types.
 
## Features
- Analyzes Pokemon type effectiveness (strengths/weaknesses)
- Handles single-type and dual-type Pokemon
- Error handling for invalid Pokemon names
- Unit tests included  

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Visual Studio 2022, Visual Studio Code, or any C# IDE
- Internet connection (for PokeAPI access)

### Clone Repository
``` 
git clone https://github.com/rajkanchadapu/PokemonTypeEffectiveness.git
cd PokemonTypeEffectiveness
```

### Build and Run
``` 
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the console application
cd .\PokemonTypeEffectiveness\ 
dotnet run
```

### Running Tests
``` 
# Run all unit tests
dotnet test  
``` 
---

## Usage

1. **Start the application**:
   ``` 
   dotnet run
   ```

2. **Enter a Pokemon name** when prompted:
   ```
   Enter a Pokemon name:
   ditto
   ```

3. **View the results**:
   ```
     Results for DITTO:

     Strengths:
     - ghost

     Weaknesses:
    - fighting
    - ghost
    - rock
 	- steel

   ```  

```
## Project Structure 
PokemonTypeEffectiveness/
├── Models/
│   ├── Pokemon.cs              # Pokemon data models
│   ├── TypeInfo.cs             # Type information model
│   ├── TypeRelations.cs        # Type relationship models
│   └── StrengthWeaknessResult.cs
├── Services/
│   ├── IPokemonApiClient.cs    # API client interface
│   ├── PokemonApiClient.cs     # HTTP client for PokemAPI
│   └── TypeEffectivenessService.cs # Core business logic
├── Tests/
│   └── TypeEffectivenessServiceTests.cs # Unit tests
├── Program.cs                  # Application entry point
└── README.md                   # This file
```

---

## API Reference

This application uses the [PokeAPI](https://pokeapi.co/) endpoints:
- `GET /pokemon/{name}` - Get Pokemon basic info and types
- `GET /type/{type-name}` - Get type effectiveness relationships

--- 
