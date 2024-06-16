using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void SpawnPlayer(string nickname)
    {
		UIManager.mainCamera.orthographicSize = 8;
        Vector2 spawnPosition = new Vector2();
        while(true)
        {
            spawnPosition.x = Random.Range(-148f, 148);
            spawnPosition.y = Random.Range(-148f, 148);

            if (Physics2D.OverlapCircle(spawnPosition, 1, 1 << 8) == null)
            {
                using(Packet _packet = new Packet((int)ClientPackets.spawnPlayer))
                {
                    _packet.Write(spawnPosition);
					_packet.Write(nickname);

                    SendTCPData(_packet);
                }
                break;
            }
        }
    }

    public static void SendSpacePressed()
    {
        using (Packet _packet = new Packet((int)ClientPackets.spacePressed))
        {
            SendTCPData(_packet);
        }
    }

	static int[] indexes = new int[16];

	public static void Velocity()
	{
		using (Packet _packet = new Packet((int)ClientPackets.position))
		{
			Ball[] playerBalls = MainPlayer.instance.balls;

			int count = 0;

			for(int i = 0; i < 16; i++)
			{
				if(playerBalls[i] != null)
				{
					indexes[count] = i;
					count++;
				}
			}

			_packet.Write(count);

			for(int i = 0; i < count; i++)
			{
				_packet.Write(indexes[i]);
				_packet.Write((Vector2)playerBalls[indexes[i]].transform.position);
			}

			SendUDPData(_packet);
		}
	}

	public static void SendWPressed()
	{
		using (Packet _packet = new Packet((int)ClientPackets.wPressed))
		{
			_packet.Write((Vector2)UIManager.mainCamera.ScreenToWorldPoint(Input.mousePosition));
			SendUDPData(_packet);
		}
	}
}
