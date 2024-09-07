# BattleshipAPI

BattleshipAPI is a RESTful API that implements the classic Battleship game logic. The API allows players to place ships on a grid and fire shots to try and sink their opponent's ships. The project is built using .NET 6 and follows a client-server (Three Tire) architecture pattern.

## Features

- Place ships of varying sizes on a 10x10 grid.
- Fire shots at a specific row and column to hit or miss enemy ships.
- Keep track of the shooting history.
- Detect when ships are sunk and the game is won.
- Handle invalid ship placements and invalid or same shot positions.

## Technologies Used

- **.NET 6**
- **C#**
- **MSTest** (for testing framework)

## Getting Started

### Prerequisites

- .NET 6 SDK or later
- Visual Studio 2022 or VS Code with C# extension

### HTTP Header

- Keep the global constant (ex: my-console-app) value for your application. 
- Add the custom 'X-consumer' HTTP header and that global value with each request. This value will be used to uniquely identify individual applications and is essential for handling game-specific data caching processes.

### API Endpoints

#### 1\. Place Ships

- **URL:** `/Ships/PlaceShips`
- **Method:** `GET`
- **Response:**

```json
{
  "message": "Ok",
  "success": true,
  "data": [
    {
      "shipType": 0,
      "shipName": "Battleship",
      "shipPositions": [
        {
          "row": 5,
          "column": 2,
          "direction": 1
        },
        {
          "row": 6,
          "column": 2,
          "direction": 1
        }
      ],
      "shipSize": 2,
      "health": 2,
      "isSunk": false
    }
  ]
}
```

#### 2\. Shoot at a Position

- **URL:** `/Shoots/ShootResult?row=5&column=2`
- **Method:** `GET`
- **Response:**

```json
{
  "message": "Ok",
  "success": true,
  "data": {
    "shootStatus": 3,
    "damagedShip": "Battleship",
    "shootHistory": [
      {
        "row": 5,
        "column": 3,
        "shootStatus": 3
      }
    ]
  }
}
```
## Support

Darshana Wijesinghe  
Email address - [dar.mail.work@gmail.com](mailto:dar.mail.work@gmail.com)  
Linkedin - [darwijesinghe](https://www.linkedin.com/in/darwijesinghe/)  
GitHub - [darwijesinghe](https://github.com/darwijesinghe)

## License

This project is licensed under the terms of the **MIT** license.
