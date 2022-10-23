using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
	public static Camera mainCamera;

    public GameObject startMenu;
    public InputField usernameField;
    public TMP_InputField nicknameField;
	public GameObject connectingScreen;

    private void Awake()
    {
		mainCamera = Camera.main;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

	public void ConnectToServer()
	{
		startMenu.SetActive(false);
		connectingScreen.gameObject.SetActive(true);
		usernameField.interactable = false;
		if (usernameField.text == "")
		{
			Client.instance.ip = "127.0.0.1";
		}
		else
		{
			Client.instance.ip = usernameField.text;
		}
        Client.instance.ConnectToServer();
		Invoke("DisconnectIfDidntConnect", 30);
    }

	public void DisconnectIfDidntConnect()
	{
		if (!Client.TCPConnected || !Client.UDPConnected)
		{
			connectingScreen.gameObject.SetActive(false);
			Client.instance.Disconnect();
			startMenu.SetActive(true);
			usernameField.interactable = true;
		}
		else if(Client.TCPConnected && !Client.UDPConnected)
		{
			SceneManager.LoadScene(0);
		}
	}

    public void SpawnPlayer()
    {
        ClientSend.SpawnPlayer(nicknameField.text);
    }
}
