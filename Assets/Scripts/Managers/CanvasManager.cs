using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CanvasManager : SingletonBehaviour<CanvasManager> {

    public Text UserID;
    public Text UserName;
    public InputField chatMessageInput;

    public IncommingChat PublicChat;

    [HideInInspector]
    public int ChatCount = 0;

    void Start() {
        SetUserID(FirebaseManager.currentUser.userID);
        SetUserName(FirebaseManager.currentUser.userNickName);
        StartCoroutine(ListenChatApply());
    }

    public void BtnSendChatMessage() {
        string chat = chatMessageInput.text.ToString().Trim();
        if (chat != "") {
            FirebaseManager.Instance().ChatMessageSendPublic(chat);
            chatMessageInput.text = "";
        }
    }

    IEnumerator ListenChatApply() {
        while (true) {
            if (FirebaseManager.Instance().MessageList.Count > ChatCount) {
                PublicChat.AddChat(FirebaseManager.Instance().MessageList[ChatCount]);
                ChatCount++;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetUserID(string userid) {
        UserID.text = userid;
    }

    public void SetUserName(string username) {
        UserName.text = username;
    }
}
