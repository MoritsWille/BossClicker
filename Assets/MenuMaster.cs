using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMaster : MonoBehaviour {
    string spath;
    string ppath;
    public GameObject OriginalButton;
    public GameObject OriginalLtext;
    public GameObject OriginalPtext;
    public GameObject OriginalHPricetext;
    public GameObject ScoreText;
    public GameObject HappinessText;
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
            Button.name = p.ID.ToString();
            Button.transform.SetParent(Canvas, false);
            Button.AddComponent<ButtonClick>();
            Button.GetComponent<RectTransform>().localPosition = new Vector3(-75.2f, 185 - 60 * i);
            Text text = Button.transform.Find("Text").gameObject.GetComponent<Text>();
            text.text = p.name;

            GameObject Ltext = Instantiate(OriginalLtext);
            Ltext.transform.SetParent(Canvas, false);
            Ltext.name = p.ID.ToString() + "Level";
            Ltext.GetComponent<RectTransform>().localPosition = new Vector3(126, 205 - 60 * i);
            Text Lltext = Ltext.gameObject.GetComponent<Text>();
            Lltext.text = "Level " + Convert.ToString(p.owned + 1);

            GameObject CPtext = Instantiate(OriginalPtext);
            CPtext.name = p.ID.ToString() + "CPrice";
            CPtext.transform.SetParent(Canvas, false);
            CPtext.GetComponent<RectTransform>().localPosition = new Vector3(126, 185 - 60 * i);
            Text Lptext = CPtext.gameObject.GetComponent<Text>();
            Lptext.text = "-" + Convert.ToString(Math.Ceiling(Math.Pow((p.owned + 1) * p.browniePrice, 1.01))) + " BP";

            GameObject HPtext = Instantiate(OriginalHPricetext);
            HPtext.name = p.ID.ToString() + "HPrice";
            HPtext.transform.SetParent(Canvas, false);
            HPtext.GetComponent<RectTransform>().localPosition = new Vector3(126, 170 - 60 * i);
            Text Hptext = HPtext.gameObject.GetComponent<Text>();
            Hptext.text = "-" + p.happinessPrice + "% Chef Glæde";

            i++;
        }

        sdata = File.ReadAllText(spath);
        Score score = JsonConvert.DeserializeObject<Score>(sdata);

        HappinessText.gameObject.GetComponent<Text>().text = Math.Floor(score.BossHappiness).ToString() + "%";
        ScoreText.gameObject.GetComponent<Text>().text = score.CC.ToString();
    }
	
	public void LoadGame () {
        SceneManager.LoadScene("Game");
	}

    public void LoadProducts()
    {
    string pdata = File.ReadAllText(ppath);
    List<Product> DeProduct = JsonConvert.DeserializeObject<List<Product>>(pdata);

        foreach (Product p in DeProduct)
        {

            GameObject Ltext = GameObject.Find(p.ID.ToString() + "Level");
            Text Lltext = Ltext.GetComponent<Text>();
            Lltext.text = "Level " + Convert.ToString(p.owned + 1);

            GameObject CPtext = GameObject.Find(p.ID.ToString() + "CPrice");
            Text Lptext = CPtext.gameObject.GetComponent<Text>();
            Lptext.text = "-" + Convert.ToString(Math.Ceiling(Math.Pow((p.owned + 1) * p.browniePrice, 1.01))) + " BP";

            GameObject HPtext = GameObject.Find(p.ID.ToString() + "HPrice");
            Text Hptext = HPtext.gameObject.GetComponent<Text>();
            Hptext.text = "-" + p.happinessPrice + "% Chef Glæde";

            i++;
        }
    }

    public void SubtractPoints(long points, int happiness)
    {
        sdata = File.ReadAllText(spath);
        Score score = JsonConvert.DeserializeObject<Score>(sdata);
        score.CC -= points;
        score.BossHappiness -= happiness;
        HappinessText.gameObject.GetComponent<Text>().text = Math.Floor(score.BossHappiness).ToString() + "%";
        ScoreText.gameObject.GetComponent<Text>().text = score.CC.ToString();
        File.WriteAllText(spath, JsonConvert.SerializeObject(score));
    }

    public bool Funds(long price, int happiness)
    {
        sdata = File.ReadAllText(spath);
        Score score = JsonConvert.DeserializeObject<Score>(sdata);
        if (score.CC >= price && score.BossHappiness > happiness)
        {
            return true;
        }
        else return false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadGame();
        }
    }
}
