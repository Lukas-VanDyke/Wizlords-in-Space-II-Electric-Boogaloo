using UnityEngine;
using System.Collections;

public class win : MonoBehaviour {
    public GameObject player1;
    public GameObject player2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            p1control p1 = player1.GetComponent<p1control>();
            p2control p2 = player2.GetComponent<p2control>();
            if (other.gameObject.name == "player1")
            {
                this.GetComponent<Animator>().SetTrigger("p1");
                p1.castSpell(3);
                p2.lost = true;
                p1.won = true;
            }
            else {
                this.GetComponent<Animator>().SetTrigger("p2");
                p2.castSpell(3);
                p1.lost = true;
                p2.won = true;
            }
        }

    }
}
