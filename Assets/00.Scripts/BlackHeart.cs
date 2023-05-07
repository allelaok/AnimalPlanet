using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHeart : MonoBehaviour
{
    [SerializeField]
    Transform towards;
    EnemyToward[] targets;

    EnemyToward target;
    EnemyToward before;

    private void Start()
    {
        targets = towards.GetComponentsInChildren<EnemyToward>();
        RandomPosition();
    }

    public void RandomPosition()
    {
        int i = Random.Range(0, targets.Length - 1);
        before = targets[i];
        transform.position = before.transform.position;

        target = before.Next(before);
        transform.up = target.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.state == STATE.Ready)
        {
            int i = Random.Range(0, targets.Length - 1);
            target = targets[i];

            transform.position = target.transform.position;
        }

        if (Player.state != STATE.Play) return;

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, GameManager.instance.Speed * Time.deltaTime);
    }
    private void LateUpdate()
    {
        if (Player.state != STATE.Play) return;

        if (transform.position == target.transform.position)
        {
            EnemyToward tmp = target;
            target = target.Next(before);
            transform.up = target.transform.position - transform.position;
            before = tmp;
        }
    }

    [SerializeField] Transform player;
    [SerializeField] float dis = 5;
    [SerializeField] float v = 0.8f;
    public void WingSound()
    {
        if (Player.state != STATE.Play) return;

        float d = Vector3.Distance(player.position, transform.position);
        if (d < dis)
        {
            SoundManager.instance.WingSound((1 - d / dis) * v);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Player.state != STATE.Play || Player.invincibility) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pet"))
        {
            print("coll");
            Player.state = STATE.GameOver;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.state != STATE.Play || Player.invincibility) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pet"))
        {
            print("coll");
            Player.state = STATE.GameOver;
        }
    }
}
