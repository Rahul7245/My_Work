using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SpawnEnemy : MonoBehaviour
{
    public static SpawnEnemy Instance;
    public GameObject[] Enemy;
    public Transform[] EneSpawnPos = new Transform[5];
    public String[] WalkableEn;
    public GameObject Envi;
    public Transform EnParent;

    void Start()
    {
        for (int i = 0; i < Enemy.Length; i++)
        {
            Instantiate(Enemy[i], EneSpawnPos[i].position, Quaternion.identity, EnParent);
            
        }
        EventManager.AddReloadWeapontListener(Reset);
        EventManager.AddShootListener(Hide);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Pressed 1");
            Reset();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Pressed 2");
            Hide(1);
        }
    }

    public void Hide(int integer)
    {
        EnParent.gameObject.SetActive(false);
        Envi.SetActive(false);
    }

    public void Reset()
    {
        EnParent.gameObject.SetActive(true);
        GameObject[] EnObjectArray = GameObject.FindGameObjectsWithTag("Burgler");

        for (int i = 0; i < EnObjectArray.Length; i++)
        {
            EnObjectArray[i].transform.position = EneSpawnPos[i].transform.position;
        }
        Envi.SetActive(true);
    }
}