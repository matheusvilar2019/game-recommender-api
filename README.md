# GameRecommenderAPI
The objective is developer an API that helps recommend games to users, based on their tastes and technical limitations (category, platform and available RAM).

## Endpoint:
Here are the availables endpoints:
- `/recommender?category={category_name}`: Retrieve an recommended game from a specific category/genre.
- `/recommender?platform={platform_name}`: Retrieve an recommended game from a specific platform.
- `/games?ramMemory={your_ram}`: Retrieve an recommended game using your RAM as a minimum requirement

## Development server
- Clone the repository
  ```bash
   git clone https://github.com/matheusvilar2019/game-recommender-api.git
   ```
- Use the CLI command: 
  ```bash
   dotnet run
   ```
