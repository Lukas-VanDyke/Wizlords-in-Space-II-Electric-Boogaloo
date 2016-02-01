using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject eyeOfNewt; //  item 1
    public GameObject featherOfBird; // item 2
    public GameObject herbOfEnlightenment; // item 3
    public GameObject platform;

    public float negHorizontalMin = -10f;
	public float negHorizontalMax = -5f;
	public float horizontalMin = 5f;
    public float horizontalMax = 10f;
    public float verticalMin = 7f;
    public float verticalMax = 13f;


    private Vector3 originPosition;
    private Vector3 negativePosition;

	public GameObject p1;
	public GameObject p2;

    public Animator torch_p1;
    public Animator torch_p2;

    void Start()
    {
        
        originPosition = new Vector3(27f, 0, 0);
        negativePosition = new Vector3(-27f, 0, 0);
        Spawn(200, 37.5f, 18);
        originPosition = new Vector3(13f, 0, 0);
        negativePosition = new Vector3(-13f, 0, 0);
        Spawn(200, 22, 2.5f);
    }

	public void cast(int player, int spell){
		//if inventory is legit cast, clear inventory

		if (player == 1)
		{
			p1.GetComponent<p1control>().castSpell(spell);
		}
		else
		{
			p2.GetComponent<p2control>().castSpell(spell);
		}

		//else do nothing
	}

	void Spawn(float maxHeight, float maxXVal, float minXVal)
    {
        float currentHeight = 0;
        float height;
        float width;
		float width2;
        while (currentHeight < maxHeight)
        {
            height = Random.Range(verticalMin, verticalMax);
            width = Random.Range(horizontalMin, horizontalMax);
			width2 = Random.Range (negHorizontalMin, negHorizontalMax);
			Vector3 randomPosition;
			Vector3 randNeg;
			int flip = Random.Range (0, 2);
			if (flip == 0) {
				randomPosition = originPosition + new Vector3 (width, height, 0);
				randNeg = negativePosition - new Vector3(width, (0 - height), 0);
			} else {
				randomPosition = originPosition + new Vector3 (width2, height, 0);
				randNeg = negativePosition - new Vector3(width2, (0 - height), 0);
			}
            if ((randomPosition.x > minXVal) && (randomPosition.x < maxXVal))
            {
				Instantiate(platform, randNeg, Quaternion.identity);
                Instantiate(platform, randomPosition, Quaternion.identity);
                originPosition = randomPosition;
				negativePosition = randNeg;
                currentHeight += height;
                
                // Pick up shit 
                int coinFlip = Random.Range(0, 4);
                if (coinFlip != 0)
                {
                    randomPosition = randomPosition + new Vector3(0,1.7f,0);
					Vector3 negativeItemPosition = negativePosition + new Vector3(0,1.7f,0);
                    int diceRoll = Random.Range(0, 6);
                    if (diceRoll % 3 == 0)
                    {
                        Instantiate(eyeOfNewt, randomPosition, Quaternion.identity);
                        Instantiate(eyeOfNewt, negativeItemPosition, Quaternion.identity);
                    }
                    if (diceRoll % 3 == 1)
                    {
                        Instantiate(featherOfBird, randomPosition, Quaternion.identity);
                        Instantiate(featherOfBird, negativeItemPosition, Quaternion.identity);
                    }
                    if (diceRoll % 3 == 2)
                    {
                        Instantiate(herbOfEnlightenment, randomPosition, Quaternion.identity);
                        Instantiate(herbOfEnlightenment, negativeItemPosition, Quaternion.identity);
                    }
                }

            }
        }
    }
}
