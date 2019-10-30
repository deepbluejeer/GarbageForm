using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public Transform trashHolder;
    Transform currentTrash;
    ObjectChecker objectChecker;
    bool recycleTrash;

    // Use this for initialization
    void Start()
    {
        objectChecker = trashHolder.GetComponent<ObjectChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!recycleTrash)
            DealWithTrash(objectChecker.CheckItem());
        else
        {
            AvoidTrash(objectChecker.CheckItem());
            TrashAnimation();
        }

        MachineAnimation();
    }

    void DealWithTrash(Transform gotTrash)
    {
        if (gotTrash == null) return;

        //Position trash
        recycleTrash = true;
        currentTrash = gotTrash;
        currentTrash.GetComponent<Trash>().IsPicked = true;
        currentTrash.position = trashHolder.position;
        currentTrash.parent = trashHolder;
        currentTrash.GetComponent<Rigidbody>().isKinematic = true;
        currentTrash.GetComponent<Collider>().enabled = false;

        StartCoroutine(Recycle(1.0f));
    }

    void AvoidTrash(Transform gotTrash)
    {
        if (gotTrash == null) return;

        gotTrash.GetComponent<Rigidbody>().AddForce(new Vector3(0, 5, -15), ForceMode.Impulse);
    }

    void TrashAnimation()
    {
        currentTrash.transform.Translate(new Vector3(0, -0.5f * Time.deltaTime, 0), Space.World);

        if (currentTrash.transform.localScale.x > Vector3.zero.x)
            currentTrash.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f) * Time.deltaTime;
        else
            currentTrash.transform.localScale = Vector3.zero;
    }

    private void MachineAnimation()
    {
        GetComponent<Animator>().SetBool("Working", recycleTrash);
    }

    private IEnumerator Recycle(float timeNeeded)
    {
        yield return new WaitForSeconds(timeNeeded);

        currentTrash.parent = null;
        Destroy(currentTrash.gameObject);
        recycleTrash = false;

        GridManager.savedCubes += 1;
    }
}
