using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using SimpleJSON;
using Newtonsoft.Json;

public class FirebaseManager : SingletonBehaviour<FirebaseManager> {

    DatabaseReference mDatabaseRef;
    FirebaseAuth auth;
    FirebaseUser user;
    FirebaseUser newUser;

    public static User currentUser = new User();
    public List<ChatMessage> MessageList = new List<ChatMessage>();

    void Start() {
        DontDestroyOnLoad(this.gameObject);

        auth = FirebaseAuth.DefaultInstance; // auth init
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://unitychatpublicapp.firebaseio.com/");// database init
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;

        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            newUser = task.Result;
            UserDataInitate(newUser.UserId);
            Debug.LogFormat("User signed in successfully: " + newUser.UserId);
        });
        DatabaseReference PublicChatRef = FirebaseDatabase.DefaultInstance.GetReference("PublicChat");
        PublicChatRef.ChildAdded += HandleChildAdded;
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    public void BtnLogout() {
        auth.SignOut();
    }

    [HideInInspector]
    public bool newUserSet = false;
    private void UserDataInitate(string userID) {
        mDatabaseRef.Child("users").OrderByKey().EqualTo(userID).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.Log(task);
                // Handle the error...
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value == null) {
                } else {
                    JSONNode N = JSON.Parse(snapshot.GetRawJsonValue());
                    if (N.Count != 0) {
                        User user = new User(N[userID]["userID"], N[userID]["userNickName"]);
                        currentUser = user;
                    }
                }
                newUserSet = true;
            }
        });
    }

    [HideInInspector]
    public bool writeUserComplete = false;// trigger flag
    public void WriteNewUser(string userNickName, bool newPlayer, bool newName) {
        writeUserComplete = false; writeUserComplete = false;
        if (newPlayer) {
            FirebaseDatabase.DefaultInstance.GetReference("userCount").GetValueAsync().ContinueWith(task => {
                if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    int countUserID = int.Parse(snapshot.Value.ToString()) + 1;
                    User user = new User(countUserID.ToString(), userNickName);
                    currentUser = user;
                    string json = JsonUtility.ToJson(user);
                    mDatabaseRef.Child("users").Child(newUser.UserId).SetRawJsonValueAsync(json);
                    mDatabaseRef.Child("userCount").SetValueAsync(countUserID); // push new 
                }
                writeUserComplete = true;
            });
        } else {
            if (newName) {
                FirebaseDatabase.DefaultInstance.GetReference("userCount").GetValueAsync().ContinueWith(task => {
                    if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;
                        User user = new User(currentUser.userID, userNickName);
                        string json = JsonUtility.ToJson(user);
                        mDatabaseRef.Child("users").Child(newUser.UserId).SetRawJsonValueAsync(json);
                    }
                    writeUserComplete = true;
                });
            }
        }
        Debug.Log("Login Success");
    }

    public void ChatMessageSendPublic(string chatMessageString) {
        string uniqueID = mDatabaseRef.Child("PublicChat").Push().Key;
        ChatMessage msg = new ChatMessage(currentUser.userNickName, "PublicChat", chatMessageString);
        mDatabaseRef.Child("PublicChat").Child(uniqueID).SetRawJsonValueAsync(JsonUtility.ToJson(msg));

        Debug.Log("uniqueID" + uniqueID);
        Debug.Log("message send" + msg.ExportText());
    }

    [HideInInspector]
    public bool NewItemAdded = false;// trigger flag
    void HandleChildAdded(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(args.Snapshot.GetRawJsonValue());
        ChatMessage msg = new ChatMessage(values["senderID"], values["receiverID"], values["messageText"]);
        MessageList.Add(msg);
        Debug.Log("new " + msg.ExportText());
        NewItemAdded = true;
    }
}