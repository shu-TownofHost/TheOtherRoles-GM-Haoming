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
        public static bool placeTrap;
        public static AudioClip test;

        public static bool meetingFlag;
        

        public Trapper()
        {
            RoleType = roleId = RoleId.NoRole;
        }

        public override void OnMeetingStart()
        {
            meetingFlag = true;
        }
        public override void OnMeetingEnd() 
        {
            unsetTrap();
            meetingFlag = false;
        }

        public override void FixedUpdate() 
        {
            try{
                if (PlayerControl.LocalPlayer.isRole(RoleId.Trapper) && trap != null && trappedPlayer == null)
                {
                    // トラップを踏んだプレイヤーを動けなくする 
                    foreach(var p in PlayerControl.AllPlayerControls)
                    {
                        if(p == PlayerControl.LocalPlayer) continue;
                        var p1 = p.transform.localPosition;
                        Dictionary<GameObject, byte> listActivate = new Dictionary<GameObject, byte>();
                        var p2 = trap.transform.localPosition;
                        var distance = Vector3.Distance(p1, p2);
                        if(distance < 0.5)
                        {
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
                        if (player.PlayerId == trappedPlayer.PlayerId) continue;
                        if (player.isRole(RoleId.Trapper)) continue;
                        Vector3 p2 = player.transform.position;
                        float distance = Vector3.Distance(p1, p2);
                        if(distance < 0.5)
                        {
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DisableTrap, Hazel.SendOption.Reliable, -1);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.disableTrap();
                        }
                    }
                }
            }
            catch (NullReferenceException e){
                Helpers.log(e.Message);
            }
        }
    public override void OnKill(PlayerControl target) { }
    public override void OnDeath(PlayerControl killer = null) { }
    public override void HandleDisconnect(PlayerControl player, DisconnectReasons reason) { }

    public static CustomButton trapperSetTrapButton;
    public static void MakeButtons(HudManager hm)
    {
        trapperSetTrapButton = new CustomButton(
            () => { // ボタンが押された時に実行
                Trapper.setTrap();
                trapperSetTrapButton.Timer = trapperSetTrapButton.MaxTimer;
            },
            () => { /*ボタン有効になる条件*/return PlayerControl.LocalPlayer.isRole(RoleId.Trapper) && !PlayerControl.LocalPlayer.Data.IsDead; },
            () => { /*ボタンが使える条件*/
                return PlayerControl.LocalPlayer.CanMove && Trapper.trap == null;
            },
            () => { /*ミーティング終了時*/
                trapperSetTrapButton.Timer = trapperSetTrapButton.MaxTimer;
                Trapper.unsetTrap();
                TrapEffect.clearTrapEffects();
            },
            Trapper.getTrapButtonSprite(),
            new Vector3(-2.6f, 0f, 0f),
            hm,
            hm.AbilityButton,
            KeyCode.F
        );
    }
    public static void SetButtonCooldowns()
    {
        trapperSetTrapButton.MaxTimer = 5f;
    }

        public static void Clear()
        {
            players = new List<Trapper>();
            test = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.test.wav", false);
            meetingFlag = false;
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
        }
        public static void unsetTrap(){
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
        
    }
}