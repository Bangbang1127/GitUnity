using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;


public class Player : BaseRobot
{
    static Player instance;
    public static Player GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
    public List<WeaponBase> Weapons;
    public Transform Hand;
    public float WalkSpeed = 10;
    

    private WeaponBase CurWeapon;
    private int CurWeaponIdx = 0;

    private Animator animator;
    private CharacterController cc;


    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        CurWeapon = Weapons[CurWeaponIdx];
    }

    void Update()
    {
        var trans = Camera.main.transform;
        var up = Vector3.ProjectOnPlane(trans.up, Vector3.up);
        var right = Vector3.ProjectOnPlane(trans.right, Vector3.up);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        var moveDirection = v * up + h * right;
        cc.Move(moveDirection.normalized * WalkSpeed * Time.deltaTime);
        animator.SetFloat("Speed", cc.velocity.magnitude / WalkSpeed);

        var r = GetAmiPoint();
        RotateToTarget(r);

        if(Input .GetButton ("Fire1"))
        {
            Shoot();
        }

        //switch weapon
        float f = Input.GetAxis("Mouse ScrollWheel");
        if (f > 0) { NextWeapon(1); }
        else if (f < 0) { NextWeapon(2); }

        //ui
        HUD.GetInstance().UpdateHpUI(hp);
        HUD.GetInstance().UpdateWeaponUI(CurWeapon.icon, CurWeapon.bulletNum);
    }

  

    public void NextWeapon(int step)
    {
        var idx = (CurWeaponIdx + step + Weapons.Count) % Weapons.Count;
        CurWeapon.gameObject.SetActive(false);
        CurWeapon = Weapons[idx];
        CurWeapon.gameObject.SetActive(true);
        CurWeaponIdx = idx;

    }

    public void AddWeapon(GameObject weapon)
    {
        for(int i = 0;i<Weapons .Count;i++)
        {
            if(Weapons [i].gameObject .name ==weapon .name )
            {
                Weapons[i].bulletNum += 15;
                return;
            }
        }
        var new_weapon = GameObject.Instantiate(weapon, Hand);
        new_weapon.name = weapon.name;
        new_weapon.transform.localRotation = CurWeapon.transform.localRotation;
        Weapons.Add(new_weapon.GetComponent<WeaponBase>());
        NextWeapon(Weapons.Count - 1 - CurWeaponIdx);
    }


public Vector3 GetAmiPoint()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if(Physics .Raycast (camRay ,out floorHit ,100.0f,LayerMask .GetMask("Floor")))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0;
            return playerToMouse;
        }
        return Vector3.zero;
    }

    public void RotateToTarget(Vector3 rot)
    {
        transform.LookAt(rot + transform.position);
    }
    
    public void Shoot()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("idle"))
        {
            animator.SetBool("Shoot", true);
        }
        //else
        //{
        //    animator.SetBool("Shoot", false);
        //}
            
    }

    public override void OpenFire()
    {
        base.OpenFire();
        //
        //Debug.Log ("openfire shoot ");

        CurWeapon.OpenFire(transform.forward);
    }
}

