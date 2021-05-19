using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DeepAction;
public class enumTest : MonoBehaviour
{

    public D_Attribute att;

    

    [Button]
    public void test()
    {
        Debug.Log(att);
        Debug.Log((int)att);
    }


}
