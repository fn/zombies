using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //serialize field lets the editor see and interact with the variables.
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;

    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpsMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] GameObject cube;

    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;

    bool isShooting;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.blue);
        movement();
        sprint();
    }

    void movement()
    {
        //moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //transform.position += moveDir*speed * Time.deltaTime;

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = Input.GetAxis("Vertical") * transform.forward +
                  Input.GetAxis("Horizontal") * transform.right;
        controller.Move(moveDir * speed * Time.deltaTime);
        //deltaTime makes the game always run reguardless of frame rate or how poorly the user computer runs.

        if (Input.GetButtonDown("Jump") && jumpCount < jumpsMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }

        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(shoot());
        }
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        //Where is it coming from? What direction is it going? What will it return? How long will the bullet travel?
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreMask))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(shootDamage);
                //Instantiate(cube, hit.point, transform.rotation);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
