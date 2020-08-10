using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState{
    walk,
    attack,
    interact,
    stagger,
    idle
}


public class PlayerMovement : MonoBehaviour
{


    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;
    public GameObject projectile;


    //dash mechanics
    public ParticleSystem Dust;
    public float DashForce;
    public float StartDashTimer;
    float CurrentDashTimer;
    float DashDirection;
    bool isDashing;

    //---------------------------


    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        //just added
        if (Input.GetButtonDown("fireball") && currentState != PlayerState.attack
           && currentState != PlayerState.stagger)
        {
                StartCoroutine(SecondAttackCo());
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }

        if (Input.GetButtonDown("Dash"))
        {
                CreateDust();
                isDashing = true;
                CurrentDashTimer = StartDashTimer;
                // DashDirection = change.x;
                speed = 24;

        }

        if(isDashing)
        {
            myRigidBody.velocity = Vector3.zero;
            CurrentDashTimer -= Time.deltaTime;

            if(CurrentDashTimer <= 0)
            {
                speed = 8;
                isDashing = false;
            }
        }

	}

    private IEnumerator SecondAttackCo()
    {
        //animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        MakeFireball();
        //animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    //just added
    private void MakeFireball()
    {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            Fireball fireball = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Fireball>();
            fireball.Setup(temp, ChooseArrowDirection());
    }

    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX"))* Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }else{
            animator.SetBool("moving", false);
        }
    }


    void MoveCharacter()
    {
        myRigidBody.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }

    void CreateDust(){
        Dust.Play();
    }

}
