using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviour
{  
    public RectTransform playerSlots;
    public List<Player> playerList;
    public int turn;

    private void Awake()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            playerList.Add(player);
        }
        Put();
    }

    private void Put()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerSlots.GetChild(i).GetComponent<PhotonView>().TransferOwnership(playerList[i]);
            playerSlots.GetChild(i).GetChild(0).GetComponent<Text>().text = playerList[i].NickName;
        }
    }
}
