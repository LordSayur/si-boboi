using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	public void InstantiateClip (Vector3 position, AudioClip clip, float duration = 1)
	{
		GameObject clone = new GameObject ("One Shot Audio");
		clone.transform.position = position;
		AudioSource audioS = clone.AddComponent<AudioSource> ();
		audioS.spatialBlend = 1;
		audioS.clip = clip;
		audioS.Play ();

		Destroy (clone, duration);
	}
}
