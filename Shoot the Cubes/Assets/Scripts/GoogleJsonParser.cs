//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using System.Collections.Generic;
//using UnityEngine.Networking;
//
//
//[System.Serializable]
//public struct loc {
//	public string lat;
//	public string lng;
//}
//[System.Serializable]
//public struct distance {
//	public string text;
//	public int value;
//}
//	
//[System.Serializable]
//public struct duration {
//	public string text;
//	public int value;
//}
//[System.Serializable]
//public struct step {
//	public loc end_location;
//	public loc start_location;
//	public string travel_mode;
//	public distance dis;
//	public duration dur;
//	public string maneuver;
//}
//[System.Serializable]
//public struct leg {
//	public distance dis;
//	public duration dur;
//	public string end_address;
//	public loc end_location;
//	public string start_address;
//	public loc start_location;
//	public List<step> steps;
//}
//[System.Serializable]
//public struct route {
//	public List<leg> legs;
//	public string warnings;
//}
//[System.Serializable]
//public struct geoCoded {
//	public List<route> routes;
//}
//public class GoogleJsonParser : MonoBehaviour {
//
//	public List<step> steps;
//
//	public 
//	// Use this for initialization
//	void Start () {
//		StartCoroutine (GetJSON ());
//	}
//
//	IEnumerator GetJSON() {
//		string baseURL = "https://maps.googleapis.com/maps/api/directions/json?";
//		string origin = "origin="+"289%20Finch%20Ave%20W";
//		string dest = "destination="+"6170%20Bathurst%20St";
//		string mode = "mode="+"walking";
//		string apiKey = "key="+"AIzaSyBA24VtZCdo5G0dh8dR5_xiJpUkmIAAVzw";
//		string api = baseURL + origin + "&" + dest + "&" + mode + "&" + apiKey;
//		UnityWebRequest www = UnityWebRequest.Get(api);
//		yield return www.Send();
//		if(www.isError) {
//			Debug.Log(www.error);
//		}
//		else {
//			// Show results as text
//			string result = www.downloadHandler.text;
//			Debug.Log (result);
//			geoCoded g = JsonUtility.FromJson<geoCoded>(result);
//			leg l = g.routes [0].legs [0];
//			Debug.Log (l.end_location.lat);
//			Debug.Log (l.end_location.lng);
//			// countText.text = Jsonwww.downloadHandler.text;
//			List<step> steps = l.steps;
//		}
//
//	}
//
////	string parseHtmlInstruction(string s) {
////
////
////
////	}
//
//	// Update is called once per frame
//	void Update () {
//		
//	}
//}
