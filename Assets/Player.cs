using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Player : MonoBehaviour {
    int i = 0;
    public Sprite YinYang;
    public GameObject CPSt;
    public GameObject CTt;
    float CPS;
    long CT;
    Text CPStt;
    Text CTtt;

    // Use this for initialization
    void Start () {
        CPStt = CPSt.gameObject.GetComponent<Text>();
        CTtt = CTt.gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        CT += Convert.ToInt64(CPS);
        CPStt.text = Math.Ceiling(CPS).ToString();
        CTtt.text = CT.ToString();
        if (CPS > 0.05f)
        {
            CPS -= 0.05f;
        }
	}

    void OnMouseDown()
    {
        GameObject Carma = new GameObject("Carma" + Convert.ToString(i));
        Carma.gameObject.AddComponent<Carma>();
        SpriteRenderer CarmaSprite = Carma.AddComponent<SpriteRenderer>();
        CarmaSprite.sprite = YinYang;

        i++;
        CPS += 1;
    }

    public void GotoBuy()
    {
        Application.LoadLevel("Buy");
    }
}
