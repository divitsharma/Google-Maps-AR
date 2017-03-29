using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


[System.Serializable]
public struct loc {
	public float lat;
	public float lng;
}
[System.Serializable]
public struct distance {
	public string text;
	public int value;
}

[System.Serializable]
public struct duration {
	public string text;
	public int value;
}
[System.Serializable]
public struct step {
	public loc end_location;
	public loc start_location;
	public string travel_mode;
	public distance dis;
	public duration dur;
	public string maneuver;
}
[System.Serializable]
public struct leg {
	public distance dis;
	public duration dur;
	public string end_address;
	public loc end_location;
	public string start_address;
	public loc start_location;
	public List<step> steps;
}
[System.Serializable]
public struct route {
	public List<leg> legs;
	public string warnings;
}
[System.Serializable]
public struct geoCoded {
	public List<route> routes;
}

public class FixedArrowPointer : MonoBehaviour {

    public Text headingText;
    public Text locationText;
    public Text bearingText;
    public Text distanceText;
	float lat;
	float lon;
	float destLat;
	float destLon;
	int count;
	List<step> steps = null;

    float brng;
    float compassBrng;
    public string query;

    public GameObject panelPrefab;
    public Transform directionsPanel;

    bool open = true;

	IEnumerator GetJSON() {
		string baseURL = "https://maps.googleapis.com/maps/api/directions/json?";
		string origin = "origin="+ "43.428887,-80.476235";
        string dest = "destination=" + query;
		string mode = "mode="+"walking";
		string apiKey = "key="+"AIzaSyBA24VtZCdo5G0dh8dR5_xiJpUkmIAAVzw";
		string api = baseURL + origin + "&" + dest + "&" + mode + "&" + apiKey;
		UnityWebRequest www = UnityWebRequest.Get(api);
		yield return www.Send();
		if(www.isError) {
			Debug.Log(www.error);
		}
		else {
			string result = www.downloadHandler.text;
			Debug.Log (result);
			geoCoded g = JsonUtility.FromJson<geoCoded>(result);
			leg l = g.routes [0].legs [0];
			Debug.Log (l.end_location.lat);
			Debug.Log (l.end_location.lng);
			// countText.text = Jsonwww.downloadHandler.text;
			steps = new List<step>(l.steps);
            
			Debug.Log (steps [0].end_location.lng);
			Debug.Log (steps [0].end_location.lat);

        }

        // Initialize directions array
        GameObject[] panels = new GameObject[1];
        panels[0] = Instantiate(panelPrefab, directionsPanel);
        var texts = panels[count].GetComponentsInChildren<Text>();
        texts[0].text = "Distance here";
        texts[1].text = steps[count].maneuver; // description
        texts[2].text = "Step " + (count+1) + " / " + steps.Count;
        texts[3].text = steps[count].end_location.lat + ", " + steps[count].end_location.lng;

        //panels[1] = Instantiate(panelPrefab, directionsPanel);
        //var texts2 = panels[count+1].GetComponentsInChildren<Text>();


        //while true so this function keeps running once started.
        while (true)
		{
			Debug.Log ("in the loop");
            //check if user has location service enabled
            if (!Input.location.isEnabledByUser)
				yield break;

			Debug.Log ("after first break");
			// Start service before querying location
			Input.location.Start();

			// Wait until service initializes
			int maxWait = 4;
			while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
			{
				yield return new WaitForSeconds(1);
				maxWait--;
			}

			 //Service didn't initialize in 20 seconds
			if (maxWait < 1)
			{
				print("Timed out");
				yield break;
			}

			// Connection has failed
			if (Input.location.status == LocationServiceStatus.Failed)
			{
                print("Unable to determine device location");
				yield break;
			}
			else
			{
				lat = Input.location.lastData.latitude;
				lon = Input.location.lastData.longitude;
				//locationText.text = "Location: " + lat + ", " + lon;

				if (steps != null)
				{
					destLat = steps[count].end_location.lat;
                    destLon = steps[count].end_location.lng;
                    float λ = destLon - lon;

					var y = Mathf.Sin(λ) * Mathf.Cos(destLat);
					var x = Mathf.Cos(lat) * Mathf.Sin(destLat) -
						Mathf.Sin(lat) * Mathf.Cos(destLat) * Mathf.Cos(λ);
					brng = Mathf.Rad2Deg * Mathf.Atan2(y, x);
					if (brng < 0)
					{
						brng = 360 + brng;
					}
					compassBrng = Input.compass.trueHeading;

                    int distance = Mathf.RoundToInt(distance_metres(lat, lon, destLat, destLon));

                    // constantly update distance shown
                    texts[0].text = distance.ToString() + "m";



                    /*distanceText.text = distance.ToString();
					bearingText.text = brng.ToString();
                    headingText.text = count.ToString() + "  " + steps.Count;*/

                    if (isCollide())
                    {
                        Destroy(panels[0]);
                        //panels count = panels[count+1]?
                        count++;
                        if (count < steps.Count)
                        {
                            panels[0] = Instantiate(panelPrefab, directionsPanel);
                            texts = panels[count].GetComponentsInChildren<Text>();
                            texts[1].text = steps[count].maneuver; // description
                            texts[2].text = "Step " + (count+1) + " / " + steps.Count;
                            texts[3].text = steps[count].end_location.lat + ", " + steps[count].end_location.lng;
                        }
                    }
					if (count == steps.Count)
					{

					}
				}
				Input.location.Stop();
			}
		}

	}

    // Use this for initialization
    void Start () {
        query = GameObject.FindGameObjectWithTag("Info").GetComponent<InfoManager>().query;
        Input.location.Start();
        Input.compass.enabled = true;
		StartCoroutine (GetJSON ());
//        StartCoroutine("GetCoordinates")s;
    }


    bool isCollide() {
		lat = Input.location.lastData.latitude;
		lon = Input.location.lastData.longitude;
		if (lat - destLat <= 0.0005f && lat - destLat >= -0.0005f) {
			if (lon - destLon <= 0.0005f && lon - destLon >= -0.0005f) {
				return true;
			}
		}

		return false;
	}

    float distance_metres (float lat1, float lon1, float lat2, float lon2)
    {
        float R = 6378.137f; // Radius of Earth in KM
        float dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        float dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) + Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) * Mathf.Sin(dLon / 2)
            * Mathf.Sin(dLon / 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        float d = R * c;
        return d * 1000f;
    }

	// Update is called once per frame
	void Update () {
        this.transform.eulerAngles = new Vector3(66.953f, 0, -(brng - compassBrng));

    }

    public void ToggleCanvas()
    {
        if (open)
        {
            directionsPanel.GetComponent<CanvasGroup>().alpha = 0;
            open = false;
        }
        else
        {
            directionsPanel.GetComponent<CanvasGroup>().alpha = 1;
            open = true;
        }
    }
}
