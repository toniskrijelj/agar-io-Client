using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : Player
{
	public static MainPlayer instance;
	private float timer;
	private const float TIMER_MAX = 10;

	List<Ball> waitedCooldown;

	int count = 0;
	float xSum = 0, ySum = 0;

	float cameraTargetSize = 10;
	Vector3 ballCenterPos;
	Vector3 final;
	bool spacePressed = false;
	bool wPressed = false;
	int totalSum = 0;

	private void Awake()
	{
		instance = this;
		waitedCooldown = new List<Ball>();
	}

	private void Start()
	{
		final = new Vector3(0, 0, -10);
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			spacePressed = true;
		}
		if(Input.GetKeyDown(KeyCode.W))
		{
			wPressed = true;
		}
	}

	private void FixedUpdate()
    {
		ClientSend.Velocity();
        if(spacePressed)
		{
			ClientSend.SendSpacePressed();
			spacePressed = false;
		}
		if(wPressed)
		{
			ClientSend.SendWPressed();
			wPressed = false;
		}
		totalSum = 0;
		count = 0;
		xSum = 0;
		ySum = 0;
		for(int i = 0; i < 16; i++)
		{
			if(balls[i] != null)
			{
				if (balls[i].GetState() != State.Flying)
				{
					balls[i].Move();
				}
				count++;
				xSum += balls[i].transform.position.x;
				ySum += balls[i].transform.position.y;
				totalSum += balls[i].GetMass();
			}
		}
		ballCenterPos.x = xSum / count;
		ballCenterPos.y = ySum / count;
		if (count > 0)
		{
			UIManager.mainCamera.transform.position = Vector3Clamp(
				UIManager.mainCamera.transform.position,
				ballCenterPos,
				3);//((mainCamera.transform.position - ballCenterPos).sqrMagnitude >= 20 * 20) ? 200 : .35f);
			//UIManager.mainCamera.orthographicSize = Convert.ToSingle(74.3981 + (7.368646 - 74.3981) / (1 + Math.Pow(totalSum / 1418.469, 1.237459)));
			cameraTargetSize = Convert.ToSingle(88.34887 + (1.081656 - 88.34887) / (1 + Math.Pow(totalMass / 5017.989, 0.539886)));
			float dir = Mathf.Sign(cameraTargetSize - UIManager.mainCamera.orthographicSize);
			UIManager.mainCamera.orthographicSize = 
				(dir == 1) ? 
				Mathf.Min(UIManager.mainCamera.orthographicSize + dir * Time.fixedDeltaTime * 10, cameraTargetSize) :
				Mathf.Max(UIManager.mainCamera.orthographicSize + dir * Time.fixedDeltaTime * 10, cameraTargetSize);
		}
		timer += Time.fixedDeltaTime;
		if (timer >= TIMER_MAX)
		{
			timer -= TIMER_MAX;
		}
	}

	public Vector3 Vector3Clamp(Vector3 startingPos, Vector3 endPos, float speed)
	{
		Vector3 movement = (endPos - startingPos).normalized * speed;
		if (Mathf.Sign(movement.x) * (startingPos.x + movement.x) > Mathf.Sign(movement.x) * endPos.x)
		{
			final.x = endPos.x;
		}
		else
		{
			final.x = startingPos.x + movement.x;
		}
		if (Mathf.Sign(movement.y) * (startingPos.y + movement.y) > Mathf.Sign(movement.y) * endPos.y)
		{
			final.y = endPos.y;
		}
		else
		{
			final.y = startingPos.y + movement.y;
		}
		return final;
	}

	public void CooldownExpired(int index)
	{
		waitedCooldown.Add(balls[index]);
	}

	public override bool KillBall(int index)
	{
		if (base.KillBall(index))
		{
			ClientHandle.instance.SetNamePromptActive();
			return true;
		}
		return false;
	}
}
