using UnityEngine;
using System.Collections;

public class TestMove : MonoBehaviour {

	private float val = 0.0f; 
	private bool down = false; 
	private Vector3 pos; 
	// Use this for initialization
	void Start () {
		pos = gameObject.transform.position; 
	}
	
	// Update is called once per frame
	void Update () {
		
		if(val > 0.05)
			down = true; 
		if(val < 0 )
			down = false; 
		if(!down)
			val += 0.05f * Time.deltaTime; 
		else 
			val -= 0.05f * Time.deltaTime; 
		
		gameObject.transform.position = new Vector3(pos.x, pos.y+val, pos.z); 
	}
}
