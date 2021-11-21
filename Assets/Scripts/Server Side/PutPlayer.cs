using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PutPlayer : MonoBehaviour
{
    [SerializeField] RectTransform playerSlots;
    private void Start()
    {
        playerSlots.GetChild(0).GetChild(0).GetComponent<Text>().text = PhotonNetwork.NickName;
    }
}
