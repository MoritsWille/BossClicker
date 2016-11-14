using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;

public class MenuMaster : MonoBehaviour {
    string path;
    public GameObject OriginalButton;
    public GameObject OriginalLtext;
    public Transform Canvas;
    int i = 0;

	// Use this for initialization
	void Start () {
        path = Application.dataPath + @"/Purchases.json";
        string data = File.ReadAllText(path);
        List<Product> DeProduct = JsonConvert.DeserializeObject<List<Product>>(data);

        foreach (Product p in DeProduct)
        {
            GameObject Button = Instantiate(OriginalButton);
            Button.transform.parent = Canvas;
            Button.GetComponent<RectTransform>().localPosition = new Vector3(-75.2f, 240 - 60 * i);
            Text text = Button.transform.Find("Text").gameObject.GetComponent<Text>();
            text.text = p.name;

            GameObject Ltext = Instantiate(OriginalLtext);
            Ltext.transform.parent = Canvas;
            Ltext.GetComponent<RectTransform>().localPosition = new Vector3(113, 240 - 60 * i);
            Text Lltext = Ltext.gameObject.GetComponent<Text>();
            Lltext.text = "Level " + Convert.ToString(p.owned + 1);

            i++;
        }

        //List<Product> product = new List<Product>();

        //Product productA = new Product();
        //productA.name = "Panodil";
        //productA.price = 500;
        //productA.owned = 0;

        //Product productB = new Product();
        //productB.name = "Aderall";
        //productB.price = 1000;
        //productB.owned = 0;

        //product.Add(productA);
        //product.Add(productB);


        //string output = JsonConvert.SerializeObject(product);
        //File.WriteAllText(path,output);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
