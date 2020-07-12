using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
    //Singleton action
    private static T instance;
    public static T Instance() {
        if (instance == null) {
            instance = (T)FindObjectOfType(typeof(T));
            string goName = typeof(T).ToString();
            GameObject go = GameObject.Find(goName);
            if (go == null) {
                go = new GameObject();
                go.name = goName;
            }
            instance = go.AddComponent<T>();
        }
        return instance;
    }

    public virtual void OnApplicationQuit() {
        instance = null;
    }

    public virtual void Awake() {
        try {
            instance = (T)FindObjectOfType(typeof(T));
        } catch (UnityException e) {
            Debug.Log(e);
        }
        
        if (instance == null) {
            instance = Instance();
        } else {
            instance = this as T;
        }
    }
}
