using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenu : MonoBehaviour {
    string apiHost = "http://bossclickerapi.azurewebsites.net/"; // http://bossclickerapi.azurewebsites.net/
    public GameObject ScoreText;


    // Use this for initialization
    void Start()
    {
        WebClient webclient = new WebClient();
        List<long> scores;
        string ScoreData;

        webclient.Headers.Add("Content-Type", "application/json");
        ScoreData = webclient.DownloadString(apiHost + "api/scoreboard/");
        scores = JsonConvert.DeserializeObject<List<long>>(ScoreData);

        ScoreData = "";
        
        foreach (long l in scores)
        {
                ScoreData += l.ToString() + "\n";
        }

        ScoreText.gameObject.GetComponent<Text>().text = ScoreData;
    }
	
	// Update is called once per frame
    public void GoBack()
    {
        Application.LoadLevel("Buy");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }
}
