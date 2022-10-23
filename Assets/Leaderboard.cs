using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
	[SerializeField] List<LeaderboardPlace> top10Places = null;
	List<Player> allPlaces = new List<Player>();
	public static Leaderboard instance;

	Color redColor = Color.red;
	Color whiteColor = Color.white;

	float nextSortTime = 0;

	private void Awake()
	{
		instance = this;
		gameObject.SetActive(false);
		nextSortTime = Time.time + 1;
	}

	private void FixedUpdate()
	{
		if(nextSortTime <= Time.time)
		{
			nextSortTime += 1;
			SetLeaderboard();
		}
	}

	public void EnterLeaderboard(Player player)
	{
		gameObject.SetActive(true);
		allPlaces.Add(player);
	}

	//public void EnterLeaderboard(Player player)
	//{
	//	gameObject.SetActive(true);
	//	player.currentPlace = allPlaces.Count + 1;
	//	allPlaces.Add(player);
	//	if(player.currentPlace <= 10)
	//	{
	//		top10Places[player.currentPlace - 1].gameObject.SetActive(true);
	//		top10Places[player.currentPlace - 1].nameText.text = player.playerName;
	//		top10Places[player.currentPlace - 1].massText.text = "0";
	//		if (player.id == Client.instance.myId)
	//		{
	//			top10Places[player.currentPlace - 1].nameText.color = Color.red;
	//			top10Places[player.currentPlace - 1].massText.color = Color.red;
	//			top10Places[player.currentPlace - 1].placeText.color = Color.red;
	//		}
	//	}

	//}

	public void LeaveLeaderboard(Player player)
	{
		allPlaces.Remove(player);
	}
	//public void LeaveLeaderboard(Player player)
	//{
	//	if (!allPlaces.Contains(player)) return;
	//	int currentPlace = player.currentPlace;
	//	int count = allPlaces.Count;

	//	for (int i = currentPlace + 1; i < count; i++)
	//	{
	//		allPlaces[i].currentPlace -= 1;
	//	}
	//	int start = currentPlace;
	//	int end = Mathf.Min(10, count);
	//	if(count <= 10)
	//	{
	//		top10Places[count - 1].gameObject.SetActive(false);
	//	}
	//	for (int i = start; i < end; i++)
	//	{
	//		if (allPlaces[i].id == Client.instance.myId)
	//		{
	//			top10Places[i - 1].nameText.color = Color.red;
	//			top10Places[i - 1].massText.color = Color.red;
	//			top10Places[i - 1].placeText.color = Color.red;
	//		}
	//		else
	//		{
	//			top10Places[i - 1].nameText.color = Color.white;
	//			top10Places[i - 1].massText.color = Color.white;
	//			top10Places[i - 1].placeText.color = Color.white;
	//		}
	//		top10Places[i - 1].nameText.text = allPlaces[i].playerName;
	//		if (allPlaces[i - 1].totalMass < 1000)
	//		{
	//			top10Places[i - 1].massText.text = allPlaces[i].totalMass.ToString();
	//		}
	//		else
	//		{
	//			float mass = allPlaces[i].totalMass / 1000f;
	//			top10Places[i - 1].massText.text = mass.ToString("0.0") + "k";
	//		}
	//	}
	//	allPlaces.Remove(player);
	//}

	public void SetLeaderboard()
	{
		allPlaces.Sort();
		int count = Mathf.Min(10, allPlaces.Count);
		for(int i = 0; i < count; i++)
		{
			top10Places[i].gameObject.SetActive(true);
			if (allPlaces[i].id == Client.instance.myId)
			{
				top10Places[i].nameText.color = redColor;
				top10Places[i].placeText.color = redColor;
				top10Places[i].massText.color = redColor;
			}
			else
			{
				top10Places[i].nameText.color = whiteColor;
				top10Places[i].placeText.color = whiteColor;
				top10Places[i].massText.color = whiteColor;
			}
			top10Places[i].nameText.text = allPlaces[i].playerName.ToString();
			top10Places[i].massText.text = allPlaces[i].totalMass.ToString();
		}
		while(count < 10)
		{
			top10Places[count].gameObject.SetActive(false);
			count++;
		}
	}

	//public void SetLeaderboard(Player player, int massDifference)
	//{
	//	if (!allPlaces.Contains(player)) return;
	//	int totalMass = player.totalMass;
	//	int currentPlace = player.currentPlace;
	//	Player temp;
	//	if(massDifference > 0)
	//	{
	//		for(int i = currentPlace - 1; i > 0; i--)
	//		{
	//			if(totalMass > allPlaces[i - 1].totalMass)
	//			{
	//				allPlaces[i - 1].currentPlace += 1;
	//				temp = allPlaces[i - 1];
	//				allPlaces[i - 1] = allPlaces[i];
	//				allPlaces[i] = temp;
	//				allPlaces.Sort();
	//				player.currentPlace -= 1;
	//			}
	//			else
	//			{
	//				break;
	//			}
	//		}
	//	}
	//	else
	//	{
	//		int count = allPlaces.Count;
	//		for (int i = currentPlace + 1; i < count - 1; i--)
	//		{
	//			if (totalMass < allPlaces[i + 1].totalMass)
	//			{
	//				allPlaces[i + 1].currentPlace -= 1;
	//				temp = allPlaces[i + 1];
	//				allPlaces[i + 1] = allPlaces[i];
	//				allPlaces[i] = temp;
	//				player.currentPlace += 1;
	//			}
	//			else
	//			{
	//				break;
	//			}
	//		}
	//	}
	//	int start = Mathf.Min(currentPlace, player.currentPlace);
	//	int end = Mathf.Max(currentPlace, player.currentPlace);
	//	if(start <= 10)
	//	{
	//		end = Mathf.Min(end, 10);
	//		for(int i = start; i <= end; i++)
	//		{
	//			if(allPlaces[i - 1].id == Client.instance.myId)
	//			{
	//				top10Places[i - 1].nameText.color = Color.red;
	//				top10Places[i - 1].massText.color = Color.red;
	//				top10Places[i - 1].placeText.color = Color.red;
	//			}
	//			else
	//			{
	//				top10Places[i - 1].nameText.color = Color.white;
	//				top10Places[i - 1].massText.color = Color.white;
	//				top10Places[i - 1].placeText.color = Color.white;
	//			}
	//			top10Places[i - 1].nameText.text = allPlaces[i - 1].playerName;
	//			if(allPlaces[i - 1].totalMass < 1000)
	//			{
	//				top10Places[i - 1].massText.text = allPlaces[i - 1].totalMass.ToString();
	//			}
	//			else
	//			{
	//				float mass = allPlaces[i - 1].totalMass / 1000f;
	//				top10Places[i - 1].massText.text = mass.ToString("0.0") + "k";
	//			}
	//		}
	//	}
	//}
	
}
