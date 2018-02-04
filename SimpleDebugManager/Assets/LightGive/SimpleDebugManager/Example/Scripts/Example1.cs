using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Example1 : MonoBehaviour {

	void Start ()
    {
		
	}
	
	void Update ()
    {
        //DebugManager.Instance.
	}

    public void OnSceneChangeButtonDown()
    {
        SceneManager.LoadScene("Example2");
    }
}
