using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
	[SerializeField] SpriteRenderer sr = null;

	float number = .2f;

	private void Awake()
	{
		Color color = new Color
		{
			a = 1
		};
		int random = Random.Range(1, 4);
		int random2 = Random.Range(1, 3);
		if(random == 1 && random2 == 1)
		{
			color.r = number;
			color.g = 1;
			color.b = Random.Range(number, 1);
			sr.color = color;
			return;
		}
		if(random == 1 && random2 == 2)
		{
			color.r = number;
			color.b = 1;
			color.g = Random.Range(number, 1);
			sr.color = color;
			return;
		}
		if(random == 2 && random2 == 1)
		{
			color.g = number;
			color.r = 1;
			color.b = Random.Range(number, 1);
			sr.color = color;
			return;
		}
		if (random == 2 && random2 == 2)
		{
			color.g = number;
			color.b = 1;
			color.r = Random.Range(number, 1);
			sr.color = color;
			return;
		}
		if (random == 3 && random2 == 1)
		{
			color.b = number;
			color.r = 1;
			color.g = Random.Range(number, 1);
			sr.color = color;
			return;
		}
		if (random == 3 && random2 == 2)
		{
			color.b = number;
			color.g = 1;
			color.r = Random.Range(number, 1);
			sr.color = color;
			return;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
