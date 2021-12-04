using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public bool isOpen = false;

    private void Awake()
    {
        cardCreator = FindObjectOfType<CardCreator>();
        gameController = FindObjectOfType<GameController>();
        SetCard();
    }
    
    public void flipCard(bool isOpen)
    {
        transform.Find("Back Face").gameObject.SetActive(!isOpen);
    }

    public void ParentTo(string name)
    {
        photonView.RPC("ParentSetRPC", RpcTarget.All, name);
    }

    [PunRPC]
    private void ParentSetRPC(string name)
    {
        transform.SetParent(gameController.playerSlots.Find(name).Find("Cards"));
        transform.localPosition = Vector3.zero;
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
