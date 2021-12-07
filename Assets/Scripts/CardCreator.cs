using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CardCreator : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject dealButton;
    private string[] shapes = {"H", "D", "C", "S"};
    public string[] types = {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q"};
    private List<string> deck =  new List<string>();
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite[] shapeSprites;
    [SerializeField] private Sprite[] jokerSprites;


    private void Start() 
    {
        if (PhotonNetwork.IsMasterClient)
        {
            createDeck();
            putDeck();
            dealButton.SetActive(true);
        }
        else
        {
            Destroy(dealButton);
        }
    }

    private void createDeck()
    {
        foreach (var shape in shapes)
        {
            foreach (var type in types)
            {
                deck.Add(shape + type);
            }
        }
        deck.Add("HK");
    }

    private void putDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            PhotonNetwork.Instantiate(cardPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }

    public void dealCards()
    {

        int temp = transform.childCount;
        for (int i = 0; i < temp; i++)
        {
            int randomInt = Random.Range(0, transform.childCount);
            transform.GetChild(randomInt).GetComponent<Card>().ParentTo(gameController.playerSlots.GetChild(i % gameController.playerList.Count).name);
        }
        gameController.updatePlayerHand();
        Destroy(dealButton);
        gameController.passButton.SetActive(true);
    }
}
