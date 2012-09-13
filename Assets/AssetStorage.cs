using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class AssetStorage : MonoBehaviour {
	
	
	public static AssetStorage Instance{
		get; 
		set; 
	}
	
	//public string[] PrefabNames; 
	public string prefab = "Penguin"; 
	public string AssetBundleName; 
	public string message = "NONE"; 
	//private Dictionary<string, AssetBundleRequest> requests = new Dictionary<string, AssetBundleRequest>(); 
	private AssetBundleRequest request; 
	
	private string mainPath; 
	private string expPath; 
	private WWW www;
	
	void Awake() {
    	DontDestroyOnLoad(gameObject);
		
		if(Instance == null)
			Instance = this; 
		
	}
	
	void OnDestroy(){
		//Instance = null; 
		Debug.LogError("Destroy");
	}
	
	
	
	// Use this for initialization
	void Start () {
		if(GooglePlayDownloader.RunningOnAndroid()){
			expPath = GooglePlayDownloader.GetExpansionFilePath();
			message = expPath; 
			mainPath = string.Empty; 
			if (expPath == null){
				message = "External storage is not available!";
				Debug.Log("External storage is not available!");
			} else {
				mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				Debug.Log("Main Path after first Fetch: " + mainPath);
				message = "MainPath: " + mainPath; 
			}
			
			if(mainPath == null){
				message = "OBB not available - download!"; 
				GooglePlayDownloader.FetchOBB(); 
				
			}
		
		}
		StartCoroutine(Load()); 	
		//Load(); 
	}
	
	IEnumerator Load(){
	//void Load(){
		
		string filePath = string.Empty; 
		if(GooglePlayDownloader.RunningOnAndroid()){	
			bool testResourceLoaded = false;
			int count = 0; 
			//while(!testResourceLoaded) { 	
				
				mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				Debug.Log("Main Path after Fetch: " + mainPath);
				if(mainPath != null){
					testResourceLoaded = true; 
					Debug.Log("Found Main Path: " + mainPath);
					//break; 
				}
				count++; 
				//if(count > 240)
				//	break; 
				//yield return new WaitForSeconds(0.5f); //Let's not constantly check, but give a buffer time to the loading
	
			//}
			
			if(!testResourceLoaded)
				message = "Connectionion Timeout"; 
			else
				message =  "Loading Finished";
			
		
		//filePath += "jar:file://" +expPath; 
	
			if(mainPath == null){
				message = "MainFile still not found!"; 	
			}
			message = "MainFile: " + mainPath;
			filePath = "jar:file://" + mainPath;
			filePath += "!/"+AssetBundleName+".unity3d";
			//filePath = mainPath; 
			
			
		
		} else {
			filePath = "file://" + Application.dataPath; 
			filePath += "/"+AssetBundleName+".unity3d"; 
		}
			
		
		
		
		message = "FilePath: " + filePath; 
		Debug.Log("FilePath: " + filePath);
		//www = new WWW(filePath);
		var bundle = WWW.LoadFromCacheOrDownload(filePath,0); 
		yield return bundle; 
		if(bundle == null)
			Debug.Log("bundle == 0"); 
		if(bundle.assetBundle == null)
			Debug.Log("Assetbundle = 0"); 
		Instantiate(bundle.assetBundle.Load(prefab, typeof(GameObject))); 
		//yield return www;
		//foreach(var prefab in PrefabNames){
			Debug.LogWarning("startLoad: " + prefab); 
			//AssetBundleRequest request = www.assetBundle.Load(prefab, typeof(GameObject));
    		//yield return request;	
			//www.LoadFromCacheOrDownload(filePath);
			Debug.LogWarning("Load: " + prefab + " finished"); 
			//requests.Add(prefab, request); 
		//}
		
		//Application.LoadLevel(1);
       message = "Finished"; 

	}
	
	public string InstancePrefab(string name){
		/*if(requests.ContainsKey(name)){
			Instantiate(requests[name].asset); 
			return ("Finished loading: " + name); 
		} else 
			return ("Asset not found: " + name); */
		//Instantiate(request.asset); 
		return ("Instance Finished: " + name ) ; 
	}
	
	void OnGUI(){
		GUI.Label(new Rect(10, 10, Screen.width-10, 20), message);
	}
	
	
	
}
