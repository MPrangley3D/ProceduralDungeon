using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRaycast : MonoBehaviour
{
    public Vector2 dir = Vector2.down;
    public float multi = 4.5f;

    // Update is called once per frame
    void Update()
    {
        TestRay();
    }

    void TestRay()
    {
        //Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y) + checkPreviousDir * 10, checkPreviousDir, rayDistance, whatIsGround);

        Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y) + dir * multi, dir*1f, Color.red);
        bool ray = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y) + dir * multi, dir, 1f);
        Debug.Log("Hit? " + ray);
    }
}
