using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Box : MonoBehaviour
{
    Rigidbody2D rb;
    const float force = 6f;
    public LayerMask layer;
    int grounded;
    RaycastHit2D[] hit = new RaycastHit2D[1];

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        grounded = Physics2D.RaycastNonAlloc(this.transform.position, Vector2.down, hit, 0.3f, layer);
        
        Debug.DrawRay(this.transform.position, Vector2.down * 0.3f, Color.red);
        if (Input.GetKeyDown(KeyCode.Space) && grounded == 1)
        {
            if(layer == LayerMask.GetMask("AnoAngSahig"))
                Jump();
        }
    }
    private void Jump()
    {
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);

    }
}
