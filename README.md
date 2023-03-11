# REST API на основе ежедневных данных о курсах валют ЦБ РФ

## Краткое описание:

- Сервис реализован на ASP.NET CORE Web API
- Репозиторий содержит несколько проектов:
> - CBCurrenciesService - Web API приложение 
> - Contracts - библиотека классов, содержащая необходимые интерфейсы
> - Entities - библиотека классов, содержащая необходимые сущности, в том числе для пагинации
> - HttpService - библиотека классов, содержащая реализацию сервиса, работающего с API ЦБ РФ
> - LoggerService - библиотека классов, содержащая реализацию кастомного логгера с использованием пакета NLog
> - CBCurrenciesService.test - проект для тестирования Web API приложения с использованием пакета xUnit (реализовано тестирование контроллеров)
  
### Описание методов сервиса

#### Получение значений валют:

<details>
 <summary><code>GET</code> <code><b>/currencies</b></code> <code>(получить значение всех валют с использованием пагинации)</code></summary>

##### Параметры
  
> | Имя параметра     | Тип данных| Описание                                                                                |
> |-------------------|-----------|-----------------------------------------------------------------------------------------|
> | PageNumber        | int       | Номер страницы                                                                          |
> | PageSize          | int       | Количество элементов, демонстрируемых на одной странице                                 |
  
##### Тело запроса

> Нет параметров

##### Возможные ответы

> | http код      | тип содержимого                   | ответ                                                                     |
> |---------------|-----------------------------------|---------------------------------------------------------------------------|
> | `200`         | `application/json; charset=utf-8 `| `Массив объектов типа "SingleCurrencyDto" - структуру типа см. ниже`      |
> | `400`         | `text/plain;charset=UTF-8`        | `Pagination parameters are not valid`                                     |
> | `404`         | `text/plain;charset=UTF-8`        | `Response status code does not indicate success: 404 (Not Found)`         |
> | `500`         | `text/html;charset=utf-8`         | `Internal server error. Something went wrong inside GetCurrencies action` |                                                                     
##### Структура типа "SingleCurrencyDto" - элемента списка, содержащегося в теле ответа с кодом 200
 
> | Имя параметра     | Тип данных| Описание                                                                                |
> |-------------------|-----------|-----------------------------------------------------------------------------------------|
> | reportDate        | datetime  | Дата и время отчета                                                                     |
> | id                | string    | Идентификатор валюты                                                                    |
> | numCode           | string    | Числовой код валюты                                                                     |
> | charCode          | string    | Символьный код валюты                                                                   |  
> | nominal           | int       | Номинал                                                                                 |   
> | name              | string    | Наименование валюты                                                                     |   
> | value             | double    | Значение                                                                                |   
> | previous          | double    | Предыдущее значение (за прошлую отчетную дату)                                          |   

##### Структура типа "Metadata", содержащегося в заголовке "x-pagination" ответа с кодом 200

> | Имя параметра     | Тип данных| Описание                                                                                |
> |-------------------|-----------|-----------------------------------------------------------------------------------------|
> | TotalCount        | int       | Общее количество доступных элементов (из всего списка доступных валют)                  |
> | PageSize          | int       | Количество элементов, демонстрируемых на одной странице                                 |
> | CurrentPage       | int       | Текущая (выбранная) страница                                                            |
> | TotalPages        | int       | Расчетное количество доступных страниц                                                  |  
> | HasNext           | bool      | Флаг, показывающий, существует ли следующая страница                                    |   
> | HasPrevious       | bool      | Флаг, показывающий, существует ли предыдущая страница                                   |    

##### Образец тела ответа с кодом 200
  
> ```javascript
> [
>  {
>    "reportDate": "2023-03-11T11:30:00+03:00",
>    "id": "R01010",
>    "numCode": "036",
>    "charCode": "AUD",
>    "nominal": 1,
>    "name": "Австралийский доллар",
>    "value": 50.1132,
>    "previous": 50.1718
>  },
>  {
>    "reportDate": "2023-03-11T11:30:00+03:00",
>    "id": "R01020A",
>    "numCode": "944",
>    "charCode": "AZN",
>    "nominal": 1,
>    "name": "Азербайджанский манат",
>    "value": 44.6709,
>    "previous": 44.6487
>  }
> ] 
> ```
  
##### Обрезец заголовков ответа с кодом 200
> ```javascript
>   content-length: 376 
>   content-type: application/json; charset=utf-8 
>   date: Sat,11 Mar 2023 08:45:45 GMT 
>   server: Kestrel 
>   x-pagination: {"TotalCount":43,"PageSize":2,"CurrentPage":1,"TotalPages":22,"HasNext":true,"HasPrevious":false} 
>  ```  

  
##### Образец cURL

> ```javascript
>  curl -X 'GET' \
>  'https://localhost:7124/currencies?PageNumber=1&PageSize=2' \
>  -H 'accept: */*'
> ```

</details>

<details>
 <summary><code>GET</code> <code><b>/currency/currencyId</b></code> <code>(получить значение валюты по ее идентификатору)</code></summary>

##### Параметры
  
> | Имя параметра     | Тип данных| Описание                                                                                |
> |-------------------|-----------|-----------------------------------------------------------------------------------------|
> | currencyId        | string    | Идентификатор валюты                                                                    |
  
##### Тело запроса

> Нет параметров

##### Возможные ответы

> | http код      | тип содержимого                   | ответ                                                                     |
> |---------------|-----------------------------------|---------------------------------------------------------------------------|
> | `200`         | `application/json; charset=utf-8 `| `Объект типа "SingleCurrencyDto" - структуру типа см. ниже`               |
> | `400`         | `text/plain;charset=UTF-8`        | -                                   |
> | `404`         | `text/plain;charset=UTF-8`        | -                                                                         |
> | `404`         | `text/plain;charset=UTF-8`        | `Response status code does not indicate success: 404 (Not Found)`         |
> | `500`         | `text/html;charset=utf-8`         | `Internal server error. Something went wrong inside GetCurrencies action` |                                                                     
##### Структура типа "SingleCurrencyDto" - тип данных объекта, возвращаемого при ответе со статусом 200
 
> | Имя параметра     | Тип данных| Описание                                                                                |
> |-------------------|-----------|-----------------------------------------------------------------------------------------|
> | reportDate        | datetime  | Дата и время отчета                                                                     |
> | id                | string    | Идентификатор валюты                                                                    |
> | numCode           | string    | Числовой код валюты                                                                     |
> | charCode          | string    | Символьный код валюты                                                                   |  
> | nominal           | int       | Номинал                                                                                 |   
> | name              | string    | Наименование валюты                                                                     |   
> | value             | double    | Значение                                                                                |   
> | previous          | double    | Предыдущее значение (за прошлую отчетную дату)                                          |   

##### Образец тела ответа с кодом 200
  
> ```javascript
> {
>   "reportDate": "2023-03-11T11:30:00+03:00",
>   "id": "R01020A",
>   "numCode": "944",
>   "charCode": "AZN",
>   "nominal": 1,
>   "name": "Азербайджанский манат",
>   "value": 44.6709,
>   "previous": 44.6487
> }
> ```
  
##### Обрезец заголовков ответа с кодом 200
> ```javascript
> content-length: 188 
> content-type: application/json; charset=utf-8 
> date: Sat,11 Mar 2023 09:12:05 GMT 
> server: Kestrel  
>  ```  

##### Образец cURL

> ```javascript
> curl -X 'GET' \
>  'https://localhost:7124/currency/R01020A' \
>  -H 'accept: */*'
> ```

</details>



