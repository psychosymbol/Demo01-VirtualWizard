using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class naming : MonoBehaviour
{
    public string part_of_the_oldName = "";
    public string part_of_the_newName = "";

    void Start()
    {
        part_of_the_oldName = "character_Ctrl_";
        part_of_the_newName = "character_Ctrl:";

        if (transform.name.Contains(part_of_the_oldName))
            transform.name = transform.name.Replace(part_of_the_oldName, part_of_the_newName);

        changeName(transform);

        DestroyImmediate(this);
    }
    public void changeName(Transform transfroms)
    {
        for (int i = 0; i < transfroms.childCount; i++)
        {
            if (transfroms.GetChild(i).name.Contains(part_of_the_oldName))
                transfroms.GetChild(i).name = transfroms.GetChild(i).name.Replace(part_of_the_oldName, part_of_the_newName);

            changeName(transfroms.GetChild(i));
        }
    }

}
