using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISingleton : MonoBehaviour
{
//variables
    public static AISingleton instance {get; private set;}

//methods

   private void Awake(){

        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
        
    }

}
