using UnityEngine;
using System.Collections;

public class LOADER : MonoBehaviour {

	public string PrefabName; 
	public string message = "NONE"; 
	// Use this for initialization
	void Start () {
		StartCoroutine("Load"); 
	}
	
	 IEnumerator Load() {
		Debug.Log("Tada");
        var filePath = string.Empty; 
		if(GooglePlayDownloader.RunningOnAndroid()){
			string expPath = GooglePlayDownloader.GetExpansionFilePath();
			message = expPath; 
			string mainPath = string.Empty; 
			if (expPath == null){
				message = "External storage is not available!";
				Debug.Log("External storage is not available!");
			} else {
				mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				message = mainPath; 
			}
			//filePath += "jar:file://" + mainPath;  
			filePath += "jar:file:/" +expPath; 
			
		} else 
			filePath += "file://" + Application.dataPath; 
		filePath += "/PenguinTest01.unity3d"; 
		
		var www = new WWW(filePath);
		yield return www;
		AssetBundleRequest request = www.assetBundle.LoadAsync(PrefabName, typeof(GameObject));
    	yield return request;
		Instantiate(request.asset); 
    }
	
	void OnGUI(){
		GUI.Label(new Rect(10, 10, Screen.width-10, 20), message);
	}
	
}
