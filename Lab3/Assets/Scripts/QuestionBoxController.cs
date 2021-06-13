using System.Collections;
using UnityEngine;

public class QuestionBoxController : MonoBehaviour {
    public Rigidbody2D rigidBody;
    public SpringJoint2D springJoint;
    public GameObject consummablePrefab; // the spawned mushroom prefab
    public SpriteRenderer spriteRenderer;
    public Sprite usedQuestionBox; // the sprite that indicates empty box instead of a question mark
    private bool _hit = false;


    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && !_hit) {
            _hit = true;
            rigidBody.AddForce(new Vector2(0, rigidBody.mass * 20), ForceMode2D.Impulse);
            var t = transform.position;
            Instantiate(consummablePrefab, new Vector3(t.x, t.y + 1.0f, t.z), Quaternion.identity);
            StartCoroutine(DisableHittable());
        }
    }

    bool ObjectMovedAndStopped() {
        return Mathf.Abs(rigidBody.velocity.magnitude) < 0.1;
    }

    IEnumerator DisableHittable() {
        if (!ObjectMovedAndStopped()) {
            yield return new WaitUntil(() => ObjectMovedAndStopped());
        }

        //continues here when the ObjectMovedAndStopped() returns true
        spriteRenderer.sprite = usedQuestionBox; // change sprite to be "used-box" sprite
        rigidBody.bodyType = RigidbodyType2D.Static; // make the box unaffected by Physics

        //reset box position
        this.transform.localPosition = Vector3.zero;
        springJoint.enabled = false; // disable spring
    }
}