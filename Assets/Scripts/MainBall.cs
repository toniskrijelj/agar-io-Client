using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBall : Ball
{
	Vector2 launchVelocity;
	CircleCollider2D circleCollider;

	protected float cooldownTimer = 0;
	protected float totalCooldownTimer = 10;

	protected float flyingTimer = 0;
	protected const float FLYING_TIMER_MAX = .8f;

	float startTime;
	private void Awake()
	{
		circleCollider = GetComponent<CircleCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		cooldownTimer = 0;
		startTime = Time.time;
	}

	private void Cooldown()
	{
		state = State.Cooldown;
		//cooldownTimer = 0;
		gameObject.layer = 10;
	}

	private void Launch(Vector3 mousePosition)
	{
		state = State.Flying;
		flyingTimer = 0;
		gameObject.layer = 13;
		launchVelocity = GetMouseDirection(mousePosition) * 50;
		rb.velocity = launchVelocity;
	}

	private void Start()
	{
		Launch(UIManager.mainCamera.ScreenToWorldPoint(Input.mousePosition));
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		cooldownTimer += Time.fixedDeltaTime;
		if (state == State.Flying)
		{
			flyingTimer += Time.fixedDeltaTime;
			rb.velocity = (1f - flyingTimer) * launchVelocity;
			if (flyingTimer >= FLYING_TIMER_MAX)
			{
				Cooldown();
			}
		}
		else
		{
			if (state == State.Cooldown)
			{
				if (cooldownTimer >= totalCooldownTimer)
				{
					state = State.None;
					gameObject.layer = 8;
				}
			}
		}
	}

	public override void SetMass(int _mass)
	{
		base.SetMass(_mass);
		float newTimer = 0.003030303f * _mass + 29.69697f;
		if(newTimer > totalCooldownTimer)
		{
			if (state != State.Flying)
			{
				Cooldown();
			}
		}
		totalCooldownTimer = newTimer;
	}
}
