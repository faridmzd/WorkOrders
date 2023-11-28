
# W.O.

A W.O. stands for Work Orders. It's a simple app to register and manage work orders along with their visits and parts(line items)

Currently only API is available.

What will be included in future releases:
CQRS, DDD, unit, functional tests.

In order to start the project 
1. you'll need to add a valid SQL Connection string in appsettings.json

2. run 'dotnet ef migrations add InitialCreate' in .NET CLI in order to add migrations.Make sure that you're running the command inside W.O.API folder

Note: for now no rows will be added to selected database, for simplicity you can either use swagger or postman to see what's available 

Postman collection : https://api.postman.com/collections/26626853-93931c7f-1984-49e7-a439-3bcf8c098bab?access_key=PMAT-01HGAZ6S50Y6PAC8SETKNK3NFD

3. run 'dotnet ef database update'






## API Reference
### Work Orders

#### Get all work orders

```https
  GET /api/v1/workorders
```
#### Get work order

```https
  GET /api/v1/workorders/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `guid` |  **Required**|

#### Add work order

```https
  POST /api/v1/workorders
```

| Body | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `title`      | `string` | **Required**|
| `description`      | `string` | **Required**|
| `phone`      | `string` | **Required**|
| `email`      | `string` | **Required**|
| `startAt`    | `date` | **Required**|
| `finishAt`   | `date` | **Required**|

one of the valid phone number formats would be: "XX-XXX-XXX-XXX"
as for now we don't support country extensions.
```https
  Example payload {

    "title":"order_1",
    "description":"something",
    "phone":"+59967895",
    "email":"farid.mmzd@gmail.com",
    "startAt": "2023-11-26T12:07:12.557",
    "finishAt": "2023-11-29T12:07:12.557"
  }
```
#### Delete work order

```https
  DELETE /api/v1/workorders/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `guid` |  **Required**|

#### Update work order

```https
  PUT /api/v1/workorders/${id}
```
| Parameter       | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `guid` | **Required**|


| Body | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `title`      | `string` | **Not Required**|
| `description`      | `string` | **Not Required**|
| `phone`      | `string` | **Not Required**|
| `email`      | `string` | **Not Required**|
| `startAt`    | `date` | **Not Required**|
| `finishAt`   | `date` | **Not Required**|

### Visits

#### Get all visits

```https
  GET /api/v1/visits
```
#### Get visit

```https
  GET /api/v1/visits/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `guid` |  **Required**|

#### Add visit

```https
  POST /api/v1/visits
```

| Body | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `workOrderId`      | `guid` | **Required**|
| `assigneeFullName`      | `string` | **Required**|
| `assignedFrom`      | `date` | **Required**|
| `parts`      | `[]` | **Required**|

Note : in order to add visit you've to a work order first, and within request you should send at least one part to be created, max 3 parts are allowed for each visit and 5 visits for each work order.

```https
  Example payload {

    "workOrderId":"",
    "assigneeFullName":"",
    "assignedFrom":"2023-11-26T12:07:12.557",
    "parts":[
    {
    "description":"",
     "amount": 100,
     "currency" : "usd",
     "quantity" : 2
    },
    {"description":"",
     "amount": 150,
     "currency" : "usd",
     "quantity" : 2
    },
    {"description":"",
     "amount": 150.56,
     "currency" : "EuR",
     "quantity" : 10
    }]
  }
```

#### Delete visit

```https
  DELETE /api/v1/visits/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `guid` |  **Required**|

#### Update visit

```https
  PUT /api/v1/visits/${id}
```
| Parameter       | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `guid` | **Required**|


| Body | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `assigneeFullName`      | `string` | **Not Required**|
| `assignedFrom`   | `date` | **Not Required**|

### Parts

#### Get all parts

```https
  GET /api/v1/parts
```
#### Get part

```https
  GET /api/v1/parts/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `guid` |  **Required**|

#### Add part

```https
  POST /api/v1/parts
```

| Body | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `visitId`      | `guid` | **Required**|
| `amount`      | `decimal(18,2)` | **Required**|
| `description`      | `string` | **Required**|
| `currency`      | `string` | **Required**|
| `quantity`      | `int` | **Required**|

Note : in order to see supported list of currencies by API, you can first ping '/api/v1/currencies'

```https
  Example payload {

    "visitId":"",
    "description":"",
    "amount":"567",
    "currency":"eur",
    "quantity": 10
  }
```

#### Delete visit

```https
  DELETE /api/v1/parts/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `guid` |  **Required**|

#### Update visit

```https
  PUT /api/v1/parts/${id}
```
| Parameter       | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `guid` | **Required**|


| Body | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `description`      | `string` | **Not Required**|
| `amount`      | `decimal(18,2)` | **Not Required**|
| `currency`      | `string` | **Not Required**|
| `quantity`      | `int` | **Not Required**|
