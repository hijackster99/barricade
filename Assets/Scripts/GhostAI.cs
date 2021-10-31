using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float range;

    GameObject player;

    Vector2 currentDir;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("MainCharacter");

        float angle = Random.Range(0, 360);
        currentDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.transform.position - transform.position).magnitude <= range)
        {
            Vector3 dir = player.transform.position - transform.position;
            Vector2 dir2D = new Vector2(dir.x, dir.y);
            rb.velocity = dir2D.normalized * speed;
        }
        else
        {
            rb.velocity = currentDir.normalized * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerController>().damaage();
        }
        else if (collision.gameObject.tag == "Walls")
        {
            if(collision.contacts.Length > 0)
            { 
                currentDir = Vector2.Reflect(currentDir, collision.contacts[0].normal).normalized;
            }
        }
    }
}
