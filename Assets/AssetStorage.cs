using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class AssetStorage : MonoBehaviour {
	
	
	public static AssetStorage Instance{
		get; 
		set; 
	}
	
	public string[] PrefabNames; 
	public string prefab = "Penguin"; 
	public string AssetBundleName; 
	public string message = "NONE"; 
	public GameObject loadingScreen; 
	public string ServerURL; 
	public int AssetVersionNumber = 0; 
	private AssetBundleRequest request; 
	public int ChangeToScene = 1;
	
	private Dictionary<string, GameObject> loadedAssets; 
	private string mainPath; 
	private string expPath; 
	private WWW www;
	private bool needServerDownload = false; 
	private AssetBundle bundle; 
	
	void Awake() {
    	DontDestroyOnLoad(gameObject);
		
		if(Instance == null)
			Instance = this; 
		
	}
	
	void OnDestroy(){
		Instance = null; 
		//Debug.LogError("Destroy");
	}
	
	
	
	// Use this for initialization
	void Start () {
		loadedAssets = new Dictionary<string, GameObject>(); 
		needServerDownload = false; 
		if(loadingScreen != null && loadingScreen.renderer != null)
			loadingScreen.renderer.enabled = true; 
			
		if(GooglePlayDownloader.RunningOnAndroid()){
			expPath = GooglePlayDownloader.GetExpansionFilePath();
			message = expPath; 
			mainPath = string.Empty; 
			if (expPath == null){
				needServerDownload = true; 
			} else {
				message = "Try get MainPath" ; 
				Debug.Log(message);
				mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				Debug.Log("Main Path after first Fetch: " + mainPath);
				message = "MainPath: " + mainPath; 
			}
			
			if(mainPath == null){ // no OBB file -> download from Server
				needServerDownload = true; 
			}
		
		}
		StartCoroutine(Load()); 	
		
	}
	
	IEnumerator Load(){
		
		string filePath = string.Empty; 
		if(GooglePlayDownloader.RunningOnAndroid()){	
			
	
			if(mainPath == null){
				needServerDownload = true; 
				
				message = "MainFile still not found!"; 
				Debug.Log(message); 
				
			} 
			
			if(needServerDownload){
				filePath = ServerURL + "/" + AssetBundleName + ".unity3d"; 
			} else {
				filePath = "jar:file://" + mainPath;
				filePath += "!/"+AssetBundleName+".unity3d";	
			}

		} else {
			filePath = "file://" + Application.dataPath; 
			filePath += "/"+AssetBundleName+".unity3d"; 
			
		}
			
		
		
		message = "FilePath: " + filePath; 
		Debug.Log(message); 
		
		
		www = WWW.LoadFromCacheOrDownload(filePath,AssetVersionNumber); 
		yield return www;
		bundle = www.assetBundle; 
		loadedAssets.Clear(); 
		foreach(var name in PrefabNames){
			GameObject go = bundle.Load(name, typeof(GameObject)) as GameObject; 
			loadedAssets.Add(name, go); 
		}
		
		if(loadingScreen != null && loadingScreen.renderer != null)
			loadingScreen.renderer.enabled = false; 
		
		Application.LoadLevel(ChangeToScene);
	}
	
	public string InstancePrefab(string name){
		if(loadedAssets.ContainsKey(name)){
			Instantiate(loadedAssets[name]); 	
		} else {
			return ("Cannot find Asset with name: " + name); 
		}
		
		return ("Instance Finished: " + name ) ; 
	}
	
	void OnGUI(){
		GUI.Label(new Rect(10, 10, Screen.width-10, 20), message);
	}
	
	
	
}
