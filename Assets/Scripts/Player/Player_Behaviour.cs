using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class Player_Behaviour : MonoBehaviour
{
    [TextArea]

    Animator anim;
     Rigidbody rb;
    [Tooltip("Requires an MainCamera with Anim component and Main Camera tag in it")]
    GameObject Damera;
    bool attack,attack2;
    [Tooltip("Skips the stand anim")]
    public bool skip;
    [Tooltip("Change the force that will be applicated to the object")]
    int speed;
    public int run;
    public int walk;
    Vector3 newpos;
    Quaternion newrot;

   
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();
        Damera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        //Rb velocity check
        if (rb.velocity.magnitude >= 0.1f)
        {

            anim.SetFloat("MovementSpeed", rb.velocity.magnitude);
            RotateToVelocity(15f, false);
        }
        if (rb.velocity.magnitude <= 0.1f)
        {

            anim.SetFloat("MovementSpeed", 1);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
        }

        //end
        transform.rotation = new Quaternion(0,transform.rotation.y,0,transform.rotation.w);
        if (skip && anim.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
        {
            anim.Play("Idle");
            Damera.GetComponent<Animator>().speed = 10;
        }
        //Stand anim and Walk or Run
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Stand")||skip) {
            if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) || !Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S))
            {

                speed = walk;
                Walk();
            }
            else if (Input.GetKey(KeyCode.LeftShift)&&Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S)) {
                speed = run;
                Walk();
            }
            }
        //attack
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Attack("Sattack");
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack("Dattack");
        }
        //Check if Player is behind an object
        ObjCheck();
    }
    private void ObjCheck()
    {
        RaycastHit hit;
        

        int layerMask = 1 << 8;
        if (Physics.Linecast(Damera.transform.position,gameObject.transform.position,out hit))
        {
            Debug.DrawLine(Damera.transform.position,gameObject.transform.position);
            if (hit.transform.gameObject.tag == "Wall")
            {
                hit.transform.gameObject.GetComponent<WallBehaviour>().timer = 13;
            }
        }
        
    }
    void Walk()
    {
        //Vertical e Horizontal
        if (!attack&& rb.velocity.magnitude <= 4) {
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(Vector3.forward * speed * Time.deltaTime);
                //gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                anim.SetBool("Walk", true);

            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector3.left * speed * Time.deltaTime);
                //gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
                anim.SetBool("Walk", true);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(Vector3.back * speed * Time.deltaTime);
                //gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                anim.SetBool("Walk", true);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(Vector3.right * speed * Time.deltaTime);
                //gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                anim.SetBool("Walk", true);
            }
        }
        //
        if (speed ==run)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", true);
        }
        if (speed == walk)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Run", false);
        }
        
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Sattack")&&!anim.GetCurrentAnimatorStateInfo(1).IsName("Dattack"))
        {
            anim.SetLayerWeight(1, 0);
            attack = false;
        }
        
    }
    void Attack(string Attackname)
    {
        if (rb.velocity.magnitude <= 0.1f&&Input.GetKeyDown(KeyCode.Mouse1)&&!anim.GetCurrentAnimatorStateInfo(1).IsName("Dattack"))
        {
            attack = true;
            rb.velocity = Vector3.zero;
            anim.SetLayerWeight(1,(float)Mathf.Lerp(0,1,2));
            anim.Play(Attackname);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)&&!anim.GetCurrentAnimatorStateInfo(1).IsName("Sattack"))
        {
            
            
            anim.SetLayerWeight(1, (float)Mathf.Lerp(0, 1, 2));
            anim.Play(Attackname);
        }

    }
    
    
    public void RotateToVelocity(float turnSpeed, bool ignoreY)
    {
        Vector3 dir; if (ignoreY) dir = new Vector3(rb.velocity.x, 0f, rb.velocity.z); else dir = rb.velocity; if (dir.magnitude > 0.1)
        {
            Quaternion dirQ = Quaternion.LookRotation(dir);
            Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, dir.magnitude * turnSpeed * Time.deltaTime);
            rb.MoveRotation(slerp);
        }
    }
}
// void Dattack()
//{


//   transform.position = GameObject.Find("Player Variant/mixamorig:Hips").transform.position;


// }
