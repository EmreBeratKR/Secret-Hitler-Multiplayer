using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class CreateNJoinRoom : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public InputField nameInput;
    public Dropdown pCountDropDown;
    [SerializeField] Text warningText;
    [SerializeField, Range(10, 20)] private int maxLength;
    [SerializeField] private string[] defaultNicknames;


    private void Start()
    {
        setInputField();
    }

    private void setInputField()
    {
        createInput.characterLimit = maxLength;
        createInput.characterValidation = InputField.CharacterValidation.Alphanumeric;

        joinInput.characterLimit = maxLength;
        joinInput.characterValidation = InputField.CharacterValidation.Alphanumeric;

        nameInput.characterLimit = maxLength;
        nameInput.characterValidation = InputField.CharacterValidation.Alphanumeric;
    }


    public void CreateRoom()
    {
        string c = createInput.text;
        string n = nameInput.text;
        byte m = System.Convert.ToByte(pCountDropDown.options[pCountDropDown.value].text.Substring(0, 1));
        if (n == "")
        {
            warningText.text = "Please enter your Nickname!";
        }
        else if (c == "")
        {
            warningText.text = "Please enter a Room Name to Create!";
        }
        else
        {
            PhotonNetwork.CreateRoom(c, Room_Option(m));
        }
    }

    public void JoinRoom()
    {
        if (nameInput.text == "")
        {
            warningText.text = "Please enter your Nickname!";
        }
        else if (joinInput.text == "")
        {
            warningText.text = "Please enter a Room Name to Join!";
        }
        else
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
    }

    public override void OnJoinedRoom()
    {
        SetNickname();
        PhotonNetwork.LoadLevel(2); // Room Scene
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        warningText.text = $"Failed to Join ({joinInput.text})";
        Debug.Log(message);
        Debug.Log(returnCode);
    }

    private void SetNickname()
    {
        string n = nameInput.text;
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

    private RoomOptions Room_Option(byte max)
    {
        RoomOptions opt = new RoomOptions();
        opt.MaxPlayers = max;
        return opt;
    }
}
