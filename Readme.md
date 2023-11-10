```shell
> dotnet new webapi -n ProductApi
> cd ProductApi
```

when the project setup is finished

```shell
> dotnet ef migrations add InitialCreate
> dotnet ef database update
```

to run the project

```shell
> docker-compose up --build
> docker-compose down
```


