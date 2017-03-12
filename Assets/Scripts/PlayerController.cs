using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;

    public GameObject basicBomb;

    public bool canBomb;
    private int bombCountLimit;
    public int bombCount;
    private List<GameObject> bombs;

	// Use this for initialization
	void Start () {
        bombs = new List<GameObject>();
        bombCountLimit = 1;
        // Set bomb size to match player scale
        basicBomb.transform.localScale = gameObject.transform.localScale;
	}

    // Once per frame
    void Update()
    {
        // Update bomb count
        bombCount = bombs.Count(e => e.activeSelf);
        print(bombCount);

        if (canBomb && bombCount < bombCountLimit && Input.GetKeyDown("space"))
        {
            basicBomb.transform.position = gameObject.transform.position;
            bombs.Add(Instantiate(basicBomb));
            bombCount++;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        var x = Input.GetAxis("Horizontal") * speed;
        var y = Input.GetAxis("Vertical") * speed;

        transform.Translate(x, 0, y);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Bomb"))
        {
            canBomb = false;
            bombCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Bomb"))
        {
            bombCount--;
            canBomb = true;
        }
    }
}
