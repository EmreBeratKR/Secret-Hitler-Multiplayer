using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class CreateNJoinRoom : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public InputField nameInput;
    [SerializeField, Range(10, 20)] private int maxLength;
    [SerializeField] private List<string> illegalChars;
    [SerializeField] private string[] defaultNicknames;


    public void CreateRoom()
    {
        string c = ignoreSpace(createInput.text);
        if (c != "")
        {
            PhotonNetwork.CreateRoom(c);
        }
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        SetNickname();
        PhotonNetwork.LoadLevel("Game");
    }

    private void SetNickname()
    {
        string n = ignoreSpace(nameInput.text);
        if (n.Length == 0)
        {
            n = defaultNicknames[Random.Range(0, defaultNicknames.Length)];
        }
        else if (n.Length > maxLength)
        {
            n = n.Substring(0, maxLength);
        }
        PhotonNetwork.NickName = n;
    }

    private string ignoreSpace(string str)
    {
        string output = "";
        for (int l = 0; l < str.Length; l++)
        {
            if (!illegalChars.Contains(str[l].ToString()))
            {
                output += str[l];
            }
        }
        return output;
    }
}
