using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace TheOtherRoles{
    class TrapEffect {
        public static List<TrapEffect> trapeffects = new List<TrapEffect>();

        public GameObject trapeffect;
        private GameObject background = null;

        private static Sprite trapeffectSprite;
        public static Sprite getTrapEffectSprite() {
            if (trapeffectSprite) return trapeffectSprite;
            trapeffectSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TrapEffect.png", 300f);
            return trapeffectSprite;
        }

        public TrapEffect(PlayerControl player) {
            trapeffect = new GameObject("TrapEffect");
            Vector3 position = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z - 0.001f); // just behind player
            trapeffect.transform.position = position;
            trapeffect.transform.localPosition = position;

            var trapeffectRenderer = trapeffect.AddComponent<SpriteRenderer>();
            trapeffectRenderer.sprite = getTrapEffectSprite();
            trapeffect.SetActive(true);
            trapeffects.Add(this);
        }

        public static void clearTrapEffects() {
            foreach(TrapEffect trapeffect in trapeffects){
                trapeffect.trapeffect.SetActive(false);
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