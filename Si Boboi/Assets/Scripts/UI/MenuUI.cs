using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour 
{
	public void StartGame ()
	{
		SceneManager.LoadScene (1);
	}

	public void QuitGame ()
	{
		Application.Quit ();
	}
}
