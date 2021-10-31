using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerKeys keys;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Camera cam;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] float camSpeed;


    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 posZAdjust = new Vector3(pos.x, pos.y, 0);
        if (Input.GetKeyDown(keys.click))
        {
            NavMeshHit hit;

            if (agent.FindClosestEdge(out hit))
            {
                agent.SetDestination(hit.position);
            }

            agent.SetDestination(posZAdjust);
        }

        if (Input.GetKey(keys.moveCamera))
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            cam.transform.position -= (new Vector3(x, y, 0) * camSpeed);
        }

        transform.rotation = Quaternion.LookRotation(transform.forward, (posZAdjust - transform.position) * -1);

        animator.SetBool("Walk", agent.hasPath);

    }


}
