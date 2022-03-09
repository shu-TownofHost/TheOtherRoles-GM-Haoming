using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace TheOtherRoles.Objects {
    public class Trap{
        public GameObject trap;
        public static AudioClip place;
        public static AudioClip activate;
        public static AudioClip disable;
        public static AudioClip countdown;
        public static AudioClip kill;
        public AudioSource audioSource;
        public static List<Trap> traps = new List<Trap>();
        public bool isActive;

        public void loadAudioClips()
        {
            // 音声ファイル読み込み 
            if(place== null)
                place = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.TrapperPlace.wav", false);
            if(activate == null)
                activate = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.TrapperActivate.wav", false);
            if(disable == null)
                disable = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.TrapperDisable.wav", false);
            if(kill == null )
                kill = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.TrapperKill.wav", false);
            if(kill == null)
                countdown = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.TrapperCountdown.wav", false);
        }

        public Trap(Vector2 p) {

            trap = new GameObject("Trap");
            audioSource = trap.gameObject.AddComponent<AudioSource>();
            var trapRenderer = Trapper.trap.AddComponent<SpriteRenderer>();
            Trapper.sound = new GameObject("TrapSound");
            audioSource.priority = 0;
            audioSource.spatialBlend = 1;
            audioSource.clip = Trapper.place;
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.minDistance = Trapper.minDsitance;
            audioSource.rolloffMode = Trapper.rollOffMode;
        }

        public static void clearAllTraps()
        {
        }

        public static void activateTrap(Trap trap)
        {

        }

        public static void disableTrap(Trap trap)
        {

        }


    }
}
