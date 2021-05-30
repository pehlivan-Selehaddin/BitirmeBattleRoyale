using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager instance;
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        else if (instance!=this)
        {
            Destroy(this);
        }
    }
    public void FindMatch()
    {
        ClientSend.FindMatch();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
