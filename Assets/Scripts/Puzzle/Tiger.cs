using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger : MonoBehaviour
{
    private Vector2 tigerPos;
    public GameObject tiger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tiger.transform.Translate(Vector3.right * 0.09f);
    }
}
