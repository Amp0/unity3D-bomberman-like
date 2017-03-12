using UnityEngine;

public class BombBehaviour : MonoBehaviour {

    private bool isRed;
    private int blinkDuration;

	// Use this for initialization
	void Start () {
        isRed = true;
        blinkDuration = 5;
	}
	
	// Update is called once per frame
	void Update () {
        blinkDuration--;
        if(blinkDuration <= 0)
        {
            GetComponent<Renderer>().material.color = (isRed) ? Color.red : Color.white;
            isRed = !isRed;
            blinkDuration = 5;
        }
	}
}
