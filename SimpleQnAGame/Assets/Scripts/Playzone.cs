using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playzone : MonoBehaviour
{
    public ContactFilter2D filter;
    private Collider2D playzoneCollider;
    private Collider2D[] hits = new Collider2D[10];

    private void Awake()
    {
        playzoneCollider = GetComponent<Collider2D>();
    }

    public bool PlayerInsideZone()
    {
        //collision work
        playzoneCollider.OverlapCollider(filter,
                                    hits);

        int countHits = 0;
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i])
            {
                continue;
            }

            if (hits[i].tag == "Player")
            {
                countHits++;
            }


            //CLean up the array

            hits[i] = null;
        }

        return countHits>0;
    }
}
