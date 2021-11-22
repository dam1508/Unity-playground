using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] float jumpResponse = 1F;
    [SerializeField] GameObject hook;
    [SerializeField] Gun gun;


    float horizontalMovement = 0F;
    bool jump = false;
    float timer = 0.0F;

    bool hookShot = false;
    bool grappled = false;

    Animator anim;
    PlayerControl playerControl;

    int currentWeapon = 0;
    List<GameObject> weapons = new List<GameObject>();

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>();

        FindChildren();
        SetWeapon(currentWeapon);

        Debug.Log(-1 % 4);
    }

    void SetWeapon(int index)
    {
        foreach(GameObject weapon in weapons)
        {
            weapon.SetActiveRecursively(false);
        }

        weapons[index].SetActiveRecursively(true);
    }
    
    void FindChildren()
    {
        foreach(Transform child in transform)
        {
            if (child.tag == "Weapon")
            {
                weapons.Add(child.gameObject); 
            }
        }
    }
    
    void OnJump()
    {
        Jump();
    }

    void OnMovePlayer(InputValue value)
    {
        horizontalMovement = value.Get<float>();
    }

    void OnFire()
    {
        HandleFire();
    }

    private void HandleFire()
    {
        switch (currentWeapon)
        {
            case 0:
                gun.Fire();
                break;
            case 1:
                anim.SetTrigger("Swing");
                break;
            default:
                break;
        }
    }

    void OnScroll(InputValue value)
    {
        float currentValue = value.Get<float>();
        int help = currentWeapon;

        if(currentValue > 0)
        {
            help++;
        }
        if (currentValue < 0)
        {
            help--;
        }

        currentWeapon = (help + weapons.Count) % weapons.Count;
        SetWeapon(currentWeapon);
    }

    void OnGrapple()
    {
        if (!hookShot)
        {
            Instantiate(hook, transform.GetChild(2));
            hookShot = true;
        }
        else
        {
            DeattachHook();
        }

    }


    private void Update()
    {
        JumpTimer();
    }

    private void FixedUpdate()
    {
        playerControl.Move(horizontalMovement, false, jump, grappled);
        
    }

    private void JumpTimer()
    {
        if (timer < 3.0F)
        {
            timer += Time.deltaTime; 
        }
        if (timer >= jumpResponse)
        {
            jump = false;
        }
    }

    void Jump()
    {
        jump = true;
        timer = 0.0F;
        
    }
    public void DeattachHook()
    {
        transform.GetChild(2).GetChild(0).GetComponent<Hook>().DestroyHook();
        grappled = false;
        hookShot = false;
    }

    public void SetGrappled(bool gr)
    {
        grappled = gr;
    }

}
