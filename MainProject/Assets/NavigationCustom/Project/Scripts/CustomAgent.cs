using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CustomAgent : MonoBehaviour
{
    /// <summary>
    /// Spawn single prefab with multiple properties 
    /// </summary>
    private NavMeshAgent PlayerAgent;

    /// <summary>
    /// List of transforms for character to move
    /// </summary>
    public Transform[] GoalPoints;

    /// <summary>
    /// Temporary List for random movement
    /// </summary>
    List<int> NumberPool;

    /// <summary>
    /// Variable to store Previous Index value to stop repeating random number
    /// </summary>
    private int PrevIndex = 0;

    /// <summary>
    /// Variable to store Current random number 
    /// </summary>
    private int CurrRandomIndex = 0;

    /// <summary>
    /// Animator attached to this character
    /// </summary>  
    private Animator anim;

    /// <summary>
    /// bool value if it matches the given burgler name in Spawn enemies
    /// </summary>
    public bool isWalkable = false;

    /// <summary>
    /// Bool value if character reached to it's next destination point 
    /// </summary>
    private bool isReached = false;

    /// <summary>
    /// current index value
    /// </summary>
    public int indexvalue = 0;

    /// <summary>
    /// bool variable to check if character is dead
    /// </summary>
    private bool isDead = false;

    private void Awake()
    {
        PlayerAgent = this.GetComponent<NavMeshAgent>();
        PlayerAgent.updateRotation = false; // Making this false as to rotate it manually
        anim = this.GetComponent<Animator>();
    }

    void Start()
    {
        Check();
    }

    public void Check()
    {
        //RestartRandom();

        if (isWalkable)
        {
            //UniqueRandom();
            AnimateCharacter();
        }
    }

    private void Update()
    {
        if (!isWalkable)
            return;

        if (!PlayerAgent.pathPending && PlayerAgent.remainingDistance < 0.1f && !isReached)
        {
            isReached = true;
            StartCoroutine(GotoNextPoint());
            if (indexvalue >= GoalPoints.Length)
                PlayerAgent.isStopped = true;
        }
    }

    IEnumerator GotoNextPoint()
    {
        PlayerAgent.updateRotation = false;
        PlayerAgent.isStopped = true;
        anim.Play("Stand");
        yield return new WaitForSeconds(0.1f);

        PlayerAgent.isStopped = false;
        anim.Play("Walk");
        //UniqueRandom();
        AnimateCharacter();
        isReached = false;
    }

    private void RotateTowards(Transform target) // Method to turn the navmesh agent manually in place
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 90);
        PlayerAgent.updateRotation = true;
    }

    //Random number generation without repeating in loop 
    public void RestartRandom()
    {
        NumberPool = new List<int>();
        for (int i = 0; i < GoalPoints.Length; i++)
        {
            NumberPool.Add(i);
        }
    }

    //public void UniqueRandom()
    //{
    //    if (NumberPool.Count == 0)
    //        RestartRandom();

    //    int index = Random.Range(0, NumberPool.Count);
    //    anim.Play("Walk");
    //    if (PrevIndex == index) //If Previous random number is same as present
    //    {
    //        UniqueRandom();
    //    }
    //    else
    //    {
    //        CurrRandomIndex = NumberPool[index];
    //        NumberPool.RemoveAt(index);
    //        PrevIndex = index;

    //        PlayerAgent.destination = GoalPoints[CurrRandomIndex].position;
    //        RotateTowards(GoalPoints[CurrRandomIndex]);
    //    }
    //}

    public void AnimateCharacter()
    {
        isDead = false;
        indexvalue++;
        if (indexvalue < GoalPoints.Length && !isDead)
            StartCoroutine(ManageAnimations());
    }

    IEnumerator ManageAnimations()
    {
        PlayerAgent.speed = 1f;
        PlayerAgent.destination = GoalPoints[indexvalue].position;
        RotateTowards(GoalPoints[indexvalue]);
        anim.Play("Walk");

        yield return new WaitForSeconds(1f);
        //PlayerAgent.isStopped = true;
        PlayerAgent.speed = 0f;
        //anim.Play("LookBack");

        yield return new WaitForSeconds(2f);
        //PlayerAgent.isStopped = false;
        PlayerAgent.speed = 3.5f;
    }

    public void DieEffect()
    {
        isDead = true;
        anim.Play("Dying");
        PlayerAgent.isStopped = true;
    }
}