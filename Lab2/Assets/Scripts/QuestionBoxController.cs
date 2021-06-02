using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxController : MonoBehaviour
{
    public  Rigidbody2D rigidBody;
    public  SpringJoint2D springJoint;
    public  GameObject consummablePrefab; // the spawned mushroom prefab
    public  SpriteRenderer spriteRenderer;
    public  Sprite usedQuestionBox; // the sprite that indicates empty box instead of a question mark
    private bool _hit =  false;


    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && !_hit) {
            _hit = true;
            var t = transform.position;
            Instantiate(consummablePrefab, new Vector3(t.x, t.y + 1.0f, t.z), Quaternion.identity);
        }
    }
}
