using UnityEngine;
using System.Collections;

public class collectionScript : MonoBehaviour {


	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) {
			if (other.gameObject.name == "player1") {
				other.gameObject.GetComponent<p1control> ().pickup (this.gameObject);
			} else {
				other.gameObject.GetComponent<p2control>().pickup(this.gameObject);
			}
			this.gameObject.SetActive(false);
		}
	}
}
