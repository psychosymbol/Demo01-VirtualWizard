using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class naming : MonoBehaviour
{
    public string part_of_the_oldName = "";
    public string part_of_the_newName = "";

    public string addName = "";


    bool add = false;
    void Start()
    {
        add = true;
        if (!add)
        {
            part_of_the_oldName = "character_Ctrl_";
            part_of_the_newName = "character_Ctrl:";

            if (transform.name.Contains(part_of_the_oldName))
                transform.name = transform.name.Replace(part_of_the_oldName, part_of_the_newName);

            changeName(transform);
        }
        else
        {
            addName = "Android_Rig:";
            transform.name = addName + transform.name;

            AddName(transform);
        }
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

    public void AddName(Transform transfroms)
    {
        for (int i = 0; i < transfroms.childCount; i++)
        {
            transfroms.GetChild(i).name = addName + transfroms.GetChild(i).name;

            AddName(transfroms.GetChild(i));
        }
    }

}
