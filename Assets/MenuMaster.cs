using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MenuMaster : MonoBehaviour {
    string spath;
    string ppath;
    public GameObject OriginalButton;
    public GameObject OriginalLtext;
    public GameObject OriginalPtext;
    public GameObject ScoreText;
    public Transform Canvas;
    string sdata;
    int i = 0;

	// Use this for initialization
	void Start () {
        if (Application.platform == RuntimePlatform.Android)
        {
            ppath = Application.persistentDataPath + @"/Purchases.json";
            spath = Application.persistentDataPath + @"/Scores.json";
        }
        else
        {
            ppath = Application.dataPath + @"/Purchases.json";
            spath = Application.dataPath + @"/Scores.json";
        }

        string pdata = File.ReadAllText(ppath);
        List<Product> DeProduct = JsonConvert.DeserializeObject<List<Product>>(pdata);

        foreach (Product p in DeProduct)
        {
            GameObject Button = Instantiate(OriginalButton);
            Button.name = p.name;
            Button.transform.parent = Canvas;
            Button.AddComponent<ButtonClick>();
            Button.GetComponent<RectTransform>().localPosition = new Vector3(-75.2f, 240 - 60 * i);
            Text text = Button.transform.Find("Text").gameObject.GetComponent<Text>();
            text.text = p.name;

            GameObject Ltext = Instantiate(OriginalLtext);
            Ltext.transform.parent = Canvas;
            Ltext.name = p.name + "Level";
            Ltext.GetComponent<RectTransform>().localPosition = new Vector3(113, 240 - 60 * i);
            Text Lltext = Ltext.gameObject.GetComponent<Text>();
            Lltext.text = "Level " + Convert.ToString(p.owned + 1);

            GameObject Ptext = Instantiate(OriginalPtext);
            Ptext.name = p.name + "Price";
            Ptext.transform.parent = Canvas;
            Ptext.GetComponent<RectTransform>().localPosition = new Vector3(113, 225 - 60 * i);
            Text Lptext = Ptext.gameObject.GetComponent<Text>();
            Lptext.text = "Price: " + Convert.ToString(Math.Ceiling(Math.Pow(p.owned * p.price + 1, 2)));

            i++;
        }

        sdata = File.ReadAllText(spath);
        Score score = JsonConvert.DeserializeObject<Score>(sdata);

        ScoreText.gameObject.GetComponent<Text>().text = score.CC.ToString();
    }
	
	// Update is called once per frame
	public void LoadGame () {
        Application.LoadLevel("Game");
	}

    public void LoadProducts()
    {
    string pdata = File.ReadAllText(ppath);
    List<Product> DeProduct = JsonConvert.DeserializeObject<List<Product>>(pdata);

        foreach (Product p in DeProduct)
        {

            GameObject Ltext = GameObject.Find(p.name + "Level");
            Text Lltext = Ltext.GetComponent<Text>();
            Lltext.text = "Level " + Convert.ToString(p.owned + 1);

            GameObject Ptext = GameObject.Find(p.name + "Price");
            Text Lptext = Ptext.gameObject.GetComponent<Text>();
            Lptext.text = "Price: " + Convert.ToString(Math.Ceiling(Math.Pow(p.owned*p.price + 1, 2)));

            i++;
        }
    }

    public void SubtractPoints(long points)
    {
        Score score = JsonConvert.DeserializeObject<Score>(sdata);
        score.CC -= points;
        ScoreText.gameObject.GetComponent<Text>().text = score.CC.ToString();
        File.WriteAllText(spath, JsonConvert.SerializeObject(score));
    }

    public bool Funds(long price)
    {
        Score score = JsonConvert.DeserializeObject<Score>(sdata);
        if (score.CC >= price)
        {
            return true;
        }
        else return false;
    }
}
