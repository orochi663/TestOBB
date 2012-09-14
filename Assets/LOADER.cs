using UnityEngine;
using System.Collections;

public class LOADER : MonoBehaviour {

	public string PrefabName; 
	//public string AssetBundleName; 
	public string message = "NONE"; 
	// Use this for initialization
	void Start () {
		message = AssetStorage.Instance.InstancePrefab(PrefabName); 
		
	}
		

	
}
