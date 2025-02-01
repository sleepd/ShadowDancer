using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    private Rigidbody2D _rb;
    private GameObject _sprite;
    private Animator _animator;
    private Vector2 _velocity;
    private Vector2 _preVelocity;
    

    private bool _onGround = true;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = transform.Find("Sprite").gameObject;
        _animator = _sprite.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _velocity = _rb.linearVelocity;
        OnGroundCheck();
        LandingCheck();

        // jump
        if (Input.GetKey(KeyCode.Space) && _onGround)
        {
            Jump();
        }

        // fall
        if (!_onGround)
        {
            if (_preVelocity.y >= 0 && _velocity.y < 0){
                _animator.SetTrigger("FallStart");
            }
        }

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        _velocity = new Vector2(inputX * moveSpeed, _velocity.y);

        if(inputX != 0)
        {
            _animator.SetBool("Running", true);
            if(inputX < 0) _sprite.transform.localScale = new Vector2(1, 1);
            if(inputX > 0) _sprite.transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            _animator.SetBool("Running", false);
        }
        _preVelocity = _velocity;
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _velocity;
    }

    void Jump()
    {
        _velocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y + jumpForce);
        _animator.SetTrigger("JumpStart");
    }

    void OnGroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.05f, groundLayer);

        if (hit.collider != null)
        {
            _onGround = true;
        }
        else
        {
            _onGround = false;
        }
    }

    void LandingCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, groundLayer);
        if (hit.collider != null && !_onGround)
        {
            _animator.SetTrigger("Landing");
            Debug.Log("Landing");
        }
    }
}
