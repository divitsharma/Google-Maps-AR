using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class InfoManager : MonoBehaviour {

    public string query;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Input ()
    {
        
    }

    public void Go()
    {
        query = GameObject.FindGameObjectWithTag("DestField").GetComponent<InputField>().text;

        var arr = query.Split(' ');
        query = String.Join("%20", arr);
        Debug.Log(query);

        SceneManager.LoadScene("ShootTheCubesMain");
        
    }
}
