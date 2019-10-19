using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class WeaponController : MonoBehaviour 
{
	[Header("General")]
	public Weapon defaultWeapon;

	[Header("Others")]
	public Transform weaponHolder;
	public Transform damageableZone;

	Weapon currentWeapon;
	int weaponType;

	Animator animator;

	void Start ()
	{
		animator = GetComponent<Animator> ();

		Equip (defaultWeapon);
	}

	void Update ()
	{
		Animate ();
	}

	public void Equip (Weapon weaponToEquip)
	{
		if (currentWeapon != null)
			Destroy (currentWeapon.gameObject);
		
		currentWeapon = Instantiate (weaponToEquip) as Weapon;

		currentWeapon.user = this;

		currentWeapon.transform.SetParent (weaponHolder);
		currentWeapon.transform.localPosition = weaponToEquip.equipPosition;
		currentWeapon.transform.localRotation = Quaternion.Euler (currentWeapon.equipRotation);
	}
		
	public void PerformAction ()
	{
		currentWeapon.Action ();
	}

	void Animate ()
	{
		switch (currentWeapon.weaponType)
		{
		case Global.WeaponType.Melee:
			weaponType = 0;
			break;
		case Global.WeaponType.Pistol:
			weaponType = 1;
			break;
		case Global.WeaponType.Rifle:
			weaponType = 2;
			break;
		}

		animator.SetInteger ("WeaponType", weaponType);
	}

	public Animator GetAnimator ()
	{
		return animator;
	}

	public void OpenDamageableZone ()
	{
		currentWeapon.OpenDamageableZone ();
	}

	public void CloseDamageableZone ()
	{
		currentWeapon.CloseDamageableZone ();
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.black;

		Gizmos.DrawWireCube (damageableZone.position, Vector3.one);
	}
}
