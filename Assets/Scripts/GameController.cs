using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviourPun
{ 
    [SerializeField] private float gap;
    [SerializeField] private Color[] nameColors;
    public GameObject passButton;
    public RectTransform playerSlots;
    public RectTransform middleStack;
    public List<Player> playerList;
    public int turn;
    public int cycle;
    public int lastCycle;
    private bool isLastTwo = false;

    private void Awake()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            playerList.Add(player);
        }
        PutPlayers();
        highlightPlayer();
    }

    private void PutPlayers()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerSlots.GetChild(i).GetComponent<PhotonView>().TransferOwnership(playerList[i]);
            playerSlots.GetChild(i).Find("Nickname").GetComponent<Text>().text = playerList[i].NickName;
        }
    }

    public void passTurn()
    {
        if (lastCycle == cycle)
        {
            photonView.RPC("passTurnRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    private void passTurnRPC()
    {
        if ((turn + 1) == playerList.Count)
        {
            cycle++;
        }

        removeWinner();

        turn = (turn + 1) % playerList.Count;
        passButton.SetActive(PhotonNetwork.LocalPlayer == playerList[turn]);

        highlightPlayer();
    }

    private void removeWinner()
    {
        Player player;
        for (int p = 0; p < playerList.Count; p++)
        {
            player = playerList[p];
            if (playerSlot(player).Find("Cards").childCount == 0)
            {
                playerList.Remove(player);
                if (playerList.Count == 2 && !isLastTwo)
                {
                    lastTwoPlayer();
                }
            }
        }
    }

    private void lastTwoPlayer()
    {
        isLastTwo = true;

        int p0 = playerSlot(playerList[0]).Find("Cards").childCount;
        int p1 = playerSlot(playerList[1]).Find("Cards").childCount;

        turn = p0 < p1 ? 0 : 1;
        passButton.SetActive(PhotonNetwork.LocalPlayer == playerList[turn]);
        highlightPlayer();
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
                Transform card = hand.GetChild(c);
                card.localRotation = Quaternion.Euler(Vector3.zero);
                float offset = -(hand.childCount - 1) / 2f;
                float yPos = card.GetComponent<Card>().isSelected ? card.localPosition.y : 0f;
                card.LeanMoveLocal(new Vector3(gap * (offset + c), yPos), 0.25f).setEaseOutQuint();
            }
        }
        revealCards();
        countCards();
    }

    private void revealCards()
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

    public void shuffleDeck()
    {
        if (isMyTurn())
        {
            Transform slot = playerSlot(PhotonNetwork.LocalPlayer).Find("Cards");
            string pattern = "";
            for (int i = 0; i < slot.childCount-1; i++)
            {
                pattern += Random.Range(0, slot.childCount).ToString() + "-";
            }
            pattern += Random.Range(0, slot.childCount).ToString();
            photonView.RPC("shuffleDeckRPC", RpcTarget.All, pattern, slot.parent.GetSiblingIndex());
        }
    }

    [PunRPC]
    private void shuffleDeckRPC(string pattern, int player)
    {
        Transform slot = playerSlots.GetChild(player).Find("Cards");
        string[] ints = pattern.Split('-');
        for (int c = 0; c < slot.childCount; c++)
        {
            slot.GetChild(c).SetSiblingIndex(System.Convert.ToInt32(ints[c]));
        }
        updatePlayerHand();
    }

    private void countCards()
    {
        PhotonView player;
        for (int p = 0; p < playerSlots.childCount; p++)
        {
            player = playerSlots.GetChild(p).GetComponent<PhotonView>();
            int count = player.transform.Find("Cards").childCount;
            player.transform.Find("Nickname").GetComponent<Text>().text = player.Owner.NickName + " (" + count + ")";
        }
    }

    private void highlightPlayer()
    {
        Transform nameText;
        for (int p = 0; p < playerList.Count; p++)
        {
            nameText = playerSlot(playerList[p]).Find("Nickname");
            if (p == turn)
            {
                nameText.GetComponent<Text>().color = nameColors[1];
            }
            else
            {
                nameText.GetComponent<Text>().color = nameColors[0];
            }
        }
    }

    public Transform playerSlot(Player player)
    {
        for (int s = 0; s < playerSlots.childCount; s++)
        {
            if (playerSlots.GetChild(s).GetComponent<PhotonView>().Owner == player)
            {
                return playerSlots.GetChild(s);
            }
        }
        return null;
    }

    public Player previousPlayer()
    {
        return playerList[(playerList.Count + turn - 1) % playerList.Count];
    }

    public bool isMyTurn()
    {
        return PhotonNetwork.LocalPlayer == playerList[turn];
    }

    public string TransformToString(Transform transform)
    {
        return transform.parent.parent.GetSiblingIndex().ToString() + "-" + transform.GetSiblingIndex().ToString();
    }

    public Transform StringToTransform(string str)
    {
        string[] s = str.Split('-');
        return playerSlots.GetChild(System.Convert.ToInt32(s[0])).Find("Cards").GetChild(System.Convert.ToInt32(s[1]));
    }
}
