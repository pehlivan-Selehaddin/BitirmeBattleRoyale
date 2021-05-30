using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnetToServer : MonoBehaviour
{

    [SerializeField] private GameObject TekrarDenePanel = null;
    [SerializeField] private GameObject LoadingImage = null;
    [SerializeField] private Text ConnectServerCallbackText = null;
    void Start()
    {
        StartCoroutine(ConnectServer());
    }

    string ConnecToServer()
    {
        string result = Client.instance.ConnectToServer();

        return result;
    }
    IEnumerator ConnectServer()
    {
        string result = ConnecToServer();

        if (result == string.Empty)
        {
            SceneManager.LoadScene("Lobby");
        }

        yield return new WaitForSeconds(3f);
        if (!Client.instance.tcp.socket.Connected)
        {
            ConnectServerCallbackText.text = result;
            TekrarDenePanel.SetActive(true);
            LoadingImage.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene("LoginScene");
        }
    }
    public void TekrarDene()
    {
        LoadingImage.SetActive(true);
        TekrarDenePanel.SetActive(false);
        StartCoroutine(ConnectServer());
    }
}
