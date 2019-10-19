using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour 
{
	public Weapon weapon;

	void OnTriggerEnter (Collider other)
	{
		if (other.GetComponent<WeaponController> ())
		{
			other.GetComponent<WeaponController> ().Equip (weapon);

			Destroy (gameObject);
		}
	}
}
