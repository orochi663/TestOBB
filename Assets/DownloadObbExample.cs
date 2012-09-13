using UnityEngine;
using System.Collections;

public class DownloadObbExample : MonoBehaviour {
	
	private string mainPath; 
	private string expPath; 
	private string message; 
	private string PrefabName = "Penguin";
	
	void OnGUI()
	{
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			GUI.Label(new Rect(10, 10, Screen.width-10, 20), "Use GooglePlayDownloader only on Android device!");
			return;
		}
		
		expPath = GooglePlayDownloader.GetExpansionFilePath();
		if (expPath == null)
		{
				//GUI.Label(new Rect(10, 10, Screen.width-10, 20), "External storage is not available!");
				message = "External storage is not available!"; 
		}
		else
		{
			message = "ExpPath exists";
			mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
			string patchPath = GooglePlayDownloader.GetPatchOBBPath(expPath);
			
			GUI.Label(new Rect(10, 10, Screen.width-10, 20), "Main = ..."  + ( mainPath == null ? " NOT AVAILABLE" :  mainPath.Substring(expPath.Length)));
			GUI.Label(new Rect(10, 25, Screen.width-10, 20), "Patch = ..." + (patchPath == null ? " NOT AVAILABLE" : patchPath.Substring(expPath.Length)));
			GUI.Label(new Rect(10, 40, Screen.width-10, 20), "Patch = ..." + expPath );

			
			if (mainPath == null || patchPath == null)
				if (GUI.Button(new Rect(10, 100, 100, 100), "Fetch OBBs"))
					GooglePlayDownloader.FetchOBB();
				if (GUI.Button(new Rect(120, 100, 100, 100), "LoadAsset"))
					StartCoroutine("Load");
		}
		GUI.Label(new Rect(10, 65, Screen.width-10, 20), "M = ..." + message );
	}
	
	 IEnumerator Load() {
		Debug.Log("Tada");
        var filePath = string.Empty; 
		if(GooglePlayDownloader.RunningOnAndroid()){
			//string expPath = GooglePlayDownloader.GetExpansionFilePath();
			message = mainPath; 
			/*string mainPath = string.Empty; 
			if (expPath == null){
				message = "External storage is not available!";
				Debug.Log("External storage is not available!");
			} else {
				mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				message = mainPath; 
			}*/
			//filePath += "jar:file://" + mainPath;  
			filePath += "jar:file:/" +expPath; 
			
		} else 
			filePath += "file://" + Application.dataPath; 
		filePath += "/Pux01.unity3d"; 
		
		var www = new WWW(filePath);
		yield return www;
		AssetBundleRequest request = www.assetBundle.LoadAsync(PrefabName, typeof(GameObject));
    	yield return request;
		Instantiate(request.asset); 
    }
}
