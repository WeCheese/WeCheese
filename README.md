# Team Roster

## Dier Hou

Dier Hou is responsible for setting up the connection between the front-end app and the back-end firebase database. First, I am responsible for setting up the user signup and login mechanism using Firebase Authentication. Second, I implemented the storage of each user's each game and every move in the Firebase Database. Finally, I realized the real-time synchronization of the two users' move in one game.

The biggest challenge I encountered was to realize the synchronization of two users' moves. I first decided to use HTTP requests to retrieve the latest user move, but it turns out it's very inefficient. After some in-depth research, I found out the Firebase Database which provides real-time data update callback mechanism that allows to realize real-time synchronization.

## Yuxuan Zhu

Yuxuan Zhu works on the machine learning part of this project, specifically, the hand gesture detection part. He deployed the hand landmark model and palm detection model from Mediapipe by Google. To integrate the models to unity engine, he transferred the tflite package to unity platform and implemented the model inference algorithm using C#. Alternatively, he designed a python version of the algorithm as a backup solution on the backend.

## Zhemin Qu

Zhemin Qu works on the abstract chess and chessboard structure at the front-end. More specifically, he applies the concept of interface in Object-Oriented-Programming to implement various different chesses, including pawns, bishops, cannons, etc., and various data structures to design and implement the abstract chessboard. He works on the movement validation, movement recording, and basic rules of the game.

Meanwhile, Zhemin works as the intermediate to communicate between the backend (Firebase Manager) and the frontend renderer. As the information stored in the back-end is significantly different from the information required in the AR renderer, and the data stored in the backend should be consistent, while the view of each player is different, one great challenge is to transform the data correctly and effectively. As the detailed need of the AR renderer for the data had not been determined when the structure was designed, a large number of different encapsulated functions were designed and implemented. He did great effort to imagine the whole image of the final project and design the APIs in advance.

## Yicheng Zhang

Yicheng Zhang works on the AR components of the project.

## Shuhui Yang

Yicheng Zhang works on the AR components of the project.

## Jingye Lin

Jingye Lin works on the UI Design of the project.

# Getting Started

## List of Third-party Libraries

* [Flask API](https://flask.palletsprojects.com/en/2.3.x/)
* [Unity AR Foundation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.0/manual/index.html)
* [OpenCV](https://opencv.org/)
* [Firebase Unity SDK](https://firebase.google.com/docs/unity/setup)

Please download the above libraries to the VChess/Assets folder.

The app can be built on folder VChess with Unity Hub

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
- **Database**: The database, implemented in Firebase, stores user information and game progress.

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
| `201 Created` | Created         |
| `400 Bad Request` | Invalid parameters     |
| `409 Conflict` | Account already exists |

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
| `401 Unauthorized` | Invalid passcode |
| `403 Forbidden` | Account not exist |

**Example**

~~~ 
curl -b cookies.txt -c cookies.txt -X GET https://OUR_SERVER/available_users/'

{}
~~~

## Invite Friend

**Request Parameters**

| Key        | Location       | Type   | Description |
| ---------- | -------------- | ------ | ----------- |
| `username` | Session Cookie | String | User        |

**Response Codes**

| Code              | Description        |
| ----------------- | ------------------ |
| `200 OK`          | Success            |
| `400 Bad Request` | Invalid parameters |

**Returns**

| Key      | Location | Type   | Description               |
| -------- | -------- | ------ | ------------------------- |
| `gameid` | JSON     | String | Generated game_id         |
| `code`   | JSON     | Number | Generated invitation code |

**Example**

~~~ 
curl -b cookies.txt -c cookies.txt -X POST https://OUR_SERVER/invite/'

{
    "game_id": "twedit",
    "code": 2034
}
~~~

## Join Game

**Required Parameters** 

| Key        | Location       | Type   | Description     |
| ---------- | -------------- | ------ | --------------- |
| `username` | Session Cookie | String | User ID         |
| `code`     | JSON           | Number | Invitation Code |

**Response Codes**

| Code              | Description        |
| ----------------- | ------------------ |
| `200 OK`          | Success            |
| `400 Bad Request` | Invalid parameters |

**Returns**

| Key       | Location | Type   | Description |
| --------- | -------- | ------ | ----------- |
| `game_id` | JSON     | String | Game ID     |

**Example**

```
curl -b cookies.txt -c cookies.txt -X POST https://OUR_SERVER/join/'

{
    "game_id": "twedit"
}
```

## Move a chess

**Request Parameters**

| Key | Location | Type | Description |
| --- | -------- | ---- | ----------- |
| `username` | Session Cookie | String | User ID |
| `game_id` | JSON | String | Current game |
| `chess_id` | JSON | String | Selected chess |
| `from` | JSON | Int | Original position of the selected chess |
| `to` | JSON | Int | Destination of the selected chess |

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
| JSON | Int | Destination of the selected chess: x position |
| `des_y` | JSON | Int | Destination of the selected chess: y position |

**Example**

~~~ 
curl -x POST https://SERVER_IP/move/' -H 'Content-Type: application/json' -d

{
    "game_id": "gameid:0",
    "chess_id": "chessid:12",
    "des_x": 5,
    "des_y": 7
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

# View UI/UX
## UI/UX Flow Overview
![UI Overview](/UI_Overview.png)
Link: https://www.figma.com/file/FKQCSTApjnvaRr3wrYhBJK/VR-Chess?type=design&node-id=0-1&mode=design&t=VmpujJMC36vtkzEg-0
Tool: Figma
## User Test Video
Link:  https://sjtu.feishu.cn/file/InWAb4pVToxTsMxrGDlcjAupn8c
## Summary of Findings
**1.** Generally the UI is straightforward and easy to use.

**2.** Opponent move is not easy to tell.

**3.** Board selection interface is confusing, more prompts needed.

**4.** Board style and chess piece style selection should be separated.

**5.** Proper prompts should be given when game ends.

**6.** User feels confused about having to invite a friend again when resuming a game.
## Numerical Results
| Task | Evaluation Metrics | VChess (% success) |
| --- | --------  | ----------- |
| Initiate game by inviting friend. | <= 2 clicks | 83% |
| Change board style | <= 2 clicks |  50% |
| Make a move | <= 2 clicks |  100% |
| Watch opponent's move | <= 1 clicks, Identify move | 34% |
| Save the game | <= 2 clicks |  100% |
| Review game | <= 2 clicks |  83% |
| Resume game | <= 2 clicks |  100% |
| End game | <= 2 clicks |  100% |

## Final Design Justification
**Provide reminding for opponent move:** Change the prompt to 'Opponent move finished!'

**Provide prompts for board selection interface:** 
Showing the difference between two boards

**Provide prompts when game ends:**
A new page is added to the game process when someone wins, and there will be another new page for a lost

**Add login and signup page:**
Pages used for login and signup are added in order to transfer user data with backend

Shown above is the UI/UX flow overview. There are three main streams: starting new game, resuming game and reviewing a previous game. To start a new game, one user will be given a 6-digit number and another user can enter the code to join the game. This is to ensure that the two users can join the same game. After both users have joined the game, they will be directed to the game view and start making moves. To resume a game, both users will resume the latest game they started, the chess board will be updated to the latest status. To review a game, users can press the "next step" button to view the game step by step.
