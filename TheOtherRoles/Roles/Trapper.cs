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
        public static bool placeTrap;
        public static Vector3 trap;
        public static AudioClip test;

        public static bool meetingFlag;
        public static float baseTrueSpeed = 0.0f;
        

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
            if (PlayerControl.LocalPlayer.isRole(RoleId.Trapper))
            {
                //トラップを踏んだプレイヤーを動けなくする 
                foreach(var p in PlayerControl.AllPlayerControls)
                {
                    if(p == PlayerControl.LocalPlayer) continue;
                    var p1 = p.transform.localPosition;
                    int index = 0;
                    foreach(var trapeffect in TrapEffect.trapeffects)
                    {
                        var p2 = trapeffect.trapeffect.transform.localPosition;
                        var distance = Vector3.Distance(p1, p2);
                        if(distance < 0.5)
                        {
                            

                        }
                        index++;
                    }
                }
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
                    return PlayerControl.LocalPlayer.CanMove;
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
            trap = new Vector3(0, 0, 0);
            test = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.test.wav", false);
            meetingFlag = false;
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
            TrapEffect.clearTrapEffects();
        }
        
    }
}