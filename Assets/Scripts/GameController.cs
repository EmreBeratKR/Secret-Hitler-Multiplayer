using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviour
{  
    [SerializeField] private float gap;
    public RectTransform playerSlots;
    public List<Player> playerList;
    public int turn;

    private void Awake()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            playerList.Add(player);
        }
        PutPlayers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int p = 0; p < playerList.Count; p++)
            {
                updatePlayerHand(p);
            }
        }

    }

    private void PutPlayers()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerSlots.GetChild(i).GetComponent<PhotonView>().TransferOwnership(playerList[i]);
            playerSlots.GetChild(i).Find("Nickname").GetComponent<Text>().text = playerList[i].NickName;
        }
    }

    public void updatePlayerHand(int player)
    {
        Transform hand = playerSlots.GetChild(player).Find("Cards");
        bool isMine = hand.parent.GetComponent<PhotonView>().IsMine;
        for (int c = 0; c < hand.childCount; c++)
        {
            if (isMine)
            {
                hand.GetChild(c).GetComponent<Card>().flipCard(true);
            }
            hand.GetChild(c).localRotation = Quaternion.Euler(Vector3.zero);
            float offset = -(hand.childCount - 1) / 2f;
            hand.GetChild(c).LeanMoveLocal(Vector3.right * gap * (offset + c), 0.25f).setEaseOutQuint();
        }
    }
}
