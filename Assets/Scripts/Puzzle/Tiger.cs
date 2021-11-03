using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger : MonoBehaviour
{
    public GameObject tiger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tiger.transform.Translate(Vector3.right * 0.2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("mudang"))
        {
            Debug.Log("무당살려~~");
            tiger.SetActive(false);
        }
    }
}
