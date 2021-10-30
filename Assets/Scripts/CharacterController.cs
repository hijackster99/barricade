using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] CharacterKeys keys;
    [SerializeField] Rigidbody2D rb;

    Vector2 endPos;
    Vector2 nextPos;

    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keys.click))
        {
            SetMove(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }

        if (endPos != new Vector2(-1, -1))
        {
            if (nextPos == new Vector2(-1, -1)) nextPos = calcNextStep(new Vector2(transform.position.x, transform.position.y), endPos);
            else if (checkWithinLimit(nextPos, new Vector2(transform.position.x, transform.position.y), 0.1f)) nextPos = new Vector2(-1, -1);
            if (checkWithinLimit(endPos, new Vector2(transform.position.x, transform.position.y), 0.1f)) endPos = new Vector2(-1, -1);
            Vector2 dir = nextPos - new Vector2(transform.position.x, transform.position.y);

            rb.velocity = speed * dir;
        }
    }

    bool checkWithinLimit(Vector2 next, Vector2 current, float epsilon) {
        return (current - next).magnitude <= epsilon;
    }

    void SetMove(Vector2 endPos) {
        this.endPos = endPos;
    }


}
