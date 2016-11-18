using UnityEngine;
using System.Collections;

public class Carma : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(Random.Range(1, 7.3f), -6, 2);
        transform.localScale = new Vector3(4,4,1);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, 2);
        if (transform.position.y > 6.5)
        {
            Destroy(gameObject);
        }
	}
}
