using UnityEngine;

public interface IDamageable
{
	void TakeHit (float damage, Vector3 hitPoint);

	void TakeDamage (float damage);

	void Heal (float amount);
}
