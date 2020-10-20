using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateColliders : MonoBehaviour
{
    List<GameObject> colliders;

    // Create Colliders for the chessboard
    void Start()
    {
        GameObject folder = new GameObject("ColliderFolder");
        colliders = new List<GameObject>();
        uint size = GetComponent<GlowTiles>().size;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject obj = new GameObject("Collider " + colliders.Count);
                obj.transform.localScale = new Vector3(2, 1, 2);
                obj.transform.position = new Vector3(i * 2, -0.48f, j * 2);
                obj.AddComponent<BoxCollider>();
                colliders.Add(obj);
                obj.transform.parent = folder.transform;
            }
        }
    }
}
