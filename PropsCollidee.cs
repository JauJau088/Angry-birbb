using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsCollidee : MonoBehaviour
{
    [SerializeField] private string sound;
    private AudioManager audio;

    private void Start() {
        audio = FindObjectOfType<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Vector2 impulse = Vector2.zero;
        int contactCount = collision.contactCount;

        for (int i = 0; i < contactCount; i++) {
            var contact = collision.GetContact(0);
            impulse += contact.normal * contact.normalImpulse;
            impulse.x += contact.tangentImpulse * contact.normal.y;
            impulse.y -= contact.tangentImpulse * contact.normal.x;
        }

        //Debug.Log(impulse);

        if (impulse.x >= 2 || impulse.y >= 2) {
            audio.Play(sound);
        }
    }
}
