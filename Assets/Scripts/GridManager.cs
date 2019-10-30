using System.Collections;
using System.Collections.Generic;
using UnityEngine;

delegate void CurrentAction();

public class GridManager : MonoBehaviour
{
    public Transform machine;
    public Transform player;
    CurrentAction currentAction;
    int trashPack;
    float gridX;
    float gridZ;
    public GameObject cube;
    List<GameObject> cubes = new List<GameObject>();
    public static int savedCubes = 0;
    List<GameObject> selectedCubes = new List<GameObject>();
    bool seriesFinished;

    // Use this for initialization
    void Start()
    {
        gridX = 6;
        gridZ = 27;
        trashPack = 18;

        currentAction = CreateGrid;
    }

    // Update is called once per frame
    void Update()
    {
        currentAction();

        Debug.Log(currentAction.Method.Name);
    }

    void CreateGrid()
    {
        GameObject obj = null;
        GameObject lastObj;

        if (cubes.Count == 0)
        {
            obj = Instantiate(cube, new Vector3(0, 0, 0), Quaternion.identity);
            obj.SetActive(true);
            cubes.Add(obj);
        }

        lastObj = cubes[cubes.Count - 1];

        if (lastObj.transform.position.x < gridX - 1)
            obj = Instantiate(cube, new Vector3(lastObj.transform.position.x + 1, 0, lastObj.transform.position.z), Quaternion.identity);
        else if (lastObj.transform.position.z < gridZ - 1)
            obj = Instantiate(cube, new Vector3(0, 0, lastObj.transform.position.z + 1), Quaternion.identity);
        else
        {
            currentAction = SelectGarbage;
        }

        if (obj != null)
        {
            obj.SetActive(true);
            cubes.Add(obj);
        }
    }

    void RestoreGrid()
    {
        selectedCubes.Clear();

        if (savedCubes > 0)
        {
            GameObject obj = null;
            GameObject lastObj;

            lastObj = cubes[cubes.Count - 1];

            if (lastObj.transform.position.x < gridX - 1)
                obj = Instantiate(cube, new Vector3(lastObj.transform.position.x + 1, 0, lastObj.transform.position.z), Quaternion.identity);
            else if (lastObj.transform.position.z < gridZ - 1)
                obj = Instantiate(cube, new Vector3(0, 0, lastObj.transform.position.z + 1), Quaternion.identity);

            if (obj != null)
            {
                obj.SetActive(true);
                cubes.Add(obj);

                savedCubes -= 1;
            }
        }

        else
            currentAction = SelectGarbage;
    }

    void SelectGarbage()
    {
        for (int i = 0; i < trashPack; i++)
        {
            GameObject lastObj;

            lastObj = cubes[cubes.Count - 1 - i];
            selectedCubes.Add(lastObj);
        }

        currentAction = PositionGarbage;
    }

    void PositionGarbage()
    {
        bool nextStep = true;

        foreach (GameObject obj in selectedCubes)
        {
            if (obj.transform.position.y < 1)
            {
                obj.transform.Translate(0, 0.5f * Time.deltaTime, 0);
                nextStep = false;
            }
        }

        if (nextStep)
            currentAction = ThrowGarbage;

    }

    void ThrowGarbage()
    {
        foreach (GameObject obj in selectedCubes)
        {
            cubes.Remove(obj);
            obj.GetComponent<Trash>().ComeToLife = true;
        }

        currentAction = Wait;
    }

    void Wait()
    {
        bool nextStep = true;

        foreach (GameObject obj in selectedCubes)
        {
            if (obj != null)
            {
                if (obj.transform.position.z > -5)
                    nextStep = false;

                if (obj.transform.position.y < -2)
                    Destroy(obj);
            }
        }

        if (nextStep)
        {
            foreach (GameObject obj in selectedCubes)
                if (obj != null)
                    Destroy(obj);

            currentAction = RestoreGrid;
        }
    }
}
