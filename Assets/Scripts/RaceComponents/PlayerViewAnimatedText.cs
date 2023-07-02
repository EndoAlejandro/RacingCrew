using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RaceComponents
{
    public class PlayerViewAnimatedText : MonoBehaviour
    {
        private static readonly int IsActive = Animator.StringToHash("IsActive");

        private TMP_Text _bannerText;
        private Animator _animator;

        private void Awake()
        {
            _bannerText = GetComponentInChildren<TMP_Text>();
            _animator = GetComponent<Animator>();
        }

        private void Start() => _bannerText.gameObject.SetActive(false);

        public void PlayAnimation(string text, float duration) => StartCoroutine(PlayAnimationAsync(text, duration));

        public void SetLayer(int layer)
        {
            transform.parent.gameObject.layer = layer;
            gameObject.layer = layer;
            foreach (Transform child in transform) child.gameObject.layer = layer;
        }

        private IEnumerator PlayAnimationAsync(string text, float duration)
        {
            _bannerText.gameObject.SetActive(false);
            _bannerText.SetText(String.Empty);
            _animator.SetBool(IsActive, true);
            yield return null;
            _bannerText.SetText(text);
            _bannerText.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            _animator.SetBool(IsActive, false);
        }
    }
}