using System.Collections.Generic;
using UnityEngine;

public class ExplosionRayBehaviour : MonoBehaviour
{
    private float maxRange;

    /// <summary>
    /// List of blocking tags
    /// </summary>
    private List<string> blockers;

    public int squareSize;
    public Color explosionColor;

    // Use this for initialization
    void Start()
    {
        maxRange = 2;

        Vector3 scale = gameObject.transform.localScale;
        scale.x = squareSize * 0.7F;
        gameObject.transform.localScale = scale;
        GetComponent<Renderer>().material.color = explosionColor;

        blockers = new List<string>();
        blockers.Add("Wall");
        blockers.Add("Destroyable Rock");
        blockers.Add("Unbreakable");
    }

    // Update is called once per frame
    void Update()
    {
        if (maxRange > 0)
        {
            gameObject.transform.Translate(new Vector3(0, 0, -gameObject.transform.position.x/10));
            var scale = gameObject.transform.localScale;
            scale.z += gameObject.transform.localScale.x;
            gameObject.transform.localScale = scale;
            maxRange--;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (blockers.Contains(other.tag))
            gameObject.SetActive(false);
    }
}
