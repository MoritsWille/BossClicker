using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    string AndroidID;
    public Transform HP;
    public float BossHappiness;
    int mod;
    long CC;
    public Sprite happy;
    public Sprite NormalSprite;
    public Sprite YinYang;
    public GameObject CPSt;
    public GameObject CTt;
    public GameObject BHt;
    public GameObject KPItext;
    float CPS;
    Text CPStt;
    Text CTtt;
    string output;
    static string ppath;
    static string spath;
    float time;
    long lastCC;
    bool GoDown = false;
    bool blink = false;
    int blinkIter = 0;
    string apiHost = "http://bossclickerapi.azurewebsites.net/"; // http://bossclickerapi.azurewebsites.net/

    // Use this for initialization
    void Start()
    {
        DefineValues();

        if (!File.Exists(spath))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = happy;       
            BossHappiness = 100;
            GoDown = true;

            Score score = new Score();
            try
            {
                string ScoreData = "";
                WebClient webclient = new WebClient();
                ScoreDB scoredb;
                try
                {
                    ScoreData = webclient.DownloadString(apiHost + "api/score/" + AndroidID);
                    scoredb = JsonConvert.DeserializeObject<ScoreDB>(ScoreData);
                }
                catch
                {
                    scoredb = new ScoreDB { AndroidID = AndroidID, Score = 0 };
                    webclient.Headers.Add("Content-Type", "application/json");
                    webclient.UploadString(apiHost + "api/score", "POST", JsonConvert.SerializeObject(scoredb));
                }
                
                score.CC = scoredb.Score;
                score.CPS = 0;
                score.TimeQuit = DateTime.Now;
            }
            catch
            {
                Application.Quit();
            }

            output = JsonConvert.SerializeObject(score);
            File.WriteAllText(spath, output);
        }

        if (!File.Exists(ppath))
        {
            List<Product> product = new List<Product>();

            Product productA = new Product();
            productA.name = "Pirat Cola";
            productA.browniePrice = 250;
            productA.happinessPrice = 0;
            productA.owned = 0;
            productA.modifier = 1;
            productA.ID = 0;

            Product productB = new Product();
            productB.name = "Kage";
            productB.browniePrice = 2500;
            productB.happinessPrice = 0;
            productB.owned = 0;
            productB.modifier = 2;
            productB.ID = 1;

            Product productC = new Product();
            productC.name = "Frugt";
            productC.browniePrice = 10000;
            productC.happinessPrice = 10;
            productC.owned = 0;
            productC.modifier = 4;
            productC.ID = 2;

            Product productD = new Product();
            productD.name = "Bordfodbold";
            productD.browniePrice = 20000;
            productD.happinessPrice = 10;
            productD.owned = 0;
            productD.modifier = 8;
            productD.ID = 3;

            Product productE = new Product();
            productE.name = "Fredagsbar";
            productE.browniePrice = 100000;
            productE.happinessPrice = 20;
            productE.owned = 0;
            productE.modifier = 12;
            productE.ID = 4;

            Product productF = new Product();
            productF.name = "Lønforhøjelse";
            productF.browniePrice = 250000;
            productF.happinessPrice = 65;
            productF.owned = 0;
            productF.modifier = 0;
            productF.ID = 5;


            product.Add(productA);
            product.Add(productB);
            product.Add(productC);
            product.Add(productD);
            product.Add(productE);
            product.Add(productF);


            output = JsonConvert.SerializeObject(product);
            File.WriteAllText(ppath, output);
        }

        List<Product> DeProduct = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(ppath));
        foreach (Product p in DeProduct)
        {
            mod += p.modifier * p.owned;
        }

        Score cscore = LoadAll();
        TimeSpan TimeSpent = DateTime.Now - cscore.TimeQuit;
        CC += Convert.ToInt64(Math.Floor(TimeSpent.TotalSeconds) * mod);
        SubHap((float)Math.Ceiling(TimeSpent.TotalSeconds * 0.0005555555555f));
        lastCC = CC;
    }

    // Update is called once per frame
    void Update()
    {
        if(GoDown)
        {
            SubHap(0.5f);
            if (BossHappiness < 45)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = NormalSprite;
            }
            if (BossHappiness < 5)
            {
                GoDown = false;
                SaveAll();
            }
        }

        time += Time.deltaTime;

        CPStt.text = Math.Floor(CPS).ToString();
        CTtt.text = CC.ToString();

        if (time > 1)
        {
            if (blink)
            {
                if (KPItext.activeSelf)
                {
                    KPItext.SetActive(false);
                }
                else KPItext.SetActive(true);

                if (blinkIter == 6)
                {
                    blink = false;

                }
            }
            SubHap(0.0005555555555f);
            CPS = CC - lastCC;
            lastCC = CC;
            time = 0;
            CC += mod;

        }
    }

    //when user presses home button
    void OnApplicationQuit()
    {
        SaveAll();
    }

    //When user presses power button when in app
    void OnApplicationPause()
    {
        SaveAll();
    }

    //when user presses power button after being off
    void OnApplicationFocus()
    {
        Score cscore = LoadAll();
        TimeSpan TimeSpent = DateTime.Now - cscore.TimeQuit;
        CC += Convert.ToInt64(Math.Floor(TimeSpent.TotalSeconds) * mod);
        SubHap((float)Math.Ceiling(TimeSpent.TotalSeconds * 0.0005555555555f));
        lastCC = CC;
    }

    //Called from KPI buttons
    public void HandIn(string color)
    {
        Color ccolor = new Color();
        switch (color)
        {
            case "red":
                ccolor = Color.red;
                break;

            case "yellow":
                ccolor = Color.yellow;
                break;

            case "green":
                ccolor = Color.green;
                break;
        }
        GameObject Carma = new GameObject("Carma");
        Carma.gameObject.AddComponent<Carma>();
        SpriteRenderer CarmaSprite = Carma.AddComponent<SpriteRenderer>();
        CarmaSprite.sprite = YinYang;
        CarmaSprite.color = ccolor;

        CC++;
        AddHap(0.05f);
        CPS += 1;
    }

    //Called from "upgrades" button
    public void GotoBuy()
    {
        Score score = new Score();
        score.CC = CC;
        score.CPS = CPS;
        score.BossHappiness = BossHappiness;
        score.TimeQuit = DateTime.Now;

        output = JsonConvert.SerializeObject(score);
        File.WriteAllText(spath, output);
        SceneManager.LoadScene("Buy");
    }

    //Used to load all system dependant variables
    void DefineValues()
    {
        AndroidID = SystemInfo.deviceUniqueIdentifier;

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

        CPStt = CPSt.gameObject.GetComponent<Text>();
        CTtt = CTt.gameObject.GetComponent<Text>();
    }

    //adds BossHappiness
    void AddHap(float add)
    {
        if (BossHappiness + add < 45)
        {
            BossHappiness += add;
        }
        else BossHappiness = 45;

        BHt.GetComponent<Text>().text = Math.Round(BossHappiness).ToString() + "%";
        HP.position = new Vector3(HP.position.x, -0.9f + BossHappiness * (1.8f / 100), -1);
    }

    //subtracts BossHappiness
    void SubHap(float sub)
    {
        if (BossHappiness > sub)
        {
            BossHappiness -= sub;
        }
        else
        {
            CC = 0;
            BossHappiness = 0;
        }
        BHt.GetComponent<Text>().text = Math.Round(BossHappiness).ToString() + "%";
        HP.position = new Vector3(HP.position.x, -0.9f + BossHappiness * (1.8f / 100), -1);
    }

    //Saves all score to local json and sql db
    void SaveAll()
    {
        DefineValues();
        Score score = new Score();
        score.CC = CC;
        score.CPS = CPS;
        score.BossHappiness = BossHappiness;
        score.TimeQuit = DateTime.Now;

        ScoreDB scoredb = new ScoreDB();
        scoredb.AndroidID = AndroidID;
        scoredb.Score = CC;

        File.WriteAllText(spath, JsonConvert.SerializeObject(score));

        WebClient webclient = new WebClient();
        webclient.Headers.Add("Content-Type", "application/json");
        webclient.UploadString(apiHost + "api/score", "PUT", JsonConvert.SerializeObject(scoredb));
    }

    //loads score from local json and returns Score object
    Score LoadAll()
    {
        DefineValues();
        Score score = JsonConvert.DeserializeObject<Score>(File.ReadAllText(spath));
        CC = score.CC;
        CPS = score.CPS;
        if (!GoDown)
        {
            BossHappiness = score.BossHappiness;
        }
        return score;
    }
}