using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase;
using TMPro;
using Unity.VisualScripting;
// using Unity.VisualScripting.Dependencies.Sqlite;

public class FirebaseManager : MonoBehaviour
{
    /*[SerializeField] Button updateCountButton;

    [SerializeField] Text countUI;*/

    public TextMeshProUGUI GameCodeDisplay;
    public TMP_Text ChessMoveDebugTxt;

    FirebaseFirestore db;
    private string email;
    private string password;
    private Int32 gameCodeInput;
    private Int32 gameCodeGenerated;
    private string userId = "zMqKfBE7cnhl9XekvRi06AQqWam1";
    private string gameId = "6MX4v0BstzFXGtwS03e0"; 
    public GameObject StartCanvas;
    public GameObject GameCanvas;
    private int currentStepCnt = 0;
    private int isFirst = 0; // -1 : unknown, 1: yes, 0: false
    
    //Test
    // public TextMeshProUGUI testtext;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start!!!!!");
        db = FirebaseFirestore.DefaultInstance;
        // Modified UpdateMoveCallbackRegisterAsync(randomtest);
        // updateCountButton.onClick.AddListener(OnHandleClick);
    }
    
    public async void OnClick()
    {
        var auth = FirebaseAuth.DefaultInstance;
        var createTask = auth.CreateUserWithEmailAndPasswordAsync("crazy@gmail.com", "passwordpassword");
        string testUserId = "";
        try
        {
            var taskResult = await createTask;
            testUserId = taskResult.User.UserId;
            Debug.Log(testUserId);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    
    

    private async void ListenForJoin()
    {
        DocumentReference gameDoc = db.Collection("Games").Document(gameId);
        gameDoc.Listen(async snapshot =>
        {
            Dictionary<string, object> docValue = snapshot.ToDictionary();
            if (docValue != null && docValue.ContainsKey("User2"))
            {
                Debug.Log("User 2 Joined!!!!!!");
                StartCanvas.SetActive(false);
                GameCanvas.SetActive(true);
                GameObject.Find("chess_game_manager").GetComponent<MoveChess>().game_start = true;
            }
        });
    }

    public async void Test()
    {
        var testDoc = db.Collection("Games").Document("Test").SetAsync(new Dictionary<string, string> { { "Games", "hello" } });
        Debug.Log("test fb!!!!!!!!!");
        try
        {
            await testDoc;
            Debug.Log("added");
        } catch(Exception e)
        {
            Debug.Log("error");
            Debug.Log(e);
        }
        
    }
    public void ReadEmail(string emailInput)
    {
        email = emailInput;
        Debug.Log(email);
    }

    public void ReadPassword(string pwdInput)
    {
        password = pwdInput;
        Debug.Log(pwdInput);
    }

    public void ReadGameCode(string codeInput) {
        gameCodeInput = Convert.ToInt32(codeInput);
        
        Debug.Log(gameCodeInput);
    }
    
    
    public async void OnHandleClick()
    {
        /*DocumentReference countRef = db.Collection("Games").Document("sample_game_id");
        
        countRef.Listen(snapshot =>
        {
            Debug.Log("Document updated");
            
        });

        var createTask = CreateAccountHandlerAsync();
        try
        {
            string userId = await createTask;
            Debug.Log(userId);

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }


        try
        {
            // await JoinGameAsync("rGqtAq94g0P9R2awRC8EKWVji9D2", 707726);
            // await CreateAccountHandlerAsync("example@email.com", "abcdeeef");

            List<Dictionary<string, object>> testCoord = new List<Dictionary<string, object>> {new Dictionary<string, object>{{"type", "horse"}, {"coord", 9}} };
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        // JoinGame("QQ", 428631);*/
    }

    public bool randomtest(int id1, int id2)
    {
        Debug.Log("randomtest is called");
        return true;
    }
    public async void CreateAccountHandlerAsync()
    {
        Debug.Log("Creating Account");
        var auth = FirebaseAuth.DefaultInstance;
        var createAccTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        try
        {
            Firebase.Auth.AuthResult taskResult = await createAccTask;
            userId = taskResult.User.UserId;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        var addTask = db.Collection("Users").Document(userId).SetAsync(new Dictionary<string, List<string>>{{"Games", new List<string>{}}});
        try
        {
            await addTask;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        Debug.Log(userId);        
        
    }

    public async void SigninHandlerAsync()
    {
        var auth = FirebaseAuth.DefaultInstance;
        // Modified userId = "";
        var signInTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        try
        {
            var taskResult = await signInTask;
            userId = taskResult.User.UserId;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
        Debug.Log(userId);
    }

    private async void AddGameToUserAsync()
    {
        var getUserDocTask = db.Collection("Users").Document(userId).GetSnapshotAsync();
        DocumentSnapshot userDoc = await getUserDocTask;
        Dictionary<string, object> userData = userDoc.ToDictionary();
        
        List<object> userGames;
        if (userData.ContainsKey("Games"))
        {
            Debug.Log("Found games");
            userGames = userData["Games"] as List<object>;
            if (userGames != null)
            {
                userGames.Add(gameId);
                Debug.LogFormat("updated games {0}", userGames);
            }
            else
            {
                Debug.Log("Error parsing games");
                
                userGames = new List<object>() { gameId };
            }
        }
        else
        {
            userGames = new List<object>() { gameId };
        }
        var addGameTask = db.Collection("Users").Document(userId).UpdateAsync(new Dictionary<string, object>{ { "Games", userGames } });
        Debug.Log(userGames);
        try
        {
            await addGameTask;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    
    public async void InitNewGameAsync()
    {
        isFirst = 1;
        System.Random rnd = new System.Random();
        List<int> existingCode = new List<int>();
        Query allGames = db.Collection("Games");
        var getExistCodeTask = allGames.GetSnapshotAsync();
        QuerySnapshot allGamesSnapshot = await getExistCodeTask;
        foreach (DocumentSnapshot gameDoc in allGamesSnapshot.Documents)
        {
            Dictionary<string, object> game = gameDoc.ToDictionary();
            if (game.ContainsKey("Code"))
            {
                existingCode.Add(Convert.ToInt32(game["Code"]));
            }
        }
        Debug.Log(existingCode);
        int gameCode = 0;
        do
        {
            gameCode = rnd.Next(100000, 1000000);

        } while (existingCode.Contains(gameCode));
        Debug.Log(gameCode);
        gameCodeGenerated = gameCode;

        Dictionary<string, object> newGame = new Dictionary<string, object>
        {
            { "Code", gameCode },
            { "User1", userId },
            { "StartTime", FieldValue.ServerTimestamp },
            {"withdraw_confmed", 0},
            {"withdraw_inited", false},
            {"withdraw_initer", ""}
        };
        var addTask = db.Collection("Games").AddAsync(newGame);
        try
        {
            DocumentReference addedDoc = await addTask;
            gameId = addedDoc.Id;
            AddGameToUserAsync();
            
            Debug.LogFormat("Game ID: {0}", gameId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        ListenForJoin();
        GameCodeDisplay.text = gameCodeGenerated.ToString();
    }
    

    public async void JoinGameAsync()
    {
        isFirst = 0;
        // TODO: Return after Update
        Debug.Log("Joining");
        Query gameQuery = db.Collection("Games").WhereEqualTo("Code", gameCodeInput);

        var getQueryTask = gameQuery.GetSnapshotAsync();
        QuerySnapshot gameSnapshot;
        try
        {
            gameSnapshot = await getQueryTask;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
        
        if (gameSnapshot.Documents.Count() == 1)
        {
            DocumentSnapshot gameDoc = gameSnapshot.Documents.ElementAt(0);
            gameId = gameDoc.Id;
            Debug.LogFormat("Game ID found{0}", gameId);
            DocumentReference userGameDoc = db.Collection("User").Document(userId);
            AddGameToUserAsync();

            DocumentReference gameRef = db.Collection("Games").Document(gameId);
            Dictionary<string, object> updateGame = new Dictionary<string, object>
            {
                {"User2", userId}
            };
            var updateTask = gameRef.UpdateAsync(updateGame);
            try
            {
                await updateTask;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
            Debug.Log("User 2 Joined!!!!!!");
            StartCanvas.SetActive(false);
            GameCanvas.SetActive(true);
        }
    }

    public bool IsStartPlayer()
    {
        return isFirst != 0;
    }

    public async void MoveChess(int from, int to)
    {
        // Coord: [0]from [1]to
        CollectionReference gameCol = db.Collection("Games").Document(gameId).Collection("Steps");
        var getStepsTask = GetStepCnt();
        int stepCnt = await getStepsTask;
        // Todo: NOT REACH?
        currentStepCnt = stepCnt;
        var addStepDocTask = gameCol.Document(stepCnt.ToString()).SetAsync(new Dictionary<string, object>{{"from", from}, { "to", to }, {"update_user", userId}});
        Debug.Log("update user field added");
        try {
            await addStepDocTask;
        } catch(Exception e) {
            Debug.Log(e);
            throw;
        }

        string debugString = "User with email " + email + " moved from " + from.ToString() + " to " + to.ToString();
        ChessMoveDebugTxt.SetText(debugString);
        

    }

    private async Task<int> GetStepCnt()
    {
        CollectionReference gameCol = db.Collection("Games").Document(gameId).Collection("Steps");
        var getStepsTask = gameCol.GetSnapshotAsync();
        QuerySnapshot allStepsDoc = await getStepsTask;
        if (allStepsDoc.Documents.Count() == 0)
        {
            Debug.Log("Steps collection doesnt exist");
            
        }
        int stepCnt = allStepsDoc.Documents.Count() + 1;

        return stepCnt;
    }

    public async void WithdrawCallbackRegister(string userId, string gameId, Func<List<Dictionary<string, object>>,bool> updateMove)
    {
        DocumentReference gameDocRef = db.Collection("Games").Document(gameId);
        gameDocRef.Listen(snapshot => 
        {
            Dictionary<string, object> game = snapshot.ToDictionary();
            // Present Withdraw Confirmation
            
            if (game["withdraw_initer"].ToString() != userId && Convert.ToInt16(game["withdraw_confmed"]) == 0)
            {
                // TODO: call withdraw confirmation handler
                
                gameDocRef.UpdateAsync(new Dictionary<string, object>{{"withdraw_confmed", 1}});
                
                
            } else if (game["withdraw_initer"].ToString() == userId )
            {
                if (Convert.ToInt16(game["withdraw_confmed"]) == 1)
                {
                    // Confirmed, delete two steps
                    GetStepCnt().ContinueWithOnMainThread(task =>
                    {
                        int stepCnt = task.Result;
                        DocumentReference stepRef = db.Collection("Games").Document(gameId).Collection("Steps").Document(stepCnt.ToString());
                        stepRef.DeleteAsync();
                        int prevStep = stepCnt - 1;
                        DocumentReference prevStepRef = db.Collection("Games").Document(gameId).Collection("Steps")
                            .Document(prevStep.ToString());
                        prevStepRef.DeleteAsync();
                    });
                }
                gameDocRef.UpdateAsync(new Dictionary<string, object>
                {
                    {"withdraw_inited", false},
                    {"withdraw_initer", ""},
                    {"withdraw_confmed", 0}
                });
            }
            
        });
    }

    public bool TestUpdateListner(List<Dictionary<string, object>> move)
    {
        Debug.Log("opponent moved");
        return true;
    }

    private bool testUpdateMove(int a, int b)
    {
        Debug.Log("hello");
        return true;
    }
    public async void UpdateMoveCallbackRegisterAsync(Func<int, int, bool> updateMove)
    {
        CollectionReference stepColRef = db.Collection("Games").Document(gameId).Collection("Steps");
        Debug.Log("here");
        ListenerRegistration stepListener = stepColRef.Listen(snapshot =>
        {
            Debug.Log("before foreach");
            
            foreach (DocumentChange change in snapshot.GetChanges())
            {
                // Modified
                // testtext.text = "QAQQQQ 1";
                //
                if (change.ChangeType == DocumentChange.Type.Added)
                {
                    // testtext.text = "Enter if 1";
                    Int32 addedStepId = Convert.ToInt32(change.Document.Id);
                    Debug.LogFormat("AddedStepId:{0}", addedStepId);
                    Debug.LogFormat("CurrentStepId:{0}", currentStepCnt);
                    if (addedStepId <= currentStepCnt)
                        continue;
                    Debug.LogFormat("Added document ID {0}", addedStepId);
                    Dictionary<string, object> docValue = change.Document.ToDictionary();
                    string updated_user = docValue["update_user"].ToString();
                    Debug.LogFormat("Opponent ID: {0}", updated_user);
                    Debug.LogFormat("User ID: {0}", userId);
                    if (updated_user != userId)
                    {
                        // testtext.text = "Enter if 2";
                        Debug.Log("Opponent move detected");
                        foreach (var key in docValue.Keys)
                        {
                            Debug.LogFormat("Key: {0}", key);
                        }
                        Debug.Log("REACH!!!");
                        // testtext.text = "QAQQQQ 2";
                        int from_cord = Convert.ToInt32(docValue["from"]);
                        int to_cord = Convert.ToInt32(docValue["to"]);
                        string debugString = "User with email " + email + " moved from " + from_cord.ToString() + " to " + to_cord.ToString();
                        
                        // testtext.text = debugString;
                        // not reach here!!!!!!!!!
                        // testtext.text = "start fb manger111";
                        Debug.LogFormat("from:{0}", from_cord);
                        Debug.LogFormat("to:{0}", to_cord);
                        updateMove(from_cord, to_cord);
                        Debug.Log("updateMoveComplete!!!");
                        // testtext.text = "Call OppoMove";
                    }
                }
            }
        });
    }

    public async void SaveGameAsync()
    {
        
    }
    

    // Update is called once per frame
    // ReSharper disable Unity.PerformanceAnalysis
    void Update()
    {
        
    }
}




