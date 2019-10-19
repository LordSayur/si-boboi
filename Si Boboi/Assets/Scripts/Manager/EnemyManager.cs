using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour 
{
	public GameObject enemy;
	public float spawnTime = 3;
	public Transform[] spawnPoints;

	PlayerStatus playerStatus;
	Transform playerTransform;

	void Start ()
	{
		GameObject playerObj = GameObject.FindGameObjectWithTag ("Player");
		if (playerObj != null)
		{
			playerStatus = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStatus> ();
			playerTransform = playerObj.transform;
		}

		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	void Spawn ()
	{
		if (playerStatus.GetCurrentHealth () <= 0)
			return;

		Transform spawnPoint = spawnNearPlayer ();

		Instantiate (enemy, spawnPoint.position, spawnPoint.rotation);
	}

	Transform spawnNearPlayer ()
	{
		Transform closeSpawnPoint = null;
		float distanceFromPlayer = Mathf.Infinity;

		foreach (Transform spawnPoint in spawnPoints)
		{
			float currentDistanceFromPlayer = (spawnPoint.position - playerTransform.position).magnitude;
			if (currentDistanceFromPlayer < distanceFromPlayer)
			{
				closeSpawnPoint = spawnPoint;
				distanceFromPlayer = currentDistanceFromPlayer;
			}
		}

		return closeSpawnPoint;
	}
}
