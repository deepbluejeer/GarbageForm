using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChecker : MonoBehaviour
{
    Transform viewingItem;

    public Transform CheckItem()
    {
        Transform previousItem;

        if (viewingItem == null)
            return null;

        if (viewingItem.tag == "Trash")
        {
            previousItem = viewingItem;
            viewingItem = null;

            return previousItem;
        }

        //if (viewingItem.tag == "Player")
        //{
        //    previousItem = viewingItem;
        //    viewingItem = null;

        //    return previousItem;
        //}

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trash")
        viewingItem = other.transform;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Trash")
            viewingItem = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        viewingItem = null;
    }
}
