using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum State
{
	None,
	Flying,
	Cooldown
}

public class Ball : MonoBehaviour
{
	[SerializeField] TextMeshPro nameText = null;
	[SerializeField] MeshRenderer nameRenderer = null;
	[SerializeField] TextMeshPro massText = null;
	[SerializeField] MeshRenderer massRenderer = null;

	[SerializeField] protected Rigidbody2D rb = null;
	[SerializeField] SpriteRenderer sr = null;

	protected float targetSize;
	protected float currentSize;
	protected State state;
	protected string playerName;

	protected bool sameSize = true;
	protected double speed;
	protected int mass;
	protected Color color = new Color(1,1,1);

	public Player player;

	protected Vector3 scale = new Vector3(1, 1, 1);

	protected virtual void FixedUpdate()
    {
        if(!sameSize)
        {
            float sign = Mathf.Sign(targetSize - currentSize);
            currentSize = (sign == 1) ? Mathf.Min(currentSize + 100f * Time.fixedDeltaTime, targetSize) : Mathf.Max(currentSize - 100f * Time.fixedDeltaTime, targetSize);
            scale.x = currentSize;
            scale.y = currentSize;
            transform.localScale = scale;
            sameSize = currentSize == targetSize;
        }
	}

	public static Vector3 GetVectorFromAngle(int angle)
	{
		// angle = 0 -> 360
		float angleRad = angle * (Mathf.PI / 180f);
		return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
	}


	private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
			return;
        }
    }

	public float timeLastMassReceived = 0;
	public float timeLastPositionReceived = 0;

    public virtual void SetMass(int _mass)
    {
		int difference = _mass - mass;
		player.ChangeTotalMass(difference);
		mass = _mass;
		int sortingLayer = mass / 60000 + 1;
		int sortingOrder = mass % 60000 - 30000;
		nameRenderer.sortingOrder = sortingOrder + 1;
		massRenderer.sortingOrder = sortingOrder + 1;
		string layerName = $"Balls{sortingLayer}";
		nameRenderer.sortingLayerName = layerName;
		massRenderer.sortingLayerName = layerName;
		massText.text = mass.ToString();
		sr.sortingOrder = sortingOrder;
		sr.sortingLayerName = layerName;
		speed = -1143.788 + (1154.788 - -1143.788) / (1 + Math.Pow(mass / 779.436, 0.001584008));
		targetSize = Convert.ToSingle(33656.95 + (0.1347952 - 33656.95) / (1 + Math.Pow(mass / 2059981000.0, 0.5041123)));
        sameSize = false;

    }

	public void Move()
	{
		SetVelocity(GetMouseDirection(UIManager.mainCamera.ScreenToWorldPoint(Input.mousePosition)) * Convert.ToSingle(speed));
	}

	public Vector2 GetMouseDirection(Vector3 mousePosition)
	{
		Vector2 direction = mousePosition - transform.position;
		if (direction.sqrMagnitude > 1)
		{
			direction = direction.normalized;
		}
		return direction;
	}

	public void SetVelocity(Vector2 velocity)
	{
		rb.velocity = velocity;
	}

	public Vector2 GetVelocity()
	{
		return rb.velocity;
	}

	public double GetSpeed()
	{
		return speed;
	}	

	public int GetMass()
	{
		return mass;
	}

	public State GetState()
	{
		return state;
	}

	public void SetName(string name)
	{
		playerName = name;
		nameText.text = name;
	}

	public void ShootMass(Vector2 mousePosition)
	{
		if(mass >= 90)
		{
			Vector2 toMouseDireciton = (mousePosition - (Vector2)transform.position).normalized;
			Mass.CreateMass(gameObject, (Vector2)transform.position + (toMouseDireciton * (transform.localScale.x * .25f - 1)), toMouseDireciton, color);
		}
	}

	public void SetColor(Color _color)
	{
		color = _color;
		sr.color = color;
	}
}
