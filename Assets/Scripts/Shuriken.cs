using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private bool hasScored;
    //private ScoringSystem scoringSystem;

    void Start()
    {
        //scoringSystem = Object.FindFirstObjectByType<ScoringSystem>();
        hasScored = false;
    }

    void Update()
    {
        if (hasScored) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        if (hit.collider != null)
        {
            string tag = hit.collider.gameObject.tag;
            Debug.Log("Shuriken hit: " + tag);

            // If it's the black background ("Finish"), wait to see if it hits anything else
            if (tag == "Shuriken")
            {
                Destroy(gameObject, 1f);
            }

            // Otherwise, register the hit and score
            ScoringSystem.instance.RegisterShurikenHit(transform.position, tag);
            hasScored = true;
        }
    }
}
