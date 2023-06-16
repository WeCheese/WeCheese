# Getting Started

## List of Third-party Libraries

* [Flask API](https://flask.palletsprojects.com/en/2.3.x/)
* [Unity AR Foundation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.0/manual/index.html)
* [OpenCV](https://opencv.org/)

# Model and Engine

## Story Map

![Engine_Architecture](/Storymap.png)

## Block Diagram

![Engine_Architecture](/Engine_Architecture.jpg)

The project is mainly composed of the following three parts:

- **User End:** The user end is implemented with Unity. It is responsible for rendering AR settings and sending user requests (find opponents, move chess, retrieve previous games) to the server engine. It relies on the Unity AR Foundation library to render AR components and depends on OpenCV to process user gesture so as to determine user move.
- **Server Engine:** The server engine, implemented with Flask API, is responsible for processing user requests. It contains the following three handlers, you can refer to the [API and Controllers](#APIs and Controller) part for implementation details:
  * Match Request Handler: Processes user request for matching an opponent.
  * Game Sync Handler: Receives user moves and forwards the game progress to the opponent.
  * Progress Retrieval Handler: Processes user request for resuming unfinished game or saving game progress. It retrieves data from the database and sends to the user end.
- **Database**: The database, implemented in mySQL, stores user information and game progress.

# APIs and Controller
## Sign up
**Request Parameters**

| Key        | Location | Type   | Description      |
| ---------- | -------- | ------ | ---------------- |
| `username` | Session Cookie| String | New User |
| `password` | Session Cookie| String | New User |

**Response Codes**
| Code              | Description            |
| ----------------- | ---------------------- |
| `200 OK`     | Success                |
| `400 Bad Request` | Invalid parameters     |

**Returns**

*If the user alreadt exists*
| Key        | Location       | Type   | Description  |
| ---------- | -------------- | ------ | ------------ |
| `exist` | JSON | Boolean | The username is used |

**Example**
~~~ 
curl -b cookies.txt -c cookies.txt -X POST https://OUR_SERVER/signup/'

{}
~~~


## Login
**Request Parameters**

| Key        | Location | Type   | Description      |
| ---------- | -------- | ------ | ---------------- |
| `username` | Session Cookie| String | Current User |
| `password` | Session Cookie| String | Current User

**Response Codes**
| Code              | Description            |
| ----------------- | ---------------------- |
| `200 OK`     | Success                |
| `400 Bad Request` | Invalid parameters     |

**Returns**

*If the user does not exist*
| Key        | Location       | Type   | Description  |
| ---------- | -------------- | ------ | ------------ |
| `nonexist` | JSON | Boolean | Backend does not have a record of the user |

**Example**
~~~ 
curl -b cookies.txt -c cookies.txt -X GET https://OUR_SERVER/available_users/'

{
    "nonexist": True
}
~~~

## Get availablity list
**Request Parameters**

None

**Response Codes**
| Code              | Description            |
| ----------------- | ---------------------- |
| `200 OK`     | Success                |
| `400 Bad Request` | Invalid parameters     |

**Returns**

*If the user alreadt exists*
| Key        | Location       | Type   | Description  |
| ---------- | -------------- | ------ | ------------ |
| `available_users` | JSON | Boolean | A list of user IDs of available users |

**Example**
~~~ 
curl -b cookies.txt -c cookies.txt -X POST https://OUR_SERVER/signup/'

{
    "available_users": [
        "userid:0",
        "userid:1",
        ...
    ]
}
~~~

## Find Opponents
**Request Parameters**

| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `user_id` | JSON | String | current user |
| `opponent_id` | JSON | String | the user ID of selected opponent |

**Response Codes**
| Code              | Description            |
| ----------------- | ---------------------- |
| `200 OK`     | Success                |
| `400 Bad Request` | Invalid parameters     |

**Returns**

*If the opponent confirms to start a game*
| Key        | Location       | Type   | Description  |
| ---------- | -------------- | ------ | ------------ |
| `game_id` | JSON | Int | Assigned current game id |

*If the opponent refuses to start a game*
| Key        | Location       | Type   | Description  |
| ---------- | -------------- | ------ | ------------ |
| `confirmed` | JSON | Boolean | If the opponent aggrees to start the game |

**Example**
~~~ 
curl -x POST https://OUR_SERVER/find_opponents/' -H 'Content-Type: application/json' -d '{"user_id": "userid:0", "opponent_id": "userid:1"}'

{
    "confirmed": False
}
~~~

## Request opponent's confirmation
**Request Parameters**

| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `opponent_id` | JSON | String | the user who wants to be the opponent |
| `game_id` | JSON | Int | the assigned game id if the user aggres to start the game |

**Response Codes**
| Code              | Description            |
| ----------------- | ---------------------- |
| `200 OK`     | Success                |
| `400 Bad Request` | Invalid parameters     |

**Returns**

| Key        | Location       | Type   | Description  |
| ---------- | -------------- | ------ | ------------ |
| `confirmed` | JSON | Boolean | If the user agrees the request to start a game |

**Example**
~~~ 
curl -x POST https://USER_IP/confirm_opponents/' -H 'Content-Type: application/json' -d '{"opponent_id": "userid:0"}'

{
    "confirmed": False
}
~~~

## Move a chess
**Request Parameters**

| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `game_id` | JSON | String | Current game |
| `chess_id` | JSON | String | Selected chess |
| `des_x` | JSON | Int | Destination of the selected chess: x position |
| `des_y` | JSON | Int | Destination of the selected chess: y position |

**Response Codes**
| Code              | Description            |
| ----------------- | ---------------------- |
| `200 OK`     | Success                |
| `400 Bad Request` | Invalid parameters     |

**Returns**
*If the game does not end, return the opponent's move*
| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `chess_id` | JSON | String | Selected chess |
| `des_x` | JSON | Int | Destination of the selected chess: x position |
| `des_y` | JSON | Int | Destination of the selected chess: y position |

*If the game ends, return an indicator*
| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `end` | JSON | Boolean | Game ended |

**Example**
~~~ 
curl -x POST https://SERVER_IP/move/' -H 'Content-Type: application/json' -d 
'{
    "game_id": "gameid:0",
    "chess_id": "chessid:12",
    "des_x": 5,
    "des_y": 7
}'

{
    "end": True
}

~~~

## Withdraw a move
**Request Parameters**

| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `game_id` | JSON | String | Current game |

**Response Codes**
| Code              | Description            |
| ----------------- | ---------------------- |
| `200 OK`     | Success                |
| `400 Bad Request` | Invalid parameters     |

**Returns**
| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| confirmed | JSON | Boolean | If the opponent agrees the move |

**Example**
~~~
curl -x POST https://SERVER_IP/withdraw/' -H 'Content-Type: application/json' -d 
'{
    "game_id": "gameid:0",
}'

{
    "confirmed": True
}
~~~

## Confirm withdraw
**Request Parameters**
None

**Response Codes**
| Code              | Description            |
| ----------------- | ---------------------- |
| `200 OK`     | Success                |
| `400 Bad Request` | Invalid parameters     |

**Returns**
| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| confirmed | JSON | Boolean | If the opponent agrees the move |

**Example**
~~~
curl -x GET https://USER_IP/withdraw_confirm/'

{
    "confirmed": True
}
~~~

## Save game
**Request Parameters**
| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `game_id` | JSON | String | current game |

**Response Codes**
| Code              | Description            |
| ----------------- | ---------------------- |
| `200 OK`     | Success                |
| `400 Bad Request` | Invalid parameters     |

**Returns**
| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `saved_game_id` | JSON | String | the saved game |

**Example**
~~~
curl -x POST https://SERVER_IP/save/' -H 'Content-Type: application/json' -d 
'{
    "game_id": "gameid:0",
}'

{
    "saved_game_id": "sgameid:0"
}
~~~

## Resume game
**Request Parameters**
| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `saved_game_id` | JSON | String | a saved game |

**Response Codes**
| Code              | Description            |
| ----------------- | ---------------------- |
| `200 OK`     | Success                |
| `400 Bad Request` | Invalid parameters     |

**Returns**
| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `game_id` | JSON | String | Current game |
| `game_progress` | JSON | Dictionary | The position of each chess piece |

**Example**
~~~
curl -x POST https://SERVER_IP/resume/' -H 'Content-Type: application/json' -d 
'{
    "svaed_game_id": "sgameid:0",
}'

{
    "game_id": "gameid:0"
    "game_progress": {
        "chessid:0": {"x": 0, "y": 0},
        "chessid:1": {"x": 0, "y": 1},
        ...
    }
}
~~~

## Recognize gesture (TBD)
**Request Parameters**
| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `image` | JSON | Image | An image with the hand-written chess board and a hand |

**Response Codes**
| Code              | Description            |
| ----------------- | ---------------------- |
| `200 OK`     | Success                |
| `400 Bad Request` | Invalid parameters     |

**Returns**
*If the finger is pointing down and a chess is detected to be selected*
| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `x_pos` | JSON | Int | x position of the selected chess piece |
| `y_pos` | JSON | Int | y position of the selected chess piece |

*If the finger is not pointing down or no chess is detected to be selected*
| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `no_selection` | JSON | Boolean | No chess is selected |

**Example**
~~~
curl -x POST https://SERVER_IP/gesture/' -H 'Content-Type: application/json' -d 
'{
    "image": ImageObject,
}'

{
    "x_pos": 2,
    "y_pos": 3
}

~~~



## Third-Party SDKs

# View UI/UX

# Team Roster
* Dier HOU:
* Jingye LIN:
* Zhemin QU:
* Shuhui YANG:
* Yicheng ZHANG:
* Yuxuan ZHU: