# GameRecommenderAPI (PT-BR)
O objetivo é desenvolver uma API que auxilie na recomendação de jogos para usuários, com base em seus gostos e limitações técnicas (gênero, plataforma e memória RAM disponível).

## 🔧 Como rodar o projeto localmente

#### 1. Clone o repositório:

``` bash
git clone https://github.com/matheusvilar2019/game-recommender-api.git
cd game-recommender-api
```

#### 2. Restaure os pacotes:
``` bash
dotnet restore
```

#### 3. Execute as migrations e crie o banco de dados:
``` bash
dotnet ef database update
```
> 💡 A API utiliza uma instância local do SQL Server. A string de conexão está definida diretamente no arquivo `Data/GameRecommenderDataContext.cs`. Certifique-se de que o SQL Server esteja rodando localmente com as configurações padrão.

#### 4. Inicie a API:
``` bash
dotnet run
```

#### 5. Acesse no navegador:
``` bash
http://localhost:5008/swagger/index.html
```
> 💡 Nota: A API será executada em uma porta dinâmica como `https://localhost:5001` ou `https://localhost:5008`, dependendo do seu sistema. Depois de executar o `dotnet run`, verifique a saída do terminal para confirmar a porta correta.

## Endpoint:
### Game Recommender

`GET /v1/recommender`

#### Parâmetros (query string):

| Parâmetro     | Obrigatório | Descrição                                  |
|---------------|-------------|--------------------------------------------|
| `category`    | ✅ Sim       | Gênero do jogo (ex: `Shooter`, `MMORPG`)  |
| `platform`    | ❌ Opcional  | Plataforma `pc`, `browser` ou `all`       |
| `ramMemory`   | ❌ Opcional  | Memória RAM disponível (em GB, ex: `8`)   |

- `/recommender?category={category_name}`: Retorna um jogo recomendado de uma categoria específica.
É possível enviar mais de uma categoria, por exemplo: `/recommender?category=mmorpg.shooter`
- `/recommender?platform={platform_name}`: Retorna um jogo recomendado para uma plataforma específica.
- `/games?ramMemory={your_ram}`: Retorna um jogo recomendado considerando a RAM disponível como requisito mínimo.

#### Exemplo:
GET `/v1/recommender?category=Shooter&platform=pc&ramMemory=8`

#### Status Code 200:
```json
{
  "title": "Call of Duty: Warzone",
  "freetogameProfileUrl": "https://www.freetogame.com/game/cod-warzone"
}
```

#### Respostas Possíveis:

- 200: Success
- 400: RCR-105 - Invalid platform. Use: pc, browser or all
- 400: RCR-105 - Invalid RAM.
- 400: RCR-105 - Category is required.
- 400: RCR-105 - Invalid category. Use: mmorpg, shooter, moba, anime, battle royale, strategy, fantasy, sci-fi, card games, racing, fighting, social, sports.
- 404: RCR-101 - Game not found. Check parameters.
- 502: RCR-103 - API call failed
- 500: RCR-104 - Internal server error


## Endpoint:
### All Games Recommended

`GET /v1/all-games-recommended`

#### Exemplo:
GET `/v1/all-games-recommended`

#### Status Code 200:
```json
[
    {
        "id": 1,
        "title": "Deceit 2",
        "category": "Action",
        "counter": 1
    },
    {
        "id": 2,
        "title": "War Thunder",
        "category": "Shooter",
        "counter": 2
    },
    {
        "id": 3,
        "title": "Mecha BREAK",
        "category": "Shooter",
        "counter": 1
    },
]
```
> O `counter` mostra quantas vezes este jogo foi recomendado.

#### Respostas Possíveis:

- 200: Success
- 204: No Content
- 500: AGR-101 - Internal server error

### 🛠 Tecnologias

```markdown
## Tecnologias utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Swagger / Swashbuckle
- HttpClient
- SQL Server
```


# GameRecommenderAPI (EN)
The objective is developer an API that helps recommend games to users, based on their tastes and technical limitations (category, platform and available RAM).

## How to run the project locally

#### 1. Clone the repository:

``` bash
git clone https://github.com/matheusvilar2019/game-recommender-api.git
cd game-recommender-api
```

#### 2. Restore the packages:
``` bash
dotnet restore
```

#### 3. Run the migrations and create the database:
``` bash
dotnet ef database update
```
> 💡 The API uses a local SQL Server instance. The connection string is currently hardcoded in `Data/GameRecommenderDataContext.cs`. Make sure your SQL Server is running locally with default settings.

#### 4. Start the API:
``` bash
dotnet run
```

#### 5. Open your browser and access:
``` bash
http://localhost:5008/swagger/index.html
```
> 💡 Note: The API will run on a dynamic port like `https://localhost:5001` or `https://localhost:5008`, depending on your system. After running `dotnet run`, check the terminal output to confirm the correct port.

## Endpoint:
### Game Recommender

`GET /v1/recommender`

#### Parameters (query string):

| Parameter     | Required | Description                                  |
|---------------|-------------|---------------------------------------------|
| `category`    | ✅ Yes       | Game Gerne (ex: `Shooter`, `MMORPG`)    |
| `platform`    | ❌ Opcional  | `pc`, `browser` or `all`                    |
| `ramMemory`   | ❌ Opcional  | Available RAM memory (in GB, ex: `8`)     |

- `/recommender?category={category_name}`: Retrieve an recommended game from a specific category/genre. 
You can try more than one category, ex: `/recommender?category=mmorpg.shooter`
- `/recommender?platform={platform_name}`: Retrieve an recommended game from a specific platform.
- `/games?ramMemory={your_ram}`: Retrieve an recommended game using your RAM as a minimum requirement

#### Example:
GET `/v1/recommender?category=Shooter&platform=pc&ramMemory=8`

#### Status Code 200:
```json
{
  "title": "Call of Duty: Warzone",
  "freetogameProfileUrl": "https://www.freetogame.com/game/cod-warzone"
}
```

#### Responses

- 200: Success
- 400: RCR-105 - Invalid platform. Use: pc, browser or all
- 400: RCR-105 - Invalid RAM.
- 400: RCR-105 - Category is required.
- 400: RCR-105 - Invalid category. Use: mmorpg, shooter, moba, anime, battle royale, strategy, fantasy, sci-fi, card games, racing, fighting, social, sports.
- 404: RCR-101 - Game not found. Check parameters.
- 502: RCR-103 - API call failed
- 500: RCR-104 - Internal server error


## Endpoint:
### All Games Recommended

`GET /v1/all-games-recommended`

#### Example:
GET `/v1/all-games-recommended`

#### Status Code 200:
```json
[
    {
        "id": 1,
        "title": "Deceit 2",
        "category": "Action",
        "counter": 1
    },
    {
        "id": 2,
        "title": "War Thunder",
        "category": "Shooter",
        "counter": 2
    },
    {
        "id": 3,
        "title": "Mecha BREAK",
        "category": "Shooter",
        "counter": 1
    },
]
```
> The `counter` shows how many times this game has been recommended.

#### Responses

- 200: Success
- 204: No Content
- 500: AGR-101 - Internal server error

### 🛠 Technologies

```markdown
## Technologies used

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Swagger / Swashbuckle
- HttpClient
- SQL Server
```
