using UnityEngine;

public class ExplosionRayBehaviour : MonoBehaviour
{
    private float maxRange;

    // Use this for initialization
    void Start()
    {
        maxRange = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (maxRange > 0)
        {
            gameObject.transform.Translate(new Vector3(0, 0, 3));
            maxRange--;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Destroyable Rock"))
        {
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        else if(other.tag.Equals("Unbreakable"))
        {
            gameObject.SetActive(false);
        }
    }
}
