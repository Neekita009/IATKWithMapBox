using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateYAxis : MonoBehaviour {
    public GameObject container;
    public Canvas canvas;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        canvas.transform.position = container.transform.position;


	}
}
