using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class ChatItem : MonoBehaviour {

    public Text t_text;
    public void SetChatText(string txt) {
        t_text.text = txt;
    }
}
