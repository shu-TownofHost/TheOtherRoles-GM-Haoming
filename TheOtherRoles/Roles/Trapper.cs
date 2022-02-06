using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;
using TheOtherRoles.Objects;
using TheOtherRoles.Patches;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.GameHistory;

namespace TheOtherRoles
{
    [HarmonyPatch]
    public class Trapper : RoleBase<Trapper>
    {
        public static Color color = Palette.CrewmateBlue;
        public static Sprite trapButtonSprite;
        public static GameObject trap;
        public static PlayerControl trappedPlayer;
        public static bool playingKillSound;
        public static DateTime placedTime;
        public static AudioClip place;
        public static AudioClip activate;
        public static AudioClip disable;
        public static AudioClip countdown;
        public static AudioClip kill;
        public static AudioRolloffMode rollOffMode = UnityEngine.AudioRolloffMode.Linear;
        public static float extensionTime {get {return CustomOptionHolder.trapperExtensionTime.getFloat();}}
        public static float killTimer {get {return CustomOptionHolder.trapperKillTimer.getFloat();}}
        public static float cooldown {get {return CustomOptionHolder.trapperCooldown.getFloat();}}
        public static float minDsitance {get {return CustomOptionHolder.trapperMinDistance.getFloat();}}
        public static float maxDistance {get {return CustomOptionHolder.trapperMaxDistance.getFloat();}}
        public static float trapRange {get {return CustomOptionHolder.trapperTrapRange.getFloat();}}
        public static float penaltyTime {get {return CustomOptionHolder.trapperPenaltyTime.getFloat();}}
        public static float bonusTime {get {return CustomOptionHolder.trapperBonusTime.getFloat();}}

        public static bool meetingFlag;
        

        public Trapper()
        {
            RoleType = roleId = RoleId.NoRole;
        }

        public override void OnMeetingStart() { }
        public override void OnMeetingEnd() 
        {
            unsetTrap();
            meetingFlag = false;
        }

        public override void FixedUpdate() 
        {
            if(DateTime.UtcNow.Subtract(placedTime).TotalSeconds < extensionTime) return;
            try{
                if (PlayerControl.LocalPlayer.isRole(RoleId.Trapper) && trap != null && trappedPlayer == null && !playingKillSound)
                {
                    // トラップを踏んだプレイヤーを動けなくする 
                    foreach(var p in PlayerControl.AllPlayerControls)
                    {
                        if(p.isDead() || p.inVent) continue;
                        var p1 = p.transform.localPosition;
                        Dictionary<GameObject, byte> listActivate = new Dictionary<GameObject, byte>();
                        var p2 = trap.transform.localPosition;
                        var distance = Vector3.Distance(p1, p2);
                        if(distance < trapRange)
                        {
                            TMPro.TMP_Text text;
                            RoomTracker roomTracker =  HudManager.Instance?.roomTracker;
                            GameObject gameObject = UnityEngine.Object.Instantiate(roomTracker.gameObject);
                            UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<RoomTracker>());
                            gameObject.transform.SetParent(HudManager.Instance.transform);
                            gameObject.transform.localPosition = new Vector3(0, -1.8f, gameObject.transform.localPosition.z);
                            gameObject.transform.localScale = Vector3.one * 2f;
                            text = gameObject.GetComponent<TMPro.TMP_Text>();
                            text.text = p.name + "が罠にかかった";
                            HudManager.Instance.StartCoroutine(Effects.Lerp(3f, new Action<float>((p) => {
                                if (p == 1f && text != null && text.gameObject != null) {
                                    UnityEngine.Object.Destroy(text.gameObject);
                                }
                            })));
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ActivateTrap, Hazel.SendOption.Reliable, -1);
                            writer.Write(PlayerControl.LocalPlayer.PlayerId);
                            writer.Write(p.PlayerId);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.activateTrap(PlayerControl.LocalPlayer.PlayerId, p.PlayerId);
                            break;
                        }
                    }
                }

                if(PlayerControl.LocalPlayer.isRole(RoleId.Trapper) && trappedPlayer != null && trap != null)
                {
                    // トラップにかかっているプレイヤーを救出する
                    Vector3 p1 = trap.transform.position;
                    foreach(var player in PlayerControl.AllPlayerControls)
                    {
                        if (player.PlayerId == trappedPlayer.PlayerId || player.isDead() || player.inVent|| player.isRole(RoleId.Trapper)) continue;
                        Vector3 p2 = player.transform.position;
                        float distance = Vector3.Distance(p1, p2);
                        if(distance < 0.5)
                        {
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DisableTrap, Hazel.SendOption.Reliable, -1);
                            writer.Write(player.PlayerId);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.disableTrap(player.PlayerId);
                        }
                    }
                }
            }
            catch (NullReferenceException e){
                Helpers.log(e.Message);
            }
        }
    public override void OnKill(PlayerControl target) 
    {
        if (target == Trapper.trappedPlayer)
        {
            player.killTimer = PlayerControl.GameOptions.KillCooldown - bonusTime;
            trapperSetTrapButton.Timer = cooldown - bonusTime;
        }
        else
        {
            player.killTimer = PlayerControl.GameOptions.KillCooldown + penaltyTime;
            trapperSetTrapButton.Timer = cooldown + penaltyTime;
        }
        // キル後に罠を解除する
        if (trappedPlayer != null)
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DisableTrap, Hazel.SendOption.Reliable, -1);
            writer.Write(player.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.disableTrap(player.PlayerId);
        }
        else
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ClearTrap, Hazel.SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.clearTrap();
        }
    }
    public override void OnDeath(PlayerControl killer = null) { }
    public override void HandleDisconnect(PlayerControl player, DisconnectReasons reason) { }

    public static CustomButton trapperSetTrapButton;
    public static void MakeButtons(HudManager hm)
    {
        trapperSetTrapButton = new CustomButton(
            () => { // ボタンが押された時に実行
                if (!PlayerControl.LocalPlayer.CanMove && Trapper.trappedPlayer != null) return;
                Trapper.setTrap();
                trapperSetTrapButton.Timer = trapperSetTrapButton.MaxTimer;
            },
            () => { /*ボタン有効になる条件*/
                return PlayerControl.LocalPlayer.isRole(RoleId.Trapper) && !PlayerControl.LocalPlayer.Data.IsDead && Trapper.trappedPlayer == null;
            },
            () => { /*ボタンが使える条件*/
                return PlayerControl.LocalPlayer.CanMove && Trapper.trappedPlayer == null;
            },
            () => { /*ミーティング終了時*/
                trapperSetTrapButton.Timer = trapperSetTrapButton.MaxTimer;
                Trapper.unsetTrap();
            },
            Trapper.getTrapButtonSprite(),
            // new Vector3(-2.6f, 0f, 0f),
            new Vector3(-1.8f, -0.06f, 0f),
            hm,
            hm.AbilityButton,
            KeyCode.F
        );
        trapperSetTrapButton.buttonText = "罠設置";
    }
    public static void SetButtonCooldowns()
    {
        trapperSetTrapButton.MaxTimer = cooldown;
    }

        public static void Clear()
        {
            players = new List<Trapper>();
            place = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.TrapperPlace.wav", false);
            activate = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.TrapperActivate.wav", false);
            disable = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.TrapperDisable.wav", false);
            kill = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.TrapperKill.wav", false);
            countdown = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.TrapperCountdown.wav", false);
            meetingFlag = false;
            placedTime = DateTime.UtcNow;
            playingKillSound = false;
            trappedPlayer = null;
            trap = null;
            unsetTrap();
        }

        public static Sprite getTrapButtonSprite() {
            if (trapButtonSprite) return trapButtonSprite;
            trapButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TrapperTrapButton.png", 115f);
            return trapButtonSprite;
        }
        public static void setTrap(){
            var pos = PlayerControl.LocalPlayer.transform.position;
            byte[] buff = new byte[sizeof(float) * 2];
            Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));
            MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlaceTrap, Hazel.SendOption.Reliable);
            writer.WriteBytesAndSize(buff);
            writer.EndMessage();
            RPCProcedure.placeTrap(buff);
            placedTime = DateTime.UtcNow;
        }
        public static void unsetTrap()
        {
            if(Trapper.trap != null)
            {
                Trapper.trap.SetActive(false);
                GameObject.Destroy(Trapper.trap);
            }
            Trapper.trap = null;
            Trapper.trappedPlayer = null;
        }

        private static Sprite trapeffectSprite;
        public static Sprite getTrapEffectSprite() {
            if (trapeffectSprite) return trapeffectSprite;
            trapeffectSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TrapEffect.png", 300f);
            return trapeffectSprite;
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdReportDeadBody))]
        class PlayerControlCmdReportDeadBodyPatch
        {
            public static void Prefix(PlayerControl __instance)
            {
                // トラップ中にミーティングが来たら直後に死亡する
                if(trappedPlayer != null){
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.TrapperMeetingFlag, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.trapperMeetingFlag();
                }
            }
        }
        
    }
}