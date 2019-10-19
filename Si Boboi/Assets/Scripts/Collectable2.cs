using UnityEngine;
using System.Collections;

public class Collectable2 : MonoBehaviour 
{
	public HealthItem healthItem;

	void OnTriggerEnter (Collider other)
	{
		if (other.GetComponent<PlayerStatus> ())
		{
			other.GetComponent<PlayerStatus> ().Heal (healthItem.healAmount);

			Destroy (gameObject);
		}
	}
}
