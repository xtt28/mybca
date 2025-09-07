<!-- Generator: Widdershins v4.0.1 -->

<h1 id="mybca-server-v1">MyBCA.Server | v1 v1.0.0</h1>

> Scroll down for code samples, example requests and responses. Select a language for code samples from the tabs above or the mobile navigation menu.

Base URLs:

* <a href="https://mybca.link">https://mybca.link</a>

<h1 id="mybca-server-v1-busapi">BusApi</h1>

## get__api_bus_List

> Code samples

`GET /api/bus/List`

*Retrieves a map of each bus to its position*

> Example responses

> 200 Response

```
{"count":0,"data":{"property1":"string","property2":"string"},"expiry":"2019-08-24T14:15:22Z"}
```

```json
{
  "count": 0,
  "data": {
    "property1": "string",
    "property2": "string"
  },
  "expiry": "2019-08-24T14:15:22Z"
}
```

<h3 id="get__api_bus_list-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[BusApiResponse](#schemabusapiresponse)|

<aside class="success">
This operation does not require authentication
</aside>

<h1 id="mybca-server-v1-linkapi">LinkApi</h1>

## get__api_links

> Code samples

`GET /api/links`

*Retrieves a list of quick links to key BCA services*

> Example responses

> 200 Response

```
[{"name":"string","target":"http://example.com"}]
```

```json
[
  {
    "name": "string",
    "target": "http://example.com"
  }
]
```

<h3 id="get__api_links-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|Inline|

<h3 id="get__api_links-responseschema">Response Schema</h3>

Status Code **200**

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|*anonymous*|[[Link](#schemalink)]|false|none|none|
|» name|string|true|none|none|
|» target|string(uri)|true|none|none|

<aside class="success">
This operation does not require authentication
</aside>

<h1 id="mybca-server-v1-lunchapi">LunchApi</h1>

## get__api_lunch_Week

> Code samples

`GET /api/lunch/Week`

*Retrieves the lunch menu for the week*

> Example responses

> 200 Response

```
{"startDate":"string","displayName":"string","days":[{"date":"string","menuItems":[{"date":"2019-08-24T14:15:22Z","position":0,"isSectionTitle":true,"text":"string","food":{"id":0,"name":"string","description":"string","subtext":"string","imageUrl":"string"},"stationID":0,"isStationHeader":true,"image":"string","category":"string"}]}]}
```

```json
{
  "startDate": "string",
  "displayName": "string",
  "days": [
    {
      "date": "string",
      "menuItems": [
        {
          "date": "2019-08-24T14:15:22Z",
          "position": 0,
          "isSectionTitle": true,
          "text": "string",
          "food": {
            "id": 0,
            "name": "string",
            "description": "string",
            "subtext": "string",
            "imageUrl": "string"
          },
          "stationID": 0,
          "isStationHeader": true,
          "image": "string",
          "category": "string"
        }
      ]
    }
  ]
}
```

<h3 id="get__api_lunch_week-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[MenuWeek](#schemamenuweek)|

<aside class="success">
This operation does not require authentication
</aside>

## get__api_lunch_Day

> Code samples

`GET /api/lunch/Day`

*Retrieves the lunch menu for the day*

> Example responses

> 200 Response

```
{"date":"string","menuItems":[{"date":"2019-08-24T14:15:22Z","position":0,"isSectionTitle":true,"text":"string","food":{"id":0,"name":"string","description":"string","subtext":"string","imageUrl":"string"},"stationID":0,"isStationHeader":true,"image":"string","category":"string"}]}
```

```json
{
  "date": "string",
  "menuItems": [
    {
      "date": "2019-08-24T14:15:22Z",
      "position": 0,
      "isSectionTitle": true,
      "text": "string",
      "food": {
        "id": 0,
        "name": "string",
        "description": "string",
        "subtext": "string",
        "imageUrl": "string"
      },
      "stationID": 0,
      "isStationHeader": true,
      "image": "string",
      "category": "string"
    }
  ]
}
```

<h3 id="get__api_lunch_day-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[MenuDay](#schemamenuday)|

<aside class="success">
This operation does not require authentication
</aside>

# Schemas

<h2 id="tocS_BusApiResponse">BusApiResponse</h2>
<!-- backwards compatibility -->
<a id="schemabusapiresponse"></a>
<a id="schema_BusApiResponse"></a>
<a id="tocSbusapiresponse"></a>
<a id="tocsbusapiresponse"></a>

```json
{
  "count": 0,
  "data": {
    "property1": "string",
    "property2": "string"
  },
  "expiry": "2019-08-24T14:15:22Z"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|count|integer(int32)|true|none|none|
|data|object|true|none|none|
|» **additionalProperties**|string|false|none|none|
|expiry|string(date-time)¦null|true|none|none|

<h2 id="tocS_FoodItem">FoodItem</h2>
<!-- backwards compatibility -->
<a id="schemafooditem"></a>
<a id="schema_FoodItem"></a>
<a id="tocSfooditem"></a>
<a id="tocsfooditem"></a>

```json
{
  "id": 0,
  "name": "string",
  "description": "string",
  "subtext": "string",
  "imageUrl": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|integer(int32)|true|none|none|
|name|string¦null|true|none|none|
|description|string¦null|true|none|none|
|subtext|string¦null|true|none|none|
|imageUrl|string¦null|true|none|none|

<h2 id="tocS_Link">Link</h2>
<!-- backwards compatibility -->
<a id="schemalink"></a>
<a id="schema_Link"></a>
<a id="tocSlink"></a>
<a id="tocslink"></a>

```json
{
  "name": "string",
  "target": "http://example.com"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|name|string|true|none|none|
|target|string(uri)|true|none|none|

<h2 id="tocS_MenuDay">MenuDay</h2>
<!-- backwards compatibility -->
<a id="schemamenuday"></a>
<a id="schema_MenuDay"></a>
<a id="tocSmenuday"></a>
<a id="tocsmenuday"></a>

```json
{
  "date": "string",
  "menuItems": [
    {
      "date": "2019-08-24T14:15:22Z",
      "position": 0,
      "isSectionTitle": true,
      "text": "string",
      "food": {
        "id": 0,
        "name": "string",
        "description": "string",
        "subtext": "string",
        "imageUrl": "string"
      },
      "stationID": 0,
      "isStationHeader": true,
      "image": "string",
      "category": "string"
    }
  ]
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|date|string¦null|true|none|none|
|menuItems|[[MenuItem](#schemamenuitem)]|true|none|none|

<h2 id="tocS_MenuItem">MenuItem</h2>
<!-- backwards compatibility -->
<a id="schemamenuitem"></a>
<a id="schema_MenuItem"></a>
<a id="tocSmenuitem"></a>
<a id="tocsmenuitem"></a>

```json
{
  "date": "2019-08-24T14:15:22Z",
  "position": 0,
  "isSectionTitle": true,
  "text": "string",
  "food": {
    "id": 0,
    "name": "string",
    "description": "string",
    "subtext": "string",
    "imageUrl": "string"
  },
  "stationID": 0,
  "isStationHeader": true,
  "image": "string",
  "category": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|date|string(date-time)¦null|true|none|none|
|position|integer(int32)|true|none|none|
|isSectionTitle|boolean|true|none|none|
|text|string¦null|true|none|none|
|food|[FoodItem](#schemafooditem)|true|none|none|
|stationID|integer(uint32)|true|none|none|
|isStationHeader|boolean|true|none|none|
|image|string¦null|true|none|none|
|category|string¦null|true|none|none|

<h2 id="tocS_MenuWeek">MenuWeek</h2>
<!-- backwards compatibility -->
<a id="schemamenuweek"></a>
<a id="schema_MenuWeek"></a>
<a id="tocSmenuweek"></a>
<a id="tocsmenuweek"></a>

```json
{
  "startDate": "string",
  "displayName": "string",
  "days": [
    {
      "date": "string",
      "menuItems": [
        {
          "date": "2019-08-24T14:15:22Z",
          "position": 0,
          "isSectionTitle": true,
          "text": "string",
          "food": {
            "id": 0,
            "name": "string",
            "description": "string",
            "subtext": "string",
            "imageUrl": "string"
          },
          "stationID": 0,
          "isStationHeader": true,
          "image": "string",
          "category": "string"
        }
      ]
    }
  ]
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|startDate|string¦null|true|none|none|
|displayName|string¦null|true|none|none|
|days|[[MenuDay](#schemamenuday)]|true|none|none|

