using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public bool autoTargetPlayer = true;
	public Transform target;
	public float lerpSpeed = 5;

	void Start () 
	{
		if (autoTargetPlayer)
			FindPlayer ();
	}
	
	void Update () 
	{
		if (target != null)
		{
			FollowTarget ();
		}
	}

	void FollowTarget()
	{
		Vector3 pos = Vector3.Lerp (transform.position, target.position, lerpSpeed * Time.deltaTime);
		transform.position = pos;
	}

	void FindPlayer ()
	{
		GameObject playerObj = GameObject.FindGameObjectWithTag ("Player");
		if (playerObj != null)
		{
			target = playerObj.transform;
		}
	}
}
