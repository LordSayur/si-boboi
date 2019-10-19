using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour 
{
	[Header("General")]
	public string Name;
	public Global.WeaponType weaponType;
	public float damage;

	[Header("Positioning")]
	public Vector3 equipPosition;
	public Vector3 equipRotation;

	public WeaponController user { get; set; }

	protected SoundManager soundManager;

	public abstract void Action ();
	public abstract void OpenDamageableZone();
	public abstract void CloseDamageableZone();

	protected virtual void Start ()
	{
		soundManager = GameObject.FindGameObjectWithTag ("SoundController").GetComponent<SoundManager> ();
	}
}
