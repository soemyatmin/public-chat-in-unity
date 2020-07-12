using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncommingChat : MonoBehaviour {

    public GameObject ChatItemPrefab;
    public Transform Content;
    public List<GameObject> ChatItems = new List<GameObject>();

    public void AddChat(ChatMessage ChatMessageText) {
        string ChatText = ChatMessageText.ExportText();
        GameObject chatItem = Instantiate(ChatItemPrefab);
        chatItem.GetComponent<ChatItem>().SetChatText(ChatText);
        chatItem.transform.SetParent(Content,false);
        ChatItems.Add(chatItem);
    }
}
