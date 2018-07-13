using System.Collections;
using UnityEngine;

namespace Narrative
{
    public class ActorObject : MonoBehaviour
    {
        public Actor actor;

        public Transform ActorTransform
        {
            get
            {
                foreach (Transform child in transform)
                    if (child.tag == "Scene")
                        return child.transform;
                return transform;
            }
        }

        private SpriteRenderer sprite;

        private void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        public void Hide()
        {
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            if (sprite != null)
                while (sprite.color.a > 0)
                {
                    var color = sprite.color;
                    color.a -= .1f;
                    sprite.color = color;
                    yield return new WaitForSecondsRealtime(.05f);
                }
        }

        public void Show()
        {
            StartCoroutine(FadeIn());
        }
        private IEnumerator FadeIn()
        {
            if (sprite != null)
                while (sprite.color.a < 1)
                {
                    var color = sprite.color;
                    color.a += .1f;
                    sprite.color = color;
                    yield return new WaitForSecondsRealtime(.05f);
                }
        }

        public void SetSprite(TaskEmotion emotion)
        {
            sprite.sprite = actor.neutral;
        }
    }
}