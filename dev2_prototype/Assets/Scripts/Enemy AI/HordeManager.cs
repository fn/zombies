using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : MonoBehaviour
{
    public static HordeManager instance {get; private set;}
    [SerializeField] Transform movePositionMain;

    GameObject player;

    
    void Awake(){
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
        player = GameObject.FindWithTag("Player");
        movePositionMain = player.transform;
    }

    public GameObject Player(){
        return player;
    }
}
