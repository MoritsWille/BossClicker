using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class Player : MonoBehaviour
{
    string AndroiID;
    int mod = 0;
    long CC = 0;
    public Sprite YinYang;
    public GameObject CPSt;
    public GameObject CTt;
    float CPS;
    long CT;
    Text CPStt;
    Text CTtt;
    string output;
    static string ppath;
    static string spath;
    float time;
    long lastCC = 0;


    // Use this for initialization
    void Start()
    {
        //AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
        //AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
        //AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
        //string android_id = secure.CallStatic<string>("getString", contentResolver, "android_id");

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

        if (!File.Exists(spath))
        {
            Score score = new Score();
            score.CC = 0;
            score.CPS = 0;
            score.TimeQuit = DateTime.Now;

            output = JsonConvert.SerializeObject(score);
            File.WriteAllText(spath, output);

        }

        if (!File.Exists(ppath))
        {
            List<Product> product = new List<Product>();

            Product productA = new Product();
            productA.name = "Panodil";
            productA.price = 250;
            productA.owned = 0;
            productA.modifier = 1;
            productA.ID = 0;

            Product productB = new Product();
            productB.name = "ADHD Medicin";
            productB.price = 2500;
            productB.owned = 0;
            productB.modifier = 2;
            productB.ID = 1;

            Product productC = new Product();
            productC.name = "Lykke Piller";
            productC.price = 10000;
            productC.owned = 0;
            productC.modifier = 4;
            productC.ID = 2;

            Product productD = new Product();
            productD.name = "Duft Lys";
            productD.price = 20000;
            productD.owned = 0;
            productD.modifier = 8;
            productD.ID = 3;

            Product productE = new Product();
            productE.name = "Kage";
            productE.price = 100000;
            productE.owned = 0;
            productE.modifier = 12;
            productE.ID = 4;


            product.Add(productA);
            product.Add(productB);
            product.Add(productC);
            product.Add(productD);
            product.Add(productE);


            output = JsonConvert.SerializeObject(product);
            File.WriteAllText(ppath, output);
        }

        string pdata = File.ReadAllText(ppath);
        List<Product> DeProduct = JsonConvert.DeserializeObject<List<Product>>(pdata);

        foreach (Product p in DeProduct)
        {
            mod += p.modifier * p.owned;
        }

        Score cscore = JsonConvert.DeserializeObject<Score>(File.ReadAllText(spath));
        TimeSpan TimeSpent = DateTime.Now - cscore.TimeQuit;
        CC = cscore.CC + Convert.ToInt64(Math.Floor(TimeSpent.TotalSeconds) * mod);
        lastCC = CC;


        CPStt = CPSt.gameObject.GetComponent<Text>();
        CTtt = CTt.gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        CPStt.text = Math.Floor(CPS).ToString();
        CTtt.text = CC.ToString();
        if (time > 1)
        {
            CPS = CC - lastCC;
            lastCC = CC;
            time = 0;
            CC += mod;
        }
    }

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
        CPS += 1;
    }

    public void GotoBuy()
    {
        Score score = new Score();
        score.CC = CC;
        score.CPS = CPS;
        score.TimeQuit = DateTime.Now;

        output = JsonConvert.SerializeObject(score);
        File.WriteAllText(spath, output);
        Application.LoadLevel("Buy");
    }

    void OnApplicationQuit()
    {
        string data = File.ReadAllText(spath);
        Score score = JsonConvert.DeserializeObject<Score>(data);
        score.TimeQuit = DateTime.Now;
        score.CC = CC;
        score.CPS = CPS;
        data = JsonConvert.SerializeObject(score);
        File.WriteAllText(spath, data);
    }
}
