using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seppuku : MonoBehaviour
{
    public int type;

    public void CommitSeppuku()
    {
        Debug.Log("Destroy this: "+this.name+"Seppuku style!");
        Destroy(gameObject);
    }
}
