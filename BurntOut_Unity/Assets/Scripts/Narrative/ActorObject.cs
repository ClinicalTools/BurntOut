using System.Collections;
using UnityEngine;

namespace Narrative
{
    public class ActorObject : MonoBehaviour
    {
        public Actor actor;

        private SpriteRenderer sprite;

        private void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        // for clicking mechanics
        private void OnMouseUpAsButton()
        {
            if (!Main_GameManager.Instance.isCurrentlyExamine)
                DialogueManager.Instance.ActorInteract(this);
        }

        public void Hide()
        {
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
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