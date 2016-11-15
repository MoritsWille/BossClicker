using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class ButtonClick : MonoBehaviour {
    string ppath;
    int[] level;
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
        data = File.ReadAllText(ppath);
        List<Product> product = JsonConvert.DeserializeObject<List<Product>>(data);
        switch (gameObject.name)
        {
            case "Panodil":
                if (Master.GetComponent<MenuMaster>().Funds(Convert.ToInt64((Math.Ceiling(Math.Pow(product[0].owned * product[0].price + 1, 2))))))
                {
                    product[0].owned += 1;
                    Master.GetComponent<MenuMaster>().SubtractPoints((product[0].owned + 1) * product[0].price);
                }
                break;

            case "Adderall":
                if (Master.GetComponent<MenuMaster>().Funds(Convert.ToInt64((Math.Ceiling(Math.Pow(product[1].owned * product[1].price + 1, 2))))))
                {
                    product[1].owned += 1;
                    Master.GetComponent<MenuMaster>().SubtractPoints((product[1].owned + 1) * product[1].price);
                }
                break;
        }
        List<Product> products = new List<Product>();
        products.Add(product[0]);
        products.Add(product[1]);
        File.WriteAllText(ppath, JsonConvert.SerializeObject(products));
        Master.GetComponent<MenuMaster>().LoadProducts();
    }
}