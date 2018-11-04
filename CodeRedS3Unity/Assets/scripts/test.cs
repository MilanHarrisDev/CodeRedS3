using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class test : MonoBehaviour {

    public Text text;

    void Start()
    {
        StartCoroutine(GetRequest("https://us-central1-codered-451b3.cloudfunctions.net/getClientUsage?clientId=laptop"));
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
