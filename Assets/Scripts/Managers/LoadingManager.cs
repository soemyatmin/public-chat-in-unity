using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour {
    void Start() {
        StartCoroutine(LoadLoginAsyn());
    }

    IEnumerator LoadLoginAsyn() {
        while (!FirebaseManager.Instance().newUserSet) {
            yield return new WaitForSeconds(0.5f);
        }
        SceneManager.LoadScene("Login");
    }
}
