using UnityEngine;
using System.Collections;

public class OBBLoader : MonoBehaviour {

	public string message; 
	void Start(){
		if (!GooglePlayDownloader.RunningOnAndroid()){
			Application.LoadLevel (1);
			return; 
		}
	
		string expPath = GooglePlayDownloader.GetExpansionFilePath();
	
	
		if (expPath == null){
			Debug.Log("External storage is not available!");
			message = "External storage is not available!"; 
			Application.LoadLevel(1);
		} else {
			string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
			
			//string patchPath = GooglePlayDownloader.GetPatchOBBPath(expPath);
			if (mainPath == null){       
				message = "FetchOBB : " + mainPath; 
				GooglePlayDownloader.FetchOBB();
			} 
			//We get here, we should have the main path for the OBB file
		
			//StartCoroutine(CoroutineLoadLevel());
			Application.LoadLevel(1);
		}
	
	}
	
	void OnGUI(){
		GUI.Label(new Rect(10, 10, Screen.width-10, 20), message);
	}
	
	
	
	/*protected IEnumerator CoroutineLoadLevel() { 
		bool testResourceLoaded = false;
		
			while(!testResourceLoaded) { 
				yield return new WaitForSeconds(0.5f); //Let's not constantly check, but give a buffer time to the loading
		
				if(Resources.Load("PenguinTest01") != null) {
					testResourceLoaded = true;
				}
		
			}
		
		//Everything should be loaded now
		Application.LoadLevel(1);
	
	}*/
	

}
