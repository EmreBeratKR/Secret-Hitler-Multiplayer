using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CardCreator : MonoBehaviour
{
    private string[] shapes = {"H", "D", "C", "S"};
    private string[] types = {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q"};
    private List<string> deck =  new List<string>();
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite[] shapeSprites;
    [SerializeField] private Sprite[] jokerSprites;


    private void Start() 
    {
        if (PhotonNetwork.IsMasterClient)
        {
            createDeck();
            //shuffleDeck();
            putDeck();   
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

    void shuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            string temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    private void putDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            //GameObject newCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, transform);
            //GameObject newCard = PhotonNetwork.Instantiate(cardPrefab.name, Vector3.zero, Quaternion.identity, (byte)(i+1));
            PhotonNetwork.Instantiate(cardPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }
}
