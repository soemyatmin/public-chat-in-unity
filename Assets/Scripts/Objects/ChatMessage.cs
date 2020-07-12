
[System.Serializable]
public class ChatMessage {
    public string senderID;
    public string receiverID;
    public string messageText;

    public ChatMessage(string senderID, string receiverID, string messageText) {
        this.senderID = senderID;
        this.receiverID = receiverID;
        this.messageText = messageText;
    }

    public string ExportText() {
        return "[" + senderID + "] " + messageText;
    }
}
