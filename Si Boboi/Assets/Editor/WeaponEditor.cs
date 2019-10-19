using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Weapon), true)]
public class WeaponEditor : Editor 
{
	Weapon weapon;

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		weapon = (Weapon)target;

		EditorGUILayout.LabelField ("Save weapon equip position");

		if (GUILayout.Button ("Save"))
		{
			Vector3 weaponPosition = weapon.transform.localPosition;
			Vector3 weaponRotation = weapon.transform.localEulerAngles;

			weapon.equipPosition = weaponPosition;
			weapon.equipRotation = weaponRotation;
		}

		EditorGUILayout.LabelField ("Move to equip Position");

		if (GUILayout.Button ("Move"))
		{
			weapon.transform.localPosition = weapon.equipPosition;
			weapon.transform.localRotation = Quaternion.Euler (weapon.equipRotation);
		}
	}
}
