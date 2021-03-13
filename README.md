# RtuItLab
Проект релизован в качестве демонстрации работы микросервисов, контейнеров docker, брокера сообщений RabbitMQ. 
![Схема](https://github.com/bezlla/RtuItLab/blob/master/Scheme.png)
Схема взаимодействия микросервисов в проекте.  

Порты, на которых располагаются микросервисы:
* Identity - 7001
* Purchases - 7002
* Shops - 7003
* Factories - 7004
* RabbitMQ - 15672 & 5672
* ApiGateway - 5000
## Стек проекта:
* Asp Net Core Web Api
* [Entity Framework Core](https://docs.microsoft.com/ru-ru/ef/core/)
* Unit Tests
* [Docker](https://www.docker.com/)
* [RabbitMQ](https://www.rabbitmq.com/)
* [Api Gateway (Ocelot)](https://ocelot.readthedocs.io/en/latest/features/configuration.html)
## Содержание
1. [Функциональность проекта](#Функциональность-проекта)
2. Описание микросервисов
   1. [Identity](#Identity) 
   2. [Purchases](#Purchases)  
   3. [Shops](#Shops)  
   4. [Factories](#Factories)  
3. [Базы данных](#Базы-данных)
4. [RabbitMQ](#RabbitMQ)
5. [API Gateway (Ocelot)](#Api-Gateway-Ocelot)
6. [Docker](#Docker)
7. [UnitTests](#UnitTests)
#
### Функциональность проекта
Возможности клиента:
1. На микросервисе Identity:
  - зарегистрироваться
  - авторизоваться
  - узнать информацию о себе
2. На микросервисе Purchases:
  - посмотреть список своих покупок
  - добавить в этот список самописную покупку
  - узнать детальную информацию по конкретной покупке
  - изменить информацию по покупке
3. На микросервисе Shops:
  - посмотреть список магазинов
  - посмотреть список товаров в магазине
  - посмотреть список продуктов по категории в магазине
  - купить список товаров в одном магазине

Микросервис Factories в свою очередь каждые 20 секунд пополняет "склады" магазинов. А микросервис Shops при покупке клиентом товаров добавляет покупку в список
покупок пользователя на микросервисе Purchases.
### Микросервисы

Каждый микросервис выполняет определённую роль. Рассмотрим их отдельно.

В каждом из микросервисов, кроме Factories используется swagger. В URL, прикреплённом к микросервисам вы сможете найти информацию по API-путям, а также методам,
которые используются в данном микросервисе.

Каждый микросервис делится на 3 слоя :
- DAL - Data Access Layer  
  Здесь вся работа с данными микросервиса.
- BLL - Business Logic Layer  
  Здесь Бизнес логика микросервиса.
- PI - Personal Interface
#### Identity
> http://localhost:7001/swagger  

Данный микросервис выполняет функцию аутентификации пользователей. Использует библиотеку [AspNetCore.Identity](https://www.nuget.org/packages/Microsoft.AspNet.Identity.Core/)
для шифрования данных юзеров в БД, в частности использование хэшей, вместо паролей.

В ответ на логин пользователя, сервис шлёт [токен JWT](https://ru.wikipedia.org/wiki/JSON_Web_Token), в котором зашифрована информация по юзеру.
```json
{
  "succeeded": true,
  "code": 200,
  "result": {
    "id": "ad6fbc79-71bc-4391-a673-c5784703eae8",
    "userName": "User",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImFkNmZiYzc5LTcxYmMtNDM5MS1hNjczLWM1Nzg0NzAzZWFlOCIsIm5iZiI6MTYxNTA0ODU3MiwiZXhwIjoxNjE1NjUzMzcyLCJpYXQiOjE2MTUwNDg1NzJ9.FHvSD7-1T1104IP5Fjb9VbSf7xUyt90oGvinq8NEGhM"
  },
  "errors": []
}
```
В дальнейшем этот токен будет использоваться в заголовках запроса пользователя по другим микросервисам также.
Конкретнее, его нужно положить в заголовок Authorization - ```Bearer {yourToken}```.

#### Purchases 
> http://localhost:7002/swagger

Здесь пользователь может найти информацию по своим покупкам, а также сделать свои, но с ограничениями. Также пользователь может отредактировать покупки.
Детальнее рассмотрим ниже.

- Добавление

  Если магазин добавляет покупку, то здесь, естественно, никаких ограничений нет. Если же пользователь, в таком случае он не может прикрепить к покупке чек, то есть 
  ```"receipt":null```
  актуально для покупок, созданных пользователем.
    У покупок, созданных пользователем, параметр ```IsShopCreate``` всегда false.
  Пример тела запроса добавления своей покупки
 ```json
 {
  "products": [
    {
      "name": "string",
      "productId": 0,
      "cost": 0,
      "count": 0,
      "category": "string"
    }
  ],
  "date": "2021-03-06T16:58:38.251Z",
  "transactionType": 0
}
 ```

Без параметра ```date``` или ```transactionType``` также будет работать.
- Редактирование

  - Покупка из магазина  
  
    Пользователь может редактировать способ оплаты, параметр ```TransactionType```,  значение 0 - оплата картой, 1 - наличными.
    
  - Покупка, добавленная пользователем  
    Здесь ограничений для пользователя нет. Он может изменить любой параметр, включая список купленных продуктов.
   Пример тела запроса редактирования своей покупки
 ```json
 {
  "id": 1,
  "products": [
    {
      "name": "string",
      "productId": 0,
      "cost": 0,
      "count": 0,
      "category": "string"
    }
  ],
  "transactionType": 1
 }
 ```
   Здесь ```id``` обязательно должен существовать, все заполняемые параметры в запросе будут изменены в покупке!
   Ненужные параметры необходимо удалить.
   
#### Shops
> http://localhost:7003/swagger

Данный микросервис реализует логику взаимодействия пользователя с магазинами. Пользователь может посмотреть список магазинов, посмотреть товары в магазине, и купить список
товаров в определенном магазине. Рассмотрим только последний пункт.
- Покупка товаров пользователем 

  Каждый пользователь может купить товары, которые есть в магазине (не больше количества, что есть).  
  Пример тела запроса на покупку:
```json
[
  {
    "productId": 0,
    "count": 0,
  }
]
```
  Для запроса необходимо лишь два параметра : количество товара и Id товара. Если запрос на сервер будет отправлен валидный, и он будет успешным, в таком случае Shops
  отправляет на сервис Purchases для этого пользователя новую покупку, в которой обязательно будет ```receipt``` - чек.
- Поиск продуктов по категории
  Каждый пользователь может выполнить поиск продуктов в магазине по категории.  
  Пример тела запроса:
```json
{
 "categoryName": "одежда"
}
```
```categoryName``` единственный и обязательный параметр.

#### Factories
>http://localhost:7004/

Данный микросервис выполняет одну функцию - пополнения магазины товарами. Каждые 20 секунд данный сервис отправляет новые товары на сервис Shops, согласно данным в БД.

### Базы данных

В проекте используется 4 базы данных:
* Для микросервиса [Identity](https://github.com/bezlla/RtuItLab/tree/master/src/Services/Identity/Identity.DAL) 
* Для микросервиса [Purchases](https://github.com/bezlla/RtuItLab/tree/master/src/Services/Purchases/Purchases.DAL)
* Для микросервиса [Shops](https://github.com/bezlla/RtuItLab/tree/master/src/Services/Shops/Shops.DAL):
  1. [Инициализация](https://github.com/bezlla/RtuItLab/blob/master/src/Services/Shops/Shops.DAL/Data/ShopsDbContext.cs) 4 магазинов и по 4 товара к каждому.
* Для микросервиса  [Factories](https://github.com/bezlla/RtuItLab/tree/master/src/Services/Factories/Factories.DAL):
  1. [Инициализация](https://github.com/bezlla/RtuItLab/blob/master/src/Services/Factories/Factories.DAL/Data/FactoriesDbContext.cs) 3-4 заводов и по несколько товаров к каждому из разных магазинов
  
 Подход проектирования БД: Code first  
 Тип хранения данных: SQL Server & Docker  
 ORM: Entity Framework Core
 
 ### RabbitMQ
 > занимает порты 15672 и 5672
 > 
 В данном проекте [RabbitMQ](https://www.rabbitmq.com/) используется в связке с библиотекой [MassTransit](https://masstransit-project.com/) для обработки запросов с
 различных микросервисов. Каждый запрос попадает в Queue и обрабатывается после того, как достигает его очередь.
 Также RabbitMQ позволяет не терять запросы и взаимодействовать микросервисам друг с другом.
 
 В каждом проекте в Startap'e прописаны конфигурации для соответсвующих очередей этому микросервису, которые работают с помощью библиотеки MassTransit.
 
 Ниже пример добавления обработчиков (Consumer'ов) для очередей RabbitMQ,
 ```C#
  // Identity
  x.AddConsumer<Authenticate>();
  x.AddConsumer<CreateUser>();
  x.AddConsumer<GetUserByToken>();
 ```
 и конфигурируются сами очереди
 ```C#
 cfg.ReceiveEndpoint("identityQueue", e =>
                    {
                        e.PrefetchCount = 20;
                        e.UseMessageRetry(r => r.Interval(2, 100));

                        //// Identity
                        e.Consumer<Authenticate>(context);
                        e.Consumer<CreateUser>(context);
                        e.Consumer<GetUserByToken>(context);

                    });
 ```
 
 Ниже можно найти настройки для сериализации/десериализации запросов. Используется [Newtonsoft.Json](https://www.newtonsoft.com/json).
 ```C#
  cfg.ConfigureJsonSerializer(settings =>
                    {
                        settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

                        return settings;
                    });
                    cfg.ConfigureJsonDeserializer(configure =>
                    {
                        configure.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                        return configure;
                    });
 ```
 
 Также в каждом микросервисе есть конфигурации для подключения к RabbitMQ : 
 ```C#
 services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {

                    cfg.Host(new Uri("rabbitmq://host.docker.internal/"));
                    cfg.ConfigureJsonSerializer(settings =>
                    {
                        settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

                        return settings;
                    });
                    cfg.ConfigureJsonDeserializer(configure =>
                    {
                        configure.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                        return configure;
                    });
                });

            });
            services.AddMassTransitHostedService();
 ```
 
### Api Gateway (Ocelot)
>http://localhost:5000 

API Gateway — это высокопроизводительный, доступный и безопасный сервис размещения API,
который помогает создавать, разворачивать программные интерфейсы приложения в любом масштабе и управлять ими.

В данном проекте вы найдёте реализацию [Ocelot Api Gateway](https://github.com/bezlla/RtuItLab/tree/master/src/RtuItLab.ApiGateway)

Для конфигурирования проекта подключается ```ocelot.json``` [файл](https://github.com/bezlla/RtuItLab/blob/master/src/RtuItLab.ApiGateway/ocelot.json), в котором расписаны пути, а также методы переадресации.
Документацию по нему вы сможете найти [здесь](https://ocelot.readthedocs.io/en/latest/features/configuration.html)

### Docker

Данный проект поддерживает docker.
Для его запуска достаточно прописать:
```
docker-compose pull
docker-compose up
```
Детали конфигурации docker-compose вы можете найти [здесь](https://github.com/bezlla/RtuItLab/blob/master/src/docker-compose.yml).
### UnitTests

В проекте есть Smoke - тесты, для запуска которых необходимо запустить менеджера RabbitMQ.

Они покрывают доступность функционала.

Также есть Unit - тесты, которые покрывают функционал сервисов [Purchases](https://github.com/bezlla/RtuItLab/tree/master/src/Services/Purchases/tests/Purchases.UnitTests)
и [Shops](https://github.com/bezlla/RtuItLab/tree/master/src/Services/Shops/tests/Shops.UnitTests).


