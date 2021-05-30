using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public static LoginManager instance;

    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject errorPanel;

    public Text registerUsername;
    public Text registerPassword;
    public Button registerBtn;

    public Text loginUsername;
    public Text loginPassword;
    public Button loginBtn;

    public Text errorText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void Register()//registerpanel registerbutton
    {
        ClientSend.RegisterUserInfo(registerUsername.text, registerPassword.text);
    }
    public void Login()//loginpanel login button
    {
        ClientSend.LoginUserInfo(loginUsername.text, loginPassword.text);
    }

    public void OpenRegisterPanel()//login panel   -> loginpanel register button
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }
    public void OpenLoginPanel()//registerpanel   -> login button
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }
    
    public void ShowMessage(string message)
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        errorPanel.SetActive(true);

        errorText.text = message;
    }

    public void GoLobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    
    public void RegisterValueChangedVerify()//register buttonun aktif olması (value changed event)
    {
        registerBtn.interactable = (registerPassword.text.Length >= 6 && registerUsername.text.Length >= 6);
    }
    public void LoginValueChangedVerify()// login butonun aktif olması (value changed event)
    {
        loginBtn.interactable = (loginPassword.text.Length >= 6 && loginUsername.text.Length >= 6);
    }
}
