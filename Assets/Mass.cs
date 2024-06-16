using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mass : MonoBehaviour
{
	//[SerializeField] float speed;
	public static Mass CreateMass(GameObject sender, Vector2 spawnPosition, Vector2 direction, Color color)
	{
		Mass mass = Instantiate(GameAssets.i.massPrefab, spawnPosition, Quaternion.identity);
		mass.rb.AddForce(direction * 1000, ForceMode2D.Force);
		mass.sender = sender;
		mass.spawnTime = Time.time;
		mass.sr.color = color;
		return mass;
	}

	public static Mass CreateMass(GameObject sender, Vector2 spawnPosition, Vector2 velocity, Color color, float spawnTime)
	{
		Mass mass = Instantiate(GameAssets.i.massPrefab, spawnPosition, Quaternion.identity);
		mass.rb.velocity = velocity;
		mass.sender = sender;
		mass.spawnTime = spawnTime;
		mass.sr.color = color;
		return mass;
	}

	[SerializeField] Rigidbody2D rb = null;
	[SerializeField] SpriteRenderer sr = null;

	public bool eaten = false;
	float spawnTime;
	GameObject sender;

	private void FixedUpdate()
	{
		if(spawnTime + 10 < Time.time)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject != sender || spawnTime + .5f < Time.time)
		{
			eaten = true;
			Destroy(gameObject);
		}
	}
}
