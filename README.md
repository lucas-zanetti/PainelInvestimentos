# API Painel Investimentos

Este projeto é uma API RESTful para simulação e controle de investimentos, desenvolvida em '.NET' 8. A aplicação oferece endpoints para simular investimentos, consultar simulações já realizadas, consultar parfil de risco, consultar produtos recomendados por perfil, geração de token, crialçao de usuário e obtenção de dados de telemetria.

-----

### Requisitos

Certifique-se de que você tem as seguintes ferramentas instaladas:

  * [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
  * [Docker](https://www.docker.com/products/docker-desktop/)

-----

### Execução Local com .NET CLI

Para rodar a aplicação localmente sem Docker, use a linha de comando do .NET.

1.  Navegue até o diretório do projeto:

    ```bash
    cd API_Painel-Investimentos
    ```

2.  Rode a aplicação:

    ```bash
    dotnet run
    ```

A aplicação será executada no seguinte endereço:

  * **HTTPS:** `https://localhost:7716`
  * **HTTP:** `http://localhost:7715`

Acesse a documentação da API em `https://localhost:7716/swagger` ou `http://localhost:7715/swagger`.

-----

### Execução com Docker Compose

Para construir e rodar a aplicação em um contêiner Docker, use o Docker Compose. Isso é ideal para criar um ambiente de execução isolado.

1.  Navegue até o diretório raiz da sua solution (onde o arquivo `docker-compose.yml` está localizado).

2.  Execute o comando para construir e iniciar os contêineres em segundo plano:

    ```bash
    docker-compose up --build -d
    ```

A aplicação será executada em um contêiner Docker e os seguintes mapeamentos de porta serão feitos para que você possa acessá-la de seu sistema local:

  * **HTTPS:** `https://localhost:7718` **FOI CRIADO UM CERTIFICADO APENAS PARA FINS DE TESTE**
  * **HTTP:** `http://localhost:7717`

Acesse a documentação da API em `https://localhost:7718/swagger` ou `http://localhost:7717/swagger`.

Para parar e remover os contêineres, execute:

```bash
docker-compose down
```