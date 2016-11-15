using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
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

        if (!File.Exists(spath))
        {
            Score score = new Score();
            score.CC = 0;
            score.CPS = 0;

            output = JsonConvert.SerializeObject(score);
            File.WriteAllText(spath, output);

        }

        if (!File.Exists(ppath))
        {
            List<Product> product = new List<Product>();

            Product productA = new Product();
            productA.name = "Panodil";
            productA.price = 500;
            productA.owned = 0;
            productA.modifier = 1;

            Product productB = new Product();
            productB.name = "Adderall";
            productB.price = 1000;
            productB.owned = 0;
            productB.modifier = 2;


            product.Add(productA);
            product.Add(productB);


            output = JsonConvert.SerializeObject(product);
            File.WriteAllText(ppath, output);
        }

        Score cscore = JsonConvert.DeserializeObject<Score>(File.ReadAllText(spath));
        CC = cscore.CC;
        lastCC = CC;

        string pdata = File.ReadAllText(ppath);
        List<Product> DeProduct = JsonConvert.DeserializeObject<List<Product>>(pdata);
        
        foreach(Product p in DeProduct)
        {
            mod += p.modifier * p.owned;
        }


        CPStt = CPSt.gameObject.GetComponent<Text>();
        CTtt = CTt.gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
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

    void OnMouseDown()
    {
        GameObject Carma = new GameObject("Carma");
        Carma.gameObject.AddComponent<Carma>();
        SpriteRenderer CarmaSprite = Carma.AddComponent<SpriteRenderer>();
        CarmaSprite.sprite = YinYang;

        CC++;
        CPS += 1;
    }

    public void GotoBuy()
    {
        Score score = new Score();
        score.CC = CC;
        score.CPS = CPS;

        output = JsonConvert.SerializeObject(score);
        File.WriteAllText(spath, output);
        Application.LoadLevel("Buy");
    }
}
