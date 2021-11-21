using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneController : MonoBehaviour
{
    public void EnterLobby()
    {
        SceneManager.LoadScene(1); // Lobby Scene
    }

    public void EnterGame()
    {
        PhotonNetwork.LoadLevel(3); // Game Scene
    }
}
