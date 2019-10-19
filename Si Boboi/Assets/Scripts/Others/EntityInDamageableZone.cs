using UnityEngine;
using System.Collections;

public class EntityInDamageableZone
{
	public Collider entity;
	public bool isDamage;

	public EntityInDamageableZone (Collider entity, bool isDamage)
	{
		this.entity = entity;
		this.isDamage = isDamage;
	}
}
