using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
	public GameObject namePrompt = null;
	public static ClientHandle instance;
	private void Awake()
	{
		instance = this;
	}

	public void SetNamePromptActive()
	{
		namePrompt.SetActive(true);
	}

	public static void Welcome(Packet _packet)
    {
		Client.TCPConnected = true;
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void ConnectPlayer(Packet _packet)
    {
        int _id = _packet.ReadShort();
        Player _player; 
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(GameManager.instance.localPlayerPrefab);
            GameManager.players.Add(_id, _player);
        }
        else
        {
            _player = Instantiate(GameManager.instance.playerPrefab);
            GameManager.players.Add(_id, _player);
        }
        _player.id = _id;
		_player.color = new Color(_packet.ReadFloat(), _packet.ReadFloat(), _packet.ReadFloat(), 1);
		Leaderboard.instance.EnterLeaderboard(_player);
    }

    public static void BallPosition(Packet _packet)
    {
		float time = _packet.ReadFloat();
		int playersID = _packet.ReadShort();
		Player player;
		if(GameManager.players.ContainsKey(playersID))
		{
			player = GameManager.players[playersID];
		}
		else
		{
			return;
		}
		int count = _packet.ReadShort();
		int ballsIndex;
		Vector2 position;
		for (int i = 0; i < count; i++)
		{
			ballsIndex = _packet.ReadShort();
			position = _packet.ReadVector2();
			if (player.balls[ballsIndex] != null)
			{
				if (player.balls[ballsIndex].timeLastPositionReceived < time)
				{
					player.balls[ballsIndex].transform.position = position;
					player.balls[ballsIndex].timeLastPositionReceived = time;
				}
			}
		}
    }

    public static void KillBall(Packet _packet)
    {
		int _fromClient = _packet.ReadShort();
		int _ballIndex = _packet.ReadShort();
		Player _player = GameManager.players[_fromClient];
		if (_player != null)
		{
			_player.KillBall(_ballIndex);
		}
    }

    public static void BallMass(Packet _packet)
    {
		int key = _packet.ReadShort();
		if (GameManager.players.ContainsKey(key))
		{
			Ball ball = GameManager.players[key].balls[_packet.ReadShort()];
			if (ball != null && ball.gameObject != null)
			{
				float time = _packet.ReadFloat();
				if(ball.timeLastMassReceived < time)
				{
					ball.timeLastMassReceived = time;
					ball.SetMass(_packet.ReadInt());
				}
			}
		}
	}

    public static void SpawnPlayer(Packet _packet)
    {
        int _fromClient = _packet.ReadShort(), _ballIndex = _packet.ReadShort();
		Ball ball;
		if (_fromClient == Client.instance.myId)
		{
			ball = Instantiate(GameAssets.i.mainBallPrefab, _packet.ReadVector2(), Quaternion.identity);
		}
		else
		{
			ball = Instantiate(GameAssets.i.ballPrefab, _packet.ReadVector2(), Quaternion.identity);
		}
		GameManager.players[_fromClient].SetBall(_ballIndex, ball);// balls[_ballIndex] = ball;
		ball.SetMass(_packet.ReadInt());
    }

	public static void SpawnFood(Packet _packet)
	{
		int count = _packet.ReadShort();
		for (int i = 0; i < count; i++)
		{
			Instantiate(GameAssets.i.foodPrefab, _packet.ReadVector2(), Quaternion.identity);
		}
    }

	public static void PlayerDisconnect(Packet _packet)
	{
		int playerId = _packet.ReadInt();
		Player player = GameManager.players[playerId];
		GameManager.players.Remove(playerId);
		player.Disconnect();
	}

	public static void PlayerName(Packet _packet)
	{
		int key = _packet.ReadInt();
		if (GameManager.players.ContainsKey(key))
		{
			GameManager.players[key].SetName(_packet.ReadString());
		}
	}

	public static void ShootMass(Packet _packet)
	{
		Player player = GameManager.players[_packet.ReadShort()];
		Vector2 mousePosition = _packet.ReadVector2();
		if (player == null) return;
		for(int i = 0; i < 16; i++)
		{
			if (player.balls[i] != null)
			{
				player.balls[i].ShootMass(mousePosition);
			}
		}
	}

	public static void SpawnMass(Packet _packet)
	{
		int count = _packet.ReadShort();
		int playerId;
		int ballIndex;
		GameObject sender;
		for (int i = 0; i < count; i++)
		{
			playerId = _packet.ReadShort();
			ballIndex = _packet.ReadShort();
			if (GameManager.players.ContainsKey(playerId))
			{
				if (GameManager.players[playerId].balls[ballIndex] != null)
				{
					sender = GameManager.players[playerId].balls[ballIndex].gameObject;
				}
				else
				{
					sender = null;
				}
			}
			else
			{
				sender = null;
			}

			Mass.CreateMass(sender,
				_packet.ReadVector2(),
				_packet.ReadVector2(),
				new Color(
					_packet.ReadFloat(),
					_packet.ReadFloat(),
					_packet.ReadFloat(),
					1),
				_packet.ReadFloat());
		}
	}
}
