using UnityEngine;

public class BombBehaviour : MonoBehaviour {

    private bool isWhite;
    private int blinkCountdown;

    public float bombCountdownInSeconds;

    public GameObject explosiveRay;
    public Color playerColor;

	// Use this for initialization
	void Start () {
        isWhite = false;
        blinkCountdown = 5;
	}
	
	// Update is called once per frame
	void Update () {
        // Blink management
        blinkCountdown--;
        if(blinkCountdown <= 0)
        {
            GetComponent<Renderer>().material.color = (!isWhite) ? playerColor : Color.white;
            isWhite = !isWhite;
            blinkCountdown = 5;
        }

        // Explosion
        bombCountdownInSeconds -= 1F / 60F; // Remove 1 60th of a second
        if (bombCountdownInSeconds <= 0)
        {
            Explode();
        }

	}

    private void OnTriggerExit(Collider other)
    {
        GetComponent<SphereCollider>().isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Damage"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        gameObject.SetActive(false);
        Vector3 bombPos = gameObject.transform.position;
        bombPos.y = 0F;
        explosiveRay.transform.position = bombPos;
        for (int i = 0; i < 4; i++)
        {
            explosiveRay.transform.Rotate(new Vector3(0, 90, 0));
            var explo = Instantiate(explosiveRay);
            explo.GetComponent<ExplosionRayBehaviour>().explosionColor = playerColor;

        }
    }
}
