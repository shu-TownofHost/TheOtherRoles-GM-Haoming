using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace TheOtherRoles{
    public class TrapEffect {
        public static List<TrapEffect> trapeffects = new List<TrapEffect>();
        public GameObject trapeffect;
        private GameObject background = null;
        private AudioSource audioSource;

        private static Sprite trapeffectSprite;
        public static Sprite getTrapEffectSprite() {
            if (trapeffectSprite) return trapeffectSprite;
            trapeffectSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TrapEffect.png", 300f);
            return trapeffectSprite;
        }

        public TrapEffect(Vector3 pos) {
            trapeffect = new GameObject("TrapEffect");
            Vector3 position = new Vector3(pos.x, pos.y, PlayerControl.LocalPlayer.transform.localPosition.z - 0.001f); // just behind player
            trapeffect.transform.position = position;
            trapeffect.transform.localPosition = position;

            var trapeffectRenderer = trapeffect.AddComponent<SpriteRenderer>();
            trapeffectRenderer.sprite = getTrapEffectSprite();

            trapeffect.SetActive(true);
            // 音を鳴らす
            audioSource = this.trapeffect.gameObject.AddComponent<AudioSource>();
            audioSource.priority = 0;
            audioSource.spatialBlend = 1;
            audioSource.clip = Trapper.test;
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 30f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.PlayOneShot(Trapper.test);
            trapeffects.Add(this);

            // 5秒後にトラップの表示を消す
            if(!PlayerControl.LocalPlayer.isRole(RoleId.Trapper))
            {
                HudManager.Instance.StartCoroutine(Effects.Lerp(5f, new Action<float>((p) =>
                { // Delayed action
                    if (p == 1f)
                    {
                        trapeffect.SetActive(false);
                    }
                })));
            }
        }

        public static void clearTrapEffects() {
            foreach(TrapEffect trapeffect in trapeffects){
                trapeffect.trapeffect.SetActive(false);
                GameObject.Destroy(trapeffect.trapeffect);
            }
            trapeffects = new List<TrapEffect>();
        }

        public static void UpdateAll() {
            foreach (TrapEffect trapeffect in trapeffects) {
                if (trapeffect != null)
                    trapeffect.Update();
            }
        }

        public void Update() {
            if (background != null)
                background.transform.Rotate(Vector3.forward * 6 * Time.fixedDeltaTime);
        }
    }
}