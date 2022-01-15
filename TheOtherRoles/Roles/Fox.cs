using HarmonyLib;
using Hazel;
using System;
using System.Linq;
using System.Collections.Generic;
using TheOtherRoles.Objects;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.Patches.PlayerControlFixedUpdatePatch;

namespace TheOtherRoles
{
    [HarmonyPatch]
    public class Fox : RoleBase<Fox>
    {
        public static Color color = new Color32(167, 87, 168, byte.MaxValue);
        private static CustomButton foxButton;
        public static List<Arrow> arrows = new List<Arrow>();
        public static float updateTimer = 0f;
        // public static bool cantKillFox {get { return CustomOptionHolder.foxCantKillFox.getBool();}}
        // public static int cantKillFoxExceptions {get { return CustomOptionHolder.foxCantKillFoxExceptions.getSelection();}}
        // public static bool cantFixSabotage {get { return CustomOptionHolder.foxCantFixSabotage.getBool();}}
        // public static bool canDetectKillers {get { return CustomOptionHolder.foxCanDetectKillers.getBool();}}
        public static float arrowUpdateInterval{get { return CustomOptionHolder.foxArrowUpdateInterval.getFloat();}}
        public static bool crewWinsByTasks {get { return CustomOptionHolder.foxCrewWinsByTasks.getBool();}}
        public static bool mustCompleteTasks {get { return CustomOptionHolder.foxMustCompleteTasks.getBool();}}
        // public static bool hasImpostorVision {get { return CustomOptionHolder.foxHasImpostorVision.getBool();}}
        // public static bool canStealth {get { return CustomOptionHolder.foxCanStealth.getBool();}}
        public static float stealthCooldown {get {return CustomOptionHolder.foxStealthCooldown.getFloat();}}
        public static float stealthDuration {get {return CustomOptionHolder.foxStealthDuration.getFloat();}}
        public static int numCommonTasks {get {return (int)CustomOptionHolder.foxNumCommonTasks.getFloat();}}
        public static int numLongTasks {get {return (int)CustomOptionHolder.foxNumLongTasks.getFloat();}}
        public static int numShortTasks {get {return (int)CustomOptionHolder.foxNumShortTasks.getFloat();}}
        public bool stealthed = false;
        public DateTime stealthedAt = DateTime.UtcNow;
        public static float fadeTime = 1f;


        public Fox()
        {
            stealthed = false;
            stealthedAt = DateTime.UtcNow;
            RoleType = roleId = RoleId.Fox;
        }

        public override void OnMeetingStart() { }

        public override void OnMeetingEnd() { }
        public override void OnKill(PlayerControl target) { }
        public override void HandleDisconnect(PlayerControl player, DisconnectReasons reason) { }
        public override void OnDeath(PlayerControl killer = null) { }

        public override void FixedUpdate() 
        {
            if(PlayerControl.LocalPlayer.isRole(RoleId.Fox))
            {
                arrowUpdate();
            }
        }
        public static void Clear()
        {
            players = new List<Fox>();
            arrows = new List<Arrow>();
        }

        private static Sprite buttonSprite;
         public static Sprite getButtonSprite()
        {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.NinjaButton.png", 115f);
            return buttonSprite;
        }
            public static float stealthFade(PlayerControl player)
        {
            if (isRole(player) && fadeTime > 0f && player.isAlive())
            {
                Fox n = players.First(x => x.player == player);
                return Mathf.Min(1.0f, (float)(DateTime.UtcNow - n.stealthedAt).TotalSeconds / fadeTime);
            }
            return 1.0f;
        }

        public static bool isStealthed(PlayerControl player)
        {
            if (isRole(player) && player.isAlive())
            {
                Fox n = players.First(x => x.player == player);
                return n.stealthed;
            }
            return false;
        }

        public static void setStealthed(PlayerControl player, bool stealthed = true)
        {
            if (isRole(player))
            {
                Fox n = players.First(x => x.player == player);
                n.stealthed = stealthed;
                n.stealthedAt = DateTime.UtcNow;
            }
        }

        public static void MakeButtons(HudManager hm)
        {
            // Fox stealth
            foxButton = new CustomButton(
                () => {
                    if (foxButton.isEffectActive)
                    {
                        foxButton.Timer = 0;
                        return;
                    }

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.FoxStealth, Hazel.SendOption.Reliable, -1);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer.Write(true);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.foxStealth(PlayerControl.LocalPlayer.PlayerId, true);
                },
                () => { return PlayerControl.LocalPlayer.isRole(RoleId.Fox) && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (foxButton.isEffectActive)
                    {
                        foxButton.buttonText = ModTranslation.getString("NinjaUnstealthText");
                    }
                    else
                    {
                        foxButton.buttonText = ModTranslation.getString("NinjaText");
                    }
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () => {
                    foxButton.Timer = foxButton.MaxTimer = Fox.stealthCooldown;
                },
                Fox.getButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                hm,
                hm.KillButton,
                KeyCode.F,
                true,
                Fox.stealthDuration,
                () => {
                    foxButton.Timer = foxButton.MaxTimer = Fox.stealthCooldown;
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.FoxStealth, Hazel.SendOption.Reliable, -1);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer.Write(false);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.foxStealth(PlayerControl.LocalPlayer.PlayerId, false);
                }
            );
            foxButton.buttonText = ModTranslation.getString("NinjaText");
            foxButton.effectCancellable = true;
        }

        static void arrowUpdate(){

            // 前フレームからの経過時間をマイナスする
            updateTimer -= Time.fixedDeltaTime;

            // 1秒経過したらArrowを更新
            if(updateTimer <= 0.0f){

                // 前回のArrowをすべて破棄する
                foreach(Arrow arrow in arrows){
                    arrow.arrow.SetActive(false);
                    UnityEngine.Object.Destroy(arrow.arrow);
                }

                // Arrorw一覧
                arrows = new List<Arrow>();

                // インポスターの位置を示すArrorwを描画
                foreach(PlayerControl p in PlayerControl.AllPlayerControls){
                    if(p.Data.IsDead) continue;
                    Arrow arrow;
                    // float distance = Vector2.Distance(p.transform.position, PlayerControl.LocalPlayer.transform.position);
                    if(p.Data.Role.IsImpostor || p.isRole(RoleId.Jackal) || p.isRole(RoleId.Sheriff)){
                        if(p.Data.Role.IsImpostor){
                            arrow = new Arrow(Color.red);
                        }
                        else if(p.isRole(RoleId.Jackal)){
                            arrow = new Arrow(Jackal.color);
                        }else if(p.isRole(RoleId.Sheriff)){
                            arrow = new Arrow(Sheriff.color);
                        }else{
                            arrow = new Arrow(Color.black);
                        }
                        arrow.arrow.SetActive(true);
                        arrow.Update(p.transform.position);
                        arrows.Add(arrow);
                    }
                }

                // タイマーに時間をセット
                updateTimer = arrowUpdateInterval;
            }
        }

        public static bool isCompletedTasks(PlayerControl p)
        {
            int counter = 0;
            int totalTasks = numCommonTasks + numLongTasks + numShortTasks;
            foreach(var task in p.Data.Tasks){

                if(task.Complete){
                    counter++;
                }
            }
            return  counter == totalTasks;
        }
         public static void SetFoxTasks()
        {
            PlayerControl me = PlayerControl.LocalPlayer;
            if (me == null)
                return;
            GameData.PlayerInfo playerById = GameData.Instance.GetPlayerById(me.PlayerId);
            if (playerById == null)
                return;
            me.clearAllTasks();
            List<byte> list = new List<byte>(10);
            SetTasksToList(
                ref list,
                ShipStatus.Instance.CommonTasks.ToList<NormalPlayerTask>(),
                Mathf.RoundToInt(numCommonTasks));
            SetTasksToList(
                ref list,
                ShipStatus.Instance.LongTasks.ToList<NormalPlayerTask>(),
                Mathf.RoundToInt(numLongTasks));
            SetTasksToList(
                ref list,
                ShipStatus.Instance.NormalTasks.ToList<NormalPlayerTask>(),
                Mathf.RoundToInt(numShortTasks));

            byte[] taskTypeIds = list.ToArray();
            playerById.Tasks = new Il2CppSystem.Collections.Generic.List<GameData.TaskInfo>(taskTypeIds.Length);
            for (int i = 0; i < taskTypeIds.Length; i++) {
                playerById.Tasks.Add(new GameData.TaskInfo(taskTypeIds[i], (uint)i));
                playerById.Tasks[i].Id = (uint)i;
            }
            for (int i = 0; i < playerById.Tasks.Count; i++) {
                GameData.TaskInfo taskInfo = playerById.Tasks[i];
                NormalPlayerTask normalPlayerTask = UnityEngine.Object.Instantiate<NormalPlayerTask>(ShipStatus.Instance.GetTaskById(taskInfo.TypeId), me.transform);
                normalPlayerTask.Id = taskInfo.Id;
                normalPlayerTask.Owner = me;
                normalPlayerTask.Initialize();
                me.myTasks.Add(normalPlayerTask);
            }
        }

        private static void SetTasksToList(
            ref List<byte> list,
            List<NormalPlayerTask> playerTasks,
            int numConfiguredTasks)
        {
            playerTasks.shuffle(0);
            int numTasks = Math.Min(playerTasks.Count, numConfiguredTasks);
            for (int i = 0; i < numTasks; i++) {
                list.Add((byte)playerTasks[i].Index);
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch {
            public static void Postfix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                if (PlayerControl.LocalPlayer.isRole(RoleId.Fox))
                {
                    SetFoxTasks();
                }
            }
        }


        // 狐のタスクをカウントから外す
        [HarmonyPatch(typeof(GameData), nameof(GameData.RecomputeTaskCounts))]
        class GameDataRecomputeTaskCountsPatch
        {
            static void Postfix(GameData __instance)
            {
                for (int i = 0;  i < __instance.AllPlayers.Count; i++)
                {
                    GameData.PlayerInfo playerInfo = __instance.AllPlayers[i];
                    PlayerControl player = Helpers.playerById(playerInfo.PlayerId);
                    if (player.isRole(RoleId.Fox))
                    {
                        if (!playerInfo.Disconnected && playerInfo.Tasks != null && playerInfo.Object && (PlayerControl.GameOptions.GhostsDoTasks || !playerInfo.IsDead) && playerInfo.Role && playerInfo.Role.TasksCountTowardProgress)
                        {
                            for (int j = 0; j < playerInfo.Tasks.Count; j++)
                            {
                                __instance.TotalTasks--;
                                if (playerInfo.Tasks[j].Complete)
                                {
                                    __instance.CompletedTasks--;
                                }
                            }
                        }
                    }
                }
            }
        }
        // 狐のタスクをカウントから外す
        [HarmonyPatch(typeof(GameData), nameof(GameData.CompleteTask))]
        class GameDataCompleteTaskPatch
        {
            static void Postfix(GameData __instance, PlayerControl pc, uint taskId)
            {
                if (pc.isRole(RoleId.Fox))
                {
                    __instance.CompletedTasks--;
                }

            }
        }

        // 透明化
        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
        public static class PlayerPhysicsFoxPatch
        {
            public static void Postfix(PlayerPhysics __instance)
            {
                if (isRole(__instance.myPlayer))
                {
                    var fox = __instance.myPlayer;
                    if (fox == null) return;

                    var opacity = 0.0f;

                    if (isStealthed(fox))
                    {
                        opacity = Math.Max(opacity, 1.0f - stealthFade(fox));
                        fox.myRend.material.SetFloat("_Outline", 0f);
                    }
                    else
                    {
                        opacity = Math.Max(opacity, stealthFade(fox));
                    }

                    // Sometimes it just doesn't work?
                    var color = Color.Lerp(Palette.ClearWhite, Palette.White, opacity);
                    try
                    {
                        if (fox.MyPhysics?.rend != null)
                            fox.MyPhysics.rend.color = color;

                        if (fox.MyPhysics?.Skin?.layer != null)
                            fox.MyPhysics.Skin.layer.color = color;

                        if (fox.HatRenderer != null)
                            fox.HatRenderer.color = color;

                        if (fox.CurrentPet?.rend != null)
                            fox.CurrentPet.rend.color = color;

                        if (fox.CurrentPet?.shadowRend != null)
                            fox.CurrentPet.shadowRend.color = color;

                        if (fox.VisorSlot != null)
                            fox.VisorSlot.color = color;
                    } catch { }
                }
            }
        }
    }
    
}