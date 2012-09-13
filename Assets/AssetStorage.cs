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
				message = "Try get MainPath" ; 
				Debug.Log(message);
				mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				Debug.Log("Main Path after first Fetch: " + mainPath);
				message = "MainPath: " + mainPath; 
			}
			
			if(mainPath == null){
				message = "OBB not available - download!"; 
				Debug.Log(message); 
				GooglePlayDownloader.FetchOBB();
				mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				message = "OBB Fetch finished" + mainPath; 
				Debug.Log(message); 
			}
		
		}
		StartCoroutine(Load()); 	
		
	}
	
	IEnumerator Load(){
		
		string filePath = string.Empty; 
		if(GooglePlayDownloader.RunningOnAndroid()){	
			bool testResourceLoaded = false;
			int count = 0; 
			while(!testResourceLoaded) { 	
				
				mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				
				//Debug.Log("Main Path after Fetch: " + mainPath);
				if(mainPath != null){
					testResourceLoaded = true; 
					//Debug.Log("Found Main Path: " + mainPath);
					break; 
				}
				count++; 
				if(count > 60)
					break; 
				yield return new WaitForSeconds(0.5f); //Let's not constantly check, but give a buffer time to the loading

	
			}
			
			if(!testResourceLoaded){
				message = "Connectionion Timeout"; 
				Debug.Log(message); 
			}
				
			else{
				
				message =  "Loading Finished";
				Debug.Log(message); 
			}

	
			if(mainPath == null){
				message = "MainFile still not found!"; 
				Debug.Log(message); 
				
			}
		
			filePath = "jar:file://" + mainPath;
			filePath += "!/"+AssetBundleName+".unity3d";
			
			
		
		} else {
			filePath = "file://" + Application.dataPath; 
			filePath += "/"+AssetBundleName+".unity3d"; 
			
		}
			
		
		
		message = "FilePath: " + filePath; 
		Debug.Log(message); 
		
		
		www = WWW.LoadFromCacheOrDownload(filePath,0); 
		yield return www;
		var bundle = www.assetBundle; 
		if(bundle == null)
			Debug.Log("bundle == 0"); 
		
		Instantiate(bundle.Load(prefab, typeof(GameObject))); 

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
