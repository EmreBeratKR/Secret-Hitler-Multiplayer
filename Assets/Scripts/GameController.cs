using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviourPun
{  
    [SerializeField] private float gap;
    public RectTransform playerSlots;
    public RectTransform middleStack;
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

    private void PutPlayers()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerSlots.GetChild(i).GetComponent<PhotonView>().TransferOwnership(playerList[i]);
            playerSlots.GetChild(i).Find("Nickname").GetComponent<Text>().text = playerList[i].NickName;
        }
    }

    public void updatePlayerHand()
    {
        photonView.RPC("updatePlayerHandRPC", RpcTarget.All);
    }

    [PunRPC]
    private void updatePlayerHandRPC()
    {
        for (int p = 0; p < playerList.Count; p++)
        {    
            Transform hand = playerSlots.GetChild(p).Find("Cards");
            bool isMine = hand.parent.GetComponent<PhotonView>().IsMine;
            for (int c = 0; c < hand.childCount; c++)
            {
                hand.GetChild(c).localRotation = Quaternion.Euler(Vector3.zero);
                float offset = -(hand.childCount - 1) / 2f;
                hand.GetChild(c).LeanMoveLocal(Vector3.right * gap * (offset + c), 0.25f).setEaseOutQuint();
            }
        }
        revealCard();
    }

    private void revealCard()
    {
        for (int s = 0; s < playerSlots.childCount; s++)
        {
            Transform slot = playerSlots.GetChild(s);
            if (slot.GetComponent<PhotonView>().IsMine)
            {
                for (int c = 0; c < slot.Find("Cards").childCount; c++)
                {
                    slot.Find("Cards").GetChild(c).GetComponent<Card>().flipCard(true);
                }
            }
            else
            {
                for (int c = 0; c < slot.Find("Cards").childCount; c++)
                {
                    slot.Find("Cards").GetChild(c).GetComponent<Card>().flipCard(false);
                }
            }
        }
    }

    public string TransformToString(Transform transform)
    {
        return transform.parent.parent.GetSiblingIndex().ToString() + "-" + transform.GetSiblingIndex().ToString();
    }

    public Transform StringToTransform(string str)
    {
        string[] s = str.Split('-');
        return playerSlots.GetChild(Convert.ToInt32(s[0])).Find("Cards").GetChild(Convert.ToInt32(s[1]));
    }
}
