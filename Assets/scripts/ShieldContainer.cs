using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ShieldContainer : MonoBehaviour
{
    public int NumberOfShields;
    public GameObject shieldPrefab;

    List<MeshRenderer> shields;

    void Start ()
    {
        shields = allocateShields();

        float startY_viewport = Camera.main.WorldToViewportPoint(transform.position).y;

        var startPlace_viewport = new Vector2(0, startY_viewport);
        var endPlace_viewport = new Vector2(1, startY_viewport);

        Vector3 startPlace = Camera.main.ViewportToWorldPoint(startPlace_viewport);
        Vector3 endPlace = Camera.main.ViewportToWorldPoint(endPlace_viewport);

        PlaceShieldsInRange(shields, 0, shields.Count - 1, startPlace, endPlace);

    }

    private List<MeshRenderer> allocateShields()
    {
        shields = new List<MeshRenderer>();
        for (int i = 0; i < NumberOfShields; ++i)
        {
            var currShield = Instantiate(shieldPrefab).GetComponent<MeshRenderer>();
            currShield.transform.parent = transform;
            currShield.transform.localScale = new Vector3(1, 1, 1);
            shields.Add(currShield);
        }

        return shields;
    }

    public void PlaceShieldsInRange(List<MeshRenderer> shields, int startIndex, int endIndex, Vector3 start, Vector3 end)
    {
        int count = endIndex - startIndex + 1; // +1 because indices are 0 based
        var midindex = startIndex + count / 2;
        var midPos = (start + end) / 2;

        if (count == 1)
        {
            shields[midindex].transform.position = midPos;
            return;
        }

        if (count % 2 == 0) // 2, 4, 6...
        {
            float interval = (end - start).x / (count + 1);
            for (int i = 0; i < count; ++i)
            {
                shields[startIndex + i].transform.position = start + new Vector3(interval * (i + 1), 0);
            }
        }
        else // 1, 3, 5
        {
            PlaceShieldsInRange(shields, midindex, midindex, start, end);
            PlaceShieldsInRange(shields, startIndex, midindex - 1, start, midPos);
            PlaceShieldsInRange(shields, midindex + 1, endIndex, midPos, end);
        }
    }

    private List<MeshRenderer> findChildShields()
    {
        List<MeshRenderer> childShields = new List<MeshRenderer>();
        foreach (Transform child in transform)
        {
            if (child.tag == "Shield")
                childShields.Add(child.GetComponent<MeshRenderer>());
        }

        return childShields;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
