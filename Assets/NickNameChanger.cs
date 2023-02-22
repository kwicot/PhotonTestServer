using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NickNameChanger : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    private void Start()
    {
        inputField.text = PlayerPrefs.GetString("Nickname", "Nickname...");
        inputField.onEndEdit.AddListener(SaveNickname);
    }

    private void SaveNickname(string arg0)
    {
        PlayerPrefs.SetString("Nickname",inputField.text);
        PlayerPrefs.Save();
    }

    public void Ok()
    {
        SceneManager.LoadScene("Lobby");
    }
}
