using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAudioController : MonoBehaviour
    {
        public FootstepPack[] stepPacks;
        public float threshold;
        public float rate;
        public float intensity;
        public AudioClip[] Clips;
        public float ClipVolume;

        private StarterAssetsInputs _input;
        private FirstPersonController _controller;
        private string currentTag;
        private int tagIndex;
        private bool footAB;
        private float walkDelta;
        private int footIndex;
        private float walkDeltaTimer;


        private Vector3 prevPos;
        // Start is called before the first frame update
        void Awake()
        {
            _input = GetComponent<StarterAssetsInputs>();
            _controller = GetComponent<FirstPersonController>();
        }

        private void Start()
        {
            currentTag = "default";
            footAB = false;
            prevPos = gameObject.transform.position;
            footIndex = 0;
        }

        void CheckGroundTag()
        {
            if (currentTag != _controller.GetGroundTag())
            {
                for (int i = 0; i < stepPacks.Length; i++)
                {
                    if (stepPacks[i].tag == currentTag)
                    {
                        tagIndex = i;
                        break;
                    }
                }

                currentTag = _controller.GetGroundTag();
            }
        }

        // Update is called once per frame
        void Update()
        {
            CheckGroundTag();

            if (_input.move.magnitude > threshold)
            {
                walkDelta = (gameObject.transform.position - prevPos).magnitude;
                prevPos = gameObject.transform.position;
                walkDeltaTimer += Time.deltaTime * (walkDelta * intensity);
            }
            else
            {
                footIndex = 0;
            }

            int roundedDelta = Mathf.RoundToInt(walkDelta);
            if (walkDeltaTimer > rate)
            {
                walkDeltaTimer = 0;
                float volume = (footIndex % 2 == 0) ? Random.Range(0.075f, 0.3f) : Random.Range(0.05f, 0.15f);
                AudioSource.PlayClipAtPoint(stepPacks[tagIndex].clips[footIndex % 5], transform.position + Vector3.down * 0.5f, volume);
                footIndex++;
            }
        }
    }
}
