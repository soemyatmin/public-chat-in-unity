
[System.Serializable]
public class User {
    public string userID;
    public string userNickName;

    public User() {
        userID = "0";
    }

    public User(string userID, string userNickName) {
        this.userID = userID;
        this.userNickName = userNickName;
    }

    public void setUser(string userID, string userNickName) {
        this.userID = userID;
        this.userNickName = userNickName;
    }
}