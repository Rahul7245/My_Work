using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class CustomAgent : MonoBehaviour
{
    public NavMeshAgent PlayerAgent;
    private int CurrDest = 0;
    private Animator anim;
    private bool isReached = false;
    List<int> NumberPool;
    public Transform GoalParent;
    public String EnName;
    public bool isWalkable = false;
    public Transform[] TargetPoint;
    public int[] PathPoints;

    private void Awake()
    {
        PlayerAgent.isStopped = true;
        GoalParent = GameObject.FindGameObjectWithTag("Respawn").transform;
        PlayerAgent.GetComponent<NavMeshAgent>();
        anim = PlayerAgent.GetComponent<Animator>();
        for (int i = 0; i < TargetPoint.Length; i++)
        {
            int index = PathPoints[i];
            TargetPoint[i] = GoalParent.GetChild(index);
        }
    }

    void Start()
    {
        Check();
        if (isWalkable)
        {
            PlayerAgent.isStopped = false;
            anim.Play("Walk");
            RestartRandom();
            UniqueRandom();

            PlayerAgent.destination = TargetPoint[CurrDest].position;
        }
    }

    public void Check()
    {
        isWalkable = false;
        int len=GameObject.Find("GameController").GetComponent<SpawnEnemy>().WalkableEn.Length;
        for (int i = 0; i < len; i++)
        {
            if (GameObject.Find("GameController").GetComponent<SpawnEnemy>().WalkableEn[i] == EnName)
            {
                isWalkable = true;
            }
        }    
    }

    private void Update()
    {
        if (!isWalkable)
            return;

        if (!PlayerAgent.pathPending && PlayerAgent.remainingDistance < 0.5f && !isReached)
        {
            isReached = true;
            GotoNextPoint();
        }
    }

    void GotoNextPoint()
    {
        if (TargetPoint.Length == 0)
            return;

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        PlayerAgent.isStopped = true;
        anim.Play("Stand");
        yield return new WaitForSeconds(4f);

        PlayerAgent.isStopped = false;
        anim.Play("Walk");
        UniqueRandom();

        PlayerAgent.destination = TargetPoint[CurrDest].position;
        isReached = false;
    }

    //Random number generation without repeating in loop 
    public void RestartRandom()
    {
        NumberPool = new List<int>();
        for (int i = 0; i < TargetPoint.Length; i++)
        {
            NumberPool.Add(i);
        }
    }

    public void UniqueRandom()
    {
        int index = Random.Range(0, NumberPool.Count);
        CurrDest = NumberPool[index];
        NumberPool.RemoveAt(index);
        //Debug.Log(CurrDest);
        if (NumberPool.Count == 0)
            RestartRandom();
    }
}