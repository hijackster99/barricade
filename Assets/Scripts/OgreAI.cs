using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OgreAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;

    [SerializeField] float range;
    [SerializeField] float wanderRange;
    [SerializeField] float doorRange;
    [SerializeField] float doorAnimTime;
    [SerializeField] bool wander;

    GameObject player;
    [SerializeField] float currentDoorAnimTime;
    bool door;

    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        player = GameObject.Find("MainCharacter");
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, doorRange);
        Debug.Log(cols.Length);
        foreach (Collider2D c in cols)
        {
            if (c.tag == "Door")
            {
                door = true;
            }
        }

        if (door)
        {
            agent.isStopped = true;
            currentDoorAnimTime = doorAnimTime;
            animator.SetBool("Break", true);
        }
        else
        {
            agent.isStopped = false;
            if ((player.transform.position - transform.position).magnitude <= range)
            {
                agent.SetDestination(player.transform.position);
                wander = false;
            }
            else
            {
                if (!agent.hasPath || !wander)
                {
                    float angle = Random.Range(0, 360);
                    Vector3 dest = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * wanderRange;

                    agent.SetDestination(dest + transform.position);
                }
                wander = true;

                transform.rotation = Quaternion.LookRotation(transform.forward, agent.desiredVelocity * -1);
            }
        }
    }

    void FixedUpdate()
    {
        if (currentDoorAnimTime > 0)
        {
            currentDoorAnimTime--;
        }
        else
        {
            BreakDoor();
            animator.SetBool("Break", false);
        }
    }

    private void BreakDoor()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, doorRange);
        foreach (Collider2D c in cols)
        {
            if (c.tag == "Door")
            {
                c.gameObject.GetComponentInChildren<Door>().Toggle();
            }
        }
    }
}
