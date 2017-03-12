using UnityEngine;

public class BombBehaviour : MonoBehaviour {

    private bool isRed;
    private int blinkCountdown;
    public float bombCountdownInSeconds;

	// Use this for initialization
	void Start () {

        isRed = true;
        blinkCountdown = 5;

	}
	
	// Update is called once per frame
	void Update () {
        // Blink management
        blinkCountdown--;
        if(blinkCountdown <= 0)
        {
            GetComponent<Renderer>().material.color = (isRed) ? Color.red : Color.white;
            isRed = !isRed;
            blinkCountdown = 5;
        }

        // Exposion
        bombCountdownInSeconds -= 1F / 60F; // Remove 1 60th of a second
        if (bombCountdownInSeconds <= 0)
        {
            
            Destroy(gameObject);
        }

	}
}
