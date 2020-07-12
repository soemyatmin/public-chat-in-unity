using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : SingletonBehaviour<Login> {

    public InputField inputPlayerName;

    bool newPlayer = true;
    bool newName = false;

    private void Start() {
        Debug.Log("Current user ID " + FirebaseManager.currentUser.userID);
        if (FirebaseManager.currentUser.userID != "0") {
            SetPlayerName(FirebaseManager.currentUser.userNickName);
        }
    }

    public void BtnEnter() {
        if (inputPlayerName.text.ToString().Trim() == "") {
            Debug.Log("Types Name");
            return; // no input
        } else {
            FirebaseManager.Instance().WriteNewUser(inputPlayerName.text.ToString().Trim(), newPlayer, newName);
            StartCoroutine(sc());
        }
    }

    IEnumerator sc() {
        while (!FirebaseManager.Instance().writeUserComplete) {
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.LoadScene("ChatingScene");
    }

    public void NameChange() { // called by on value change
        newName = true;
    }

    public void SetPlayerName(string playerName) {
        inputPlayerName.text = playerName;
        newPlayer = false;
    }

    public string GetPlayerName() {
        return inputPlayerName.text;
    }

    public void BtnLogout() {
        FirebaseManager.Instance().BtnLogout();
    }
}
