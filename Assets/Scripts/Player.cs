using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IComparable<Player>
{
    public int id;
	public int totalMass = 0;
	public int currentPlace = -1;
	public string playerName = "";
	public Color color;

    public Ball[] balls = new Ball[16];

	public void SetBall(int index, Ball ball)
	{
		ball.player = this;
		ball.SetColor(color);
		balls[index] = ball;
		balls[index].SetName(playerName);
	}

	public void SetName(string _name)
	{
		playerName = _name;
		for (int i = 0; i < 16; i++)
		{
			if(balls[i] != null)
			{
				balls[i].SetName(_name);
			}
		}
	}

	public void Disconnect()
	{
		for(int i = 0; i < 16; i++)
		{
			if(balls[i] != null)
			{
				Destroy(balls[i].gameObject);
			}
		}
		Leaderboard.instance.LeaveLeaderboard(this);
		Destroy(gameObject);
	}

	public void ChangeTotalMass(int difference)
	{
		totalMass += difference;
	}

	public virtual bool KillBall(int index)
	{
		if(balls[index] != null)
		{
			ChangeTotalMass(-balls[index].GetMass());
			Destroy(balls[index].gameObject);
			balls[index] = null;
			for(int i = 0; i < 16; i ++)
			{
				if(balls[i] != null)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}


	public int CompareTo(Player other)
	{
		if(totalMass > other.totalMass)
		{
			return -1;
		}
		else if(totalMass == other.totalMass)
		{
			return 0;
		}
		return 1;
	}
}