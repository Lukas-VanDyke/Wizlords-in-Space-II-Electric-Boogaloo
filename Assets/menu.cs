using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour {
    
    private int i;

	// Use this for initialization
	void Start () {
        i = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.anyKeyDown && i == 0){
            GetComponent<Transform>().Translate(new Vector3(0f, 0f, 1.0f));
            i++;
		}
        else if(Input.anyKeyDown && i == 1)
        {
            Application.LoadLevel("level1");
        }
	}
}
