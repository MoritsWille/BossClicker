using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class ButtonClick : MonoBehaviour {
    string ppath;
    string data;
    GameObject Master;

    void Start()
    {
        Master = GameObject.Find("MenuMaster");
        if (Application.platform == RuntimePlatform.Android)
        {
            ppath = Application.persistentDataPath + @"/Purchases.json";
        }
        else
        {
            ppath = Application.dataPath + @"/Purchases.json";
        }
        data = File.ReadAllText(ppath);
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(delegate() { Select(); } );
    }

    public void Select()
    {
        int i = 0;
        data = File.ReadAllText(ppath);
        List<Product> product = JsonConvert.DeserializeObject<List<Product>>(data);
        foreach(Product p in product)
        {
            if(gameObject.name == i.ToString())
            {
                if (Master.GetComponent<MenuMaster>().Funds(Convert.ToInt64(Math.Ceiling(Math.Pow((p.owned + 1) * p.browniePrice, 1.01))), p.happinessPrice))
                {
                    Master.GetComponent<MenuMaster>().SubtractPoints(Convert.ToInt64(Math.Ceiling(Math.Pow((p.owned + 1) * p.browniePrice, 1.01))), p.happinessPrice);
                    p.owned++;
                    break;
                }
            }
            i++;
        }

        List<Product> products = new List<Product>();
        foreach(Product p in product)
        {
            products.Add(p);
        }
        File.WriteAllText(ppath, JsonConvert.SerializeObject(products));
        Master.GetComponent<MenuMaster>().LoadProducts();
    }
}