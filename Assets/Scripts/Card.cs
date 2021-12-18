using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class Card : MonoBehaviourPun
{
    [SerializeField] private Sprite[] shapeSprites;
    [SerializeField] private Sprite[] jokerSprites;
    private CardCreator cardCreator;
    private GameController gameController;
    public string shape;
    public string type;
    public bool isOpen;
    public bool isSelected;
    public byte Id { get; set; }

    private void Awake()
    {
        cardCreator = FindObjectOfType<CardCreator>();
        gameController = FindObjectOfType<GameController>();
        SetCard();
    }

    public void Click()
    {
        if (transform.parent.tag != gameController.middleStack.tag && gameController.isMyTurn())
        {
            if (photonView.IsMine)
            {
                if (!isSelected)
                {
                    isSelected = true;
                    transform.localPosition += Vector3.up * 40f;
                    checkSelected();
                }
                else
                {
                    isSelected = false;
                    transform.localPosition += Vector3.down * 40f;
                }
            }
            else
            {
                if (gameController.cycle != 0 &&
                    gameController.lastCycle != gameController.cycle &&
                    transform.parent.parent.GetComponent<PhotonView>().Owner == gameController.previousPlayer())
                {    
                    gameController.lastCycle = gameController.cycle;
                    ParentTo(gameController.entryIndex(PhotonNetwork.LocalPlayer));
                    gameController.updatePlayerHand();
                }
            }
        }
    }

    private void checkSelected()
    {
        List<Card> selected = new List<Card>();
        Card card;
        for (int c = 0; c < transform.parent.childCount; c++)
        {
            card = transform.parent.GetChild(c).GetComponent<Card>();
            if (card.isSelected)
            {
                selected.Add(card);
            }
        }
        if (selected.Count == 2)
        {
            if (selected[0].type == selected[1].type)
            {
                foreach (var c in selected)
                {
                    photonView.RPC("stackToMiddle", RpcTarget.All, gameController.TransformToString(c.transform));
                }
                gameController.updatePlayerHand();
            }
            else
            {
                foreach (var c in selected)
                {
                    c.isSelected = false;
                    c.transform.localPosition += Vector3.down * 40f;
                }
            }
        }
    }
    
    public void flipCard(bool open)
    {
        transform.Find("Back Face").gameObject.SetActive(!open);
        isOpen = open;

    }

    public void ParentTo(int index)
    {
        photonView.RPC("ParentSetRPC", RpcTarget.All, index);
    }

    public Transform GetSlot(Player player)
    {
        for (int s = 0; s < gameController.playerSlots.childCount; s++)
        {
            if (gameController.playerSlots.GetChild(s).GetComponent<PhotonView>().Owner == player)
            {
                return gameController.playerSlots.GetChild(s);
            }
        }
        return null;
    }

    [PunRPC]
    private void ParentSetRPC(int index)
    {
        Transform slot = gameController.playerSlots.GetChild(index);
        transform.SetParent(slot.Find("Cards"));
        transform.LeanMoveLocal(Vector3.zero, 0.25f).setEaseOutQuint();
        if (slot.TryGetComponent(out PhotonView view))
        {
            photonView.TransferOwnership(view.Owner);
        }
    }

    [PunRPC]
    private void stackToMiddle(string card)
    {
        Transform t = gameController.StringToTransform(card);
        t.SetParent(gameController.middleStack);
        t.LeanMoveLocal(Vector3.zero, 0.25f).setEaseOutQuint();
        t.GetComponent<Card>().flipCard(true);
    }

    private void SetCard()
    {
        int id = GetComponent<PhotonView>().InstantiationId - 1000;
        string card = "";
        if (id <= 12)
        {
            card += "H" + cardCreator.types[id-1];
        }
        else if (id <= 24)
        {
            card += "D" + cardCreator.types[id-13];
        }
        else if (id <= 36)
        {
            card += "C" + cardCreator.types[id-25];
        }
        else if (id <= 48)
        {
            card += "S" + cardCreator.types[id-37];
        }
        else
        {
            card = "HK";
        }

        transform.SetParent(cardCreator.transform);
        name = card;
        GetComponent<Card>().shape = card.Substring(0, 1);
        GetComponent<Card>().type = card.Substring(1, card.Length-1);
        transform.Find("Type").GetComponent<TextMeshProUGUI>().text = card.Substring(1, card.Length-1);
        if (card.Substring(0, 1) == "H")
        {
            //newCard.GetComponent<Card>().color = "red";
            transform.Find("Type").GetComponent<TextMeshProUGUI>().color = Color.red;
            transform.Find("Shape Small").GetComponent<Image>().sprite = shapeSprites[0];
            if (card.Substring(1, 1) == "J")
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = jokerSprites[0];
            }
            else if (card.Substring(1, 1) == "Q")
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = jokerSprites[1];
            }
            else if (card.Substring(1, 1) == "K")
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = jokerSprites[2];
            }
            else
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = shapeSprites[0];
            }
        }
        else if (card.Substring(0, 1) == "D")
        {
            //newCard.GetComponent<Card>().color = "red";
            transform.Find("Type").GetComponent<TextMeshProUGUI>().color = Color.red;
            transform.Find("Shape Small").GetComponent<Image>().sprite = shapeSprites[1];
            if (card.Substring(1, 1) == "J")
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = jokerSprites[0];
            }
            else if (card.Substring(1, 1) == "Q")
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = jokerSprites[1];
            }
            else if (card.Substring(1, 1) == "K")
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = jokerSprites[2];
            }
            else
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = shapeSprites[1];
            }
        }
        else if (card.Substring(0, 1) == "C")
        {
            //newCard.GetComponent<Card>().color = "black";
            transform.Find("Type").GetComponent<TextMeshProUGUI>().color = Color.black;
            transform.Find("Shape Small").GetComponent<Image>().sprite = shapeSprites[2];
            if (card.Substring(1, 1) == "J")
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = jokerSprites[3];
            }
            else if (card.Substring(1, 1) == "Q")
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = jokerSprites[4];
            }
            else if (card.Substring(1, 1) == "K")
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = jokerSprites[5];
            }
            else
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = shapeSprites[2];
            }
        }
        else if (card.Substring(0, 1) == "S")
        {
            //newCard.GetComponent<Card>().color = "black";
            transform.Find("Type").GetComponent<TextMeshProUGUI>().color = Color.black;
            transform.Find("Shape Small").GetComponent<Image>().sprite = shapeSprites[3];
            if (card.Substring(1, 1) == "J")
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = jokerSprites[3];
            }
            else if (card.Substring(1, 1) == "Q")
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = jokerSprites[4];
            }
            else if (card.Substring(1, 1) == "K")
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = jokerSprites[5];
            }
            else
            {
                transform.Find("Shape Large").GetComponent<Image>().sprite = shapeSprites[3];
            }
        }
    }
}
