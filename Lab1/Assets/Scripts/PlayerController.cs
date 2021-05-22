using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float speed;
    public float upSpeed;
    public float maxSpeed;
    public float linearDrag;
    public float gravity;

    private bool _onGroundState = true;
    private SpriteRenderer _marioSprite;
    private Rigidbody2D _marioBody;
    private bool _faceRightState = true;
    
    public Transform enemyLocation;
    public Text scoreText;
    private int _score = 0;
    private bool _countScoreState = false;

    
    
    // Start is called before the first frame update
    void Start() {
        Application.targetFrameRate = 30;
        _marioBody = GetComponent<Rigidbody2D>();
        _marioSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (!_onGroundState && _countScoreState) {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f) {
                _countScoreState = false;
                _score++;
                Debug.Log(_score);
            }
        }
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        MoveMario(moveHorizontal);
        
        if (Input.GetKeyDown("space") && _onGroundState) {
            Jump();
            _countScoreState = true;
        }
        
        ModifyDrag(moveHorizontal);
    }
    
    private void Jump() {
        _marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
        _onGroundState = false;
    }

    private void MoveMario(float moveHorizontal) {
        // move by add force
        Vector2 movement = new Vector2(moveHorizontal, 0);
        _marioBody.AddForce(movement * speed);
        
        // change sprite looking direction
        if (moveHorizontal < 0 && _faceRightState) {
            _faceRightState = false;
            _marioSprite.flipX = true;
        }else if(moveHorizontal > 0 && !_faceRightState) {
            _faceRightState = true;
            _marioSprite.flipX = false;
        }
        
        // set max speed 
        if (Mathf.Abs(_marioBody.velocity.x) > maxSpeed) {
            _marioBody.velocity = new Vector2(Mathf.Sign(_marioBody.velocity.x) * maxSpeed, _marioBody.velocity.y);
        }
    }

    private void ModifyDrag(float moveHorizontal) {
        bool changingDirections = (moveHorizontal > 0 && _marioBody.velocity.x < 0) ||  (moveHorizontal < 0 && _marioBody.velocity.x > 0);

        if(_onGroundState) {
            if(Mathf.Abs(moveHorizontal) < 0.4f || changingDirections){
                _marioBody.gravityScale = 1;
                _marioBody.drag = linearDrag;
            }else{
                _marioBody.gravityScale = 0f;
                _marioBody.drag = 0f;
            }
        }else {
            _marioBody.gravityScale = gravity;
            _marioBody.drag = linearDrag * 0.15f;
        }
        
    }
    
    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground")) {
            _onGroundState = true;
            _countScoreState = false;
            scoreText.text = "Score: " + _score.ToString();
        }
            
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Debug.Log("Collided with Gomba!");
        }
    }
}