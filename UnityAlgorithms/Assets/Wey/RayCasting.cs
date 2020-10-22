using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasting : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Drag and drop the main camera into this field")]
    private Camera camera;

    private RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) { 
            
        }
    }
}
