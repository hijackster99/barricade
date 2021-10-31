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
    int breaking;

    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        player = GameObject.Find("MainCharacter");

        breaking = 0;
    }

    // Update is called once per frame
    void Update()
    {
        door = false;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, doorRange);
        foreach (Collider2D c in cols)
        {
            if (c.tag == "Door" && !c.gameObject.GetComponent<Door>().open)
            {
                door = true;
            }
        }

        if (door)
        {
            agent.isStopped = true;
            breaking++;
            animator.SetBool("Break", true);
            animator.SetBool("Walk", false);

        }
        if (breaking == 1)
        {
            currentDoorAnimTime = doorAnimTime;
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

            }
            if(!door)
            {
                transform.rotation = Quaternion.LookRotation(transform.forward, agent.desiredVelocity * -1);
            }
            animator.SetBool("Walk", true);
        }
    }

    void FixedUpdate()
    {
        Debug.Log(door);
        if (currentDoorAnimTime > 0)
        {
            currentDoorAnimTime--;
        }
        else if(door)
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
        breaking = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") player.GetComponent<PlayerController>().damage();
    }
}
