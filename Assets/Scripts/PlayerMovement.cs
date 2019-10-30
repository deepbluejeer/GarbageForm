using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour
{
    public Text text;
    public float Speed = 0f;
    private float horizontal = 0f;
    private float vertical = 0f;
    bool holdingItem;
    Transform currentItem;
    public Transform objHolder;
    public Transform objChk;
    ObjectChecker objectChecker;

    bool dead;
    Rigidbody rgb;
    Vector3 Movement;

    void Start()
    {
        rgb = GetComponent<Rigidbody>();
        objectChecker = objChk.GetComponent<ObjectChecker>();
        text.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckIfDead();

        if (!dead)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            Movement = new Vector3(horizontal, 0f, vertical);

            if (Input.GetButtonDown("Action"))
            {
                if (!holdingItem)
                    GrabItem(objectChecker.CheckItem());
                else
                    ThrowItem();
            }

            MovementAnimation();
            HoldingAnimation();
        }
    }

    private void CheckIfDead()
    {
        if (transform.position.y < -1)
        {
            text.gameObject.SetActive(true);
            dead = true;
            rgb.isKinematic = true;
            DeadAnimation();
        }

        if (dead)
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void FixedUpdate()
    {
        Walk();
    }

    public void Walk()
    {
        if (vertical != 0 || horizontal != 0)
            transform.rotation = Quaternion.LookRotation(Movement);

        rgb.AddForce(Movement.normalized * Speed, ForceMode.VelocityChange);
    }

    void GrabItem(Transform item)
    {
        if (item == null) return;
        else currentItem = item;

        currentItem.GetComponent<Rigidbody>().isKinematic = true;
        currentItem.GetComponent<Collider>().enabled = false;
        currentItem.GetComponent<Trash>().IsPicked = true;

        currentItem.rotation = transform.rotation;

        currentItem.position = objHolder.position;
        currentItem.parent = objHolder;

        holdingItem = true;
    }

    void ThrowItem()
    {
        if (currentItem == null) return;

        currentItem.parent = null;
        currentItem.GetComponent<Rigidbody>().isKinematic = false;
        currentItem.GetComponent<Collider>().enabled = true;
        currentItem.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 6, 6), ForceMode.VelocityChange);
        currentItem.GetComponent<Trash>().IsPicked = false;

        currentItem = null;
        holdingItem = false;
    }

    public void MovementAnimation()
    {
        if (horizontal != 0 || vertical != 0)
            GetComponent<Animator>().SetFloat("Moving", 1);
        else if (horizontal == 0 && vertical == 0)
            GetComponent<Animator>().SetFloat("Moving", 0);
    }

    public void HoldingAnimation()
    {
        GetComponent<Animator>().SetBool("Holding", holdingItem);
    }

    public void DeadAnimation()
    {
        GetComponent<Animator>().SetBool("Die", true);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Screenmanager Resolution Width", 800);
        PlayerPrefs.SetInt("Screenmanager Resolution Height", 600);
        PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
    }
}