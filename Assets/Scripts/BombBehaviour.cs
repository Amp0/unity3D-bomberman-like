using UnityEngine;

public class BombBehaviour : MonoBehaviour {

    private bool isRed;
    private int blinkCountdown;

    public float bombCountdownInSeconds;

    public GameObject explosiveRay;

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

        // Explosion
        bombCountdownInSeconds -= 1F / 60F; // Remove 1 60th of a second
        if (bombCountdownInSeconds <= 0)
        {
            gameObject.SetActive(false);
            Vector3 bombPos = gameObject.transform.position;
            bombPos.y = 0F;
            explosiveRay.transform.position = bombPos;
            for(int i=0; i<4; i++)
            {
                explosiveRay.transform.Rotate(new Vector3(0, 90, 0));
                Instantiate(explosiveRay);
            }
        }

	}

    private void OnTriggerExit(Collider other)
    {
        GetComponent<SphereCollider>().isTrigger = false;
    }
}
