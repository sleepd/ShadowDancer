using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public int maxJumpCount = 2;
    public LayerMask groundLayer;
    public VisualEffect landingEffect;
    public float maxFallSpeed = -20f;

    // sound effects
    [SerializeField]
    AudioSource sfx;
    [SerializeField]
    AudioClip sfx_landing;

    private Rigidbody2D _rb;
    private GameObject _sprite;
    private Animator _animator;
    
    
    // temporary velocity per frame
    private Vector2 _velocity;
    // use to check if the player is starting to fall
    private Vector2 _preVelocity;
    // double jump count, support triple jump
    private int _jumpCount = 0;
    public float _jumpCooldown = 0.1f;
    private float _jumpCooldownTimer = 0f;
    private bool _jumpPressed = false;

    // the boolean to record if the player is on the ground
    private bool _onGround = true;
    // the boolean to record if the player is on the ground in the previous frame
    // for landing animation
    private bool _preOnGround = true;



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
        if (Input.GetButtonDown("Jump") && _jumpCooldownTimer <= 0)
        {
            if (_onGround)
            {
                _jumpPressed = true;
                _jumpCount++;
                Debug.Log("Jump");
            }

            else if (_jumpCount < maxJumpCount)
            {
                _jumpPressed = true;
                _jumpCount++;
                Debug.Log("Double Jump");
            }

        }

        if (_jumpCooldownTimer > 0)
        {
            _jumpCooldownTimer -= Time.deltaTime;
        }


        // fall
        if (!_onGround)
        {
            if (_preVelocity.y >= 0 && _velocity.y < 0){
                _animator.SetTrigger("FallStart");
            }
        }

        float inputX = Input.GetAxisRaw("Horizontal");
        inputX = Mathf.Abs(inputX) < 0.1f ? 0f : Mathf.Sign(inputX);
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
        if (_velocity.y < maxFallSpeed) _velocity.y = maxFallSpeed;
        _rb.linearVelocity = _velocity;

        if (_jumpPressed)
        {
            Jump();
            _jumpPressed = false;
        }
    }



    void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        _animator.SetTrigger("JumpStart");
        _jumpCooldownTimer = _jumpCooldown;
    }




    void OnGroundCheck()
    {
        _preOnGround = _onGround;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, groundLayer);

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
        if (!_preOnGround && _onGround)
        {
            _animator.SetTrigger("Landing");
            Debug.Log("Landing");
            _jumpCount = 0;
            landingEffect.SendEvent("PlayerLanding");
            sfx.Play();
        }
    }

    public void OnLandingAnimationEnd()
    {
        _animator.ResetTrigger("Landing");
    }



}
