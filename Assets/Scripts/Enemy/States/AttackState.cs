using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            //lock lose player timer and incremenet move and shot timers
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);

            if(shotTimer > enemy.fireRate)
            {
                Shoot();
            }

            //move enemy to a random position after a random time
            if (moveTimer > Random.Range (3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
            enemy.LastKnowPos = enemy.Player.transform.position; 
        }
        else //lost sight of player
        {
            losePlayerTimer += Time.deltaTime;

            if (losePlayerTimer  > 8)
            {
                //change to search state
                stateMachine.ChangeState(new SearchState());
            }
        }
    }

    public void Shoot()
    {
        Transform gunBarrel = enemy.gunBarrel;
        //instantiate a new bullet from resources folder
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunBarrel.position, enemy.transform.rotation);

        //calculate direction to player
        Vector3 shootDirection = (enemy.Player.transform.position - gunBarrel.transform.position).normalized;
        //adding force rigidbody
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40;
        shotTimer = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
