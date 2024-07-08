using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    // list of gameobject tags to give to this object
    [SerializeField] List<string> tags;

    // Start is called before the first frame update
    void Start()
    {
        // loop through tags
        foreach (var tag in tags)
        {
            // find all gameobjects with the tag
            GameObject[] environmentObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in environmentObjects)
            {
                // set each object in list to be the child of this game object
                obj.transform.SetParent(this.transform);
                Debug.Log(environmentObjects.Length);
            }
        }
        Debug.Log(tags.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
