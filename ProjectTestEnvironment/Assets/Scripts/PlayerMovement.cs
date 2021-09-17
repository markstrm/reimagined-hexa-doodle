using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] Camera _Camera;

    PlayerInputActions _Input;
    Rigidbody2D _Rigidbody;
    public Bullet bulletPrefab;

    private Vector2 movement;
    private Vector2 _MousePos;

    private bool canShoot = true;

    private void Awake()
    {
        _Input = new PlayerInputActions();
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.localPosition += Vector3.right * speed * Time.deltaTime * movement.x;
        transform.localPosition += Vector3.up * speed * Time.deltaTime * movement.y;


    }

    private void FixedUpdate()
    {
        Vector2 facingDirection = _MousePos - _Rigidbody.position;
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg -90;
        _Rigidbody.MoveRotation(angle);
    }

    private void OnEnable()
    {
        _Input.Enable();

        _Input.Player.MousePos.performed += OnMousePos;
    }

    private void OnDisable()
    {
        _Input.Disable();
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    private void OnMousePos(InputAction.CallbackContext context)
    {
        _MousePos = _Camera.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!canShoot) return;

            Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation); //spawns the bullet and gives it a position and rotatio. Will spawn at the players positon and in the same rotation as the player.
            bullet.Project(this.transform.up); //projects in the same positon as the player
            StartCoroutine(CanShoot());
        }

    }

    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.35f);
        canShoot = true;
    }

}
