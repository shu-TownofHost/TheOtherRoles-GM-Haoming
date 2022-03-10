using System;
using System.Collections.Generic;
using System.Collections;
using HarmonyLib;
using Hazel;
using UnityEngine;
using TheOtherRoles.Patches;

namespace TheOtherRoles.Objects {
    public class Trap{
        public GameObject trap;
        public static Sprite trapSprite;
        public static Sprite trapActiveSprite;
        public static AudioClip place;
        public static AudioClip activate;
        public static AudioClip disable;
        public static AudioClip countdown;
        public static AudioClip kill;
         public static AudioRolloffMode rollOffMode = UnityEngine.AudioRolloffMode.Linear;
        private static byte maxId = 0;
        public AudioSource audioSource;
        public static Dictionary<byte, Trap> traps = new Dictionary<byte, Trap>();
        public bool isActive = false;
        public PlayerControl target;
        public DateTime placedTime;

        public static void loadAudioClips()
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

        public static void loadSprite()
        {
            if (trapSprite == null)
                trapSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.Trap.png", 300f);
            if (trapActiveSprite == null)
                trapActiveSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TrapActive.png", 300f);

        }

        private static byte getAvailableId()
        {
            byte ret = maxId;
            maxId++;
            return ret;
        }

        public Trap(Vector3 pos) {
            // 罠を設置
            trap = new GameObject("Trap");
            var trapRenderer = trap.AddComponent<SpriteRenderer>();
            trapRenderer.sprite = trapSprite;
            trap.transform.position = pos;
            trap.transform.localPosition = pos;
            trap.SetActive(true);

            // 音を鳴らす
            audioSource = trap.gameObject.AddComponent<AudioSource>();
            audioSource.priority = 0;
            audioSource.spatialBlend = 1;
            audioSource.clip = place;
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.maxDistance = 2 * Trapper.maxDistance/3;
            audioSource.minDistance = Trapper.minDsitance;
            audioSource.rolloffMode = rollOffMode;
            audioSource.PlayOneShot(place);

            // 設置時刻を設定
            placedTime = DateTime.UtcNow;

            traps.Add(getAvailableId(), this);
            
        }

        public static void clearAllTraps()
        {
            loadAudioClips();
            loadSprite();
            foreach(var trap in traps.Values)
            {
                if(trap.trap != null)
                    UnityEngine.GameObject.DestroyObject(trap.trap);
            }
            traps = new Dictionary<byte, Trap>();
        }

        public static void activateTrap(byte trapId, PlayerControl trapper, PlayerControl target)
        {
            var trap = traps[trapId];
            // 他のトラップを全て無効化する
            var newTraps = new Dictionary<byte, Trap>();
            newTraps.Add(trapId, trap);
            foreach(var t in traps.Values)
            {
                if(t.trap == null) continue;
                t.trap.SetActive(false);
                UnityEngine.GameObject.Destroy(t.trap);
            }
            traps = newTraps;

            // 有効にする
            trap.isActive = true;

            // 音を鳴らす
            trap.audioSource.loop = true;
            trap.audioSource.maxDistance = Trapper.maxDistance;
            trap.audioSource.clip = countdown;
            trap.audioSource.Play();

            // ターゲットを動けなくする
            target.NetTransform.Halt();

            bool moveableFlag = false;
            HudManager.Instance.StartCoroutine(Effects.Lerp(Trapper.killTimer, new Action<float>((p) => 
            {
                try
                {
                    if(Trapper.meetingFlag) return;
                    if(trap == null || trap.isActive == false) //　解除された場合の処理
                    {
                        if(!moveableFlag)
                        {
                            target.moveable = true;
                            moveableFlag = true;
                        }
                        return;
                    }
                    else if((p==1f) && target.isAlive()){ // 正常にキルが発生する場合の処理
                        target.moveable = true;
                        if(PlayerControl.LocalPlayer.isRole(RoleType.Trapper))
                        {
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.TrapperKill, Hazel.SendOption.Reliable, -1);
                            writer.Write(trapId);
                            writer.Write(PlayerControl.LocalPlayer.PlayerId);
                            writer.Write(target.PlayerId);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.trapperKill(trapId, PlayerControl.LocalPlayer.PlayerId, target.PlayerId);
                        }
                    }else{ // カウントダウン中の処理
                        target.moveable = false;
                        target.transform.position = trap.trap.transform.position;
                    }
                } catch (Exception e){
                    Helpers.log("カウントダウン中にエラー発生");
                    Helpers.log(e.Message);
                }
            })));
        }

        public static void disableTrap(byte trapId)
        {
            var trap = traps[trapId];
            trap.audioSource.Stop();
            trap.audioSource.PlayOneShot(disable);
            HudManager.Instance.StartCoroutine(Effects.Lerp(disable.length, new Action<float>((p) => 
            {
                if(p == 1f)
                {
                    if(trap.trap != null)
                        trap.trap.SetActive(false);
                        UnityEngine.GameObject.Destroy(trap.trap);
                    traps.Remove(trapId);
                }
            })));

            if(PlayerControl.LocalPlayer.isRole(RoleType.Trapper))
            {
                PlayerControl.LocalPlayer.killTimer = PlayerControl.GameOptions.KillCooldown + Trapper.penaltyTime;
                Trapper.trapperSetTrapButton.Timer = Trapper.cooldown + Trapper.penaltyTime;
            }
        }
        public static void trapKill(byte trapId, PlayerControl trapper, PlayerControl target)
        {
            var trap = traps[trapId];
            var audioSource = trap.audioSource;
            audioSource.Stop();
            audioSource.maxDistance = Trapper.maxDistance;
            audioSource.PlayOneShot(kill);
            HudManager.Instance.StartCoroutine(Effects.Lerp(kill.length, new Action<float>((p) => 
            {
                if(p ==1)
                {
                    if(trap.trap != null)
                        trap.trap.SetActive(false);
                        UnityEngine.GameObject.Destroy(trap.trap);
                    traps.Remove(trapId);
                }
            })));
            Trapper.isTrapKill = true;
            KillAnimationCoPerformKillPatch.hideNextAnimation = true;
            trapper.MurderPlayer(target);
        }

        public static void onMeeting()
        {
            Trapper.meetingFlag = true;
            foreach(var trap in traps)
            {
                trap.Value.audioSource.Stop();
                if(trap.Value.target != null)
                {

                    if(PlayerControl.LocalPlayer.isRole(RoleType.Trapper))
                    {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.TrapperKill, Hazel.SendOption.Reliable, -1);
                        writer.Write(PlayerControl.LocalPlayer.PlayerId);
                        writer.Write(trap.Value.target.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.trapperKill(trap.Key, PlayerControl.LocalPlayer.PlayerId, trap.Value.target.PlayerId);
                    }

                }
            }

        }

        public static bool isTrapped(PlayerControl p)
        {
            foreach(var trap in traps.Values)
            {
                if(trap.target == p) return true;
            }
            return false;
        }
        
        public static bool hasTrappedPlayer()
        {
            foreach(var trap in traps.Values)
            {
                if(trap.target != null) return true;
            }
            return false;
        }

        public static Trap getActiveTrap()
        {
            foreach(var trap in traps.Values)
            {
                if(trap.target != null) return trap;
            }
            return null;
        }

        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
        public static class PlayerPhysicsNinjaPatch
        {
            public static void Postfix(PlayerPhysics __instance)
            {
                foreach(var trap in Trap.traps.Values )
                {
                    bool canSee = 
                        trap.isActive ||
                        PlayerControl.LocalPlayer.isImpostor() ||
                        PlayerControl.LocalPlayer.isDead() ||
                        (PlayerControl.LocalPlayer.isRole(RoleType.Lighter) && Lighter.isLightActive(PlayerControl.LocalPlayer)) ||
                        PlayerControl.LocalPlayer.isRole(RoleType.Fox);
                    var opacity = canSee ? 0.1f : 0.0f;
                    trap.trap.GetComponent<SpriteRenderer>().material.color = Color.Lerp(Palette.ClearWhite, Palette.White, opacity);
                }
            }
        }
    }
}
