using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Collider col;

    [SerializeField] float range;
    [SerializeField] float lockRange;
    [SerializeField] float wanderRange;
    [SerializeField] float projSpeed;

    [SerializeField] bool wander;

    [SerializeField] GameObject bone;

    [SerializeField] float cooldown;

    float boneCooldown;
    bool inRange;
    GameObject player;

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
        agent.isStopped = false;
        if ((player.transform.position - transform.position).magnitude <= lockRange)
        {
            if ((player.transform.position - transform.position).magnitude >= range)
            {
                agent.SetDestination(player.transform.position);
                inRange = false;
            }
            else
            {
                agent.isStopped = true;
                inRange = true;
            }
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
            inRange = false;
        }

        if (inRange)
        {
            transform.rotation = Quaternion.LookRotation(transform.forward, transform.position - player.transform.position);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(transform.forward, agent.desiredVelocity * -1);
        }

        animator.SetBool("Walk", agent.hasPath);
    }

    void FixedUpdate()
    {
        if (inRange && boneCooldown <= 0)
        {
            GameObject boneInst = Instantiate(bone, GameObject.Find("Grid").transform);

            boneInst.transform.position = transform.position;
            boneInst.GetComponent<Rigidbody2D>().velocity = (player.transform.position - boneInst.transform.position).normalized * projSpeed;

            boneCooldown = cooldown;
        }
        if (boneCooldown > 0) boneCooldown--;
    }
}
