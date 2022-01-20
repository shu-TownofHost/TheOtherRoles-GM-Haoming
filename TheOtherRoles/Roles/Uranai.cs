using HarmonyLib;
using Hazel;
using System;
using System.Linq;
using System.Collections.Generic;
using TheOtherRoles.Objects;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.Patches.PlayerControlFixedUpdatePatch;
using System.Text.RegularExpressions;

namespace TheOtherRoles
{
    [HarmonyPatch]
    public class Uranai : RoleBase<Uranai>
    {
        public static Color color = Color.cyan;
        public static int numUsed = 0;
        public static int numTasks {get { return (int)CustomOptionHolder.uranaiNumTasks.getFloat();}}
        public static bool resultIsCrewOrNot {get { return CustomOptionHolder.uranaiResultIsCrewOrNot.getBool();}}

        public static Dictionary<byte, float> progress = new Dictionary<byte, float>();
        public static float progressMax = 20f;
        public static bool impostorArrowFlag = false;
        public static bool meetingFlag = false;
        private static Sprite targetSprite;


        public Uranai()
        {
            RoleType = roleId = RoleId.Uranai;
        }

        public override void OnMeetingStart() { }

        public override void OnMeetingEnd() { }
        public override void OnKill(PlayerControl target) { }
        public override void HandleDisconnect(PlayerControl player, DisconnectReasons reason) { }
        public override void OnDeath(PlayerControl killer = null) { }

        public override void FixedUpdate()
        {
            uranaiUpdate();
            impostorArrowUpdate();
        }

        public static bool isCompletedNumTasks(PlayerControl p)
        {
            var (tasksCompleted, tasksTotal) = TasksHandler.taskInfo(p.Data);
            return  tasksCompleted >= numTasks;
        }

        public static List<CustomButton> uranaiButtons;
        public static void MakeButtons(HudManager hm)
        {
            uranaiButtons = new List<CustomButton>();

            Vector3 uranaiCalcPos(byte index)
            {
                //return new Vector3(-0.25f, -0.25f, 0) + Vector3.right * index * 0.55f;
                return new Vector3(-0.25f, -0.15f, 0) + Vector3.right * index * 0.55f;
            }

            Action uranaiButtonOnClick(byte index)
            {
                return () =>
                {
                    PlayerControl p = Helpers.playerById(index);
                    Uranai.divine(p);
                };
            };

            Func<bool> uranaiHasButton(byte index)
            {
                return () =>
                {
                    if (!isCompletedNumTasks(PlayerControl.LocalPlayer)) return false;
                    else if (!MapOptions.playerIcons.ContainsKey(index)) return false;
                    else if (PlayerControl.LocalPlayer.isRole(RoleId.Uranai) && PlayerControl.LocalPlayer.CanMove) return true;
                    return false;
                };
            }


            Func<bool> uranaiCouldUse(byte index)
            {
                return () =>
                {
                    if (!MapOptions.playerIcons.ContainsKey(index)) return false;
                    Vector3 pos = uranaiCalcPos(index);
                    Vector3 scale = new Vector3(0.4f, 0.8f, 1.0f);

                    Vector3 iconBase = hm.UseButton.transform.localPosition;
                    iconBase.x *= -1;
                    if (uranaiButtons[index].PositionOffset != pos)
                    {
                        uranaiButtons[index].PositionOffset = pos;
                        uranaiButtons[index].LocalScale = scale;
                        MapOptions.playerIcons[index].transform.localPosition = iconBase + pos;
                    }

                    MapOptions.playerIcons[index].transform.localScale = Vector3.one * 0.25f;
                    MapOptions.playerIcons[index].gameObject.SetActive(isCompletedNumTasks(PlayerControl.LocalPlayer)&&PlayerControl.LocalPlayer.CanMove);
                    MapOptions.playerIcons[index].setSemiTransparent(Uranai.progress[index] <= Uranai.progressMax);
                    uranaiButtons[index].buttonText = $"{Uranai.progress[index]:.0}/{Uranai.progressMax}";
                    return PlayerControl.LocalPlayer.CanMove;
                };
            }


            for (byte i = 0; i < 15; i++)
            {
                //TheOtherRolesPlugin.Instance.Log.LogInfo($"Added {i}");
                // if(i >= PlayerControl.AllPlayerControls.Count) break;

                CustomButton uranaiButton = new CustomButton(
                    // Action OnClick
                    uranaiButtonOnClick(i),
                    // bool HasButton
                    uranaiHasButton(i),
                    // bool CouldUse
                    uranaiCouldUse(i),
                    // Action OnMeetingEnds
                    () => { },
                    // sprite
                    null,
                    // position
                    Vector3.zero,
                    // hudmanager
                    hm,
                    // keyboard shortcut
                    null,
                    KeyCode.None,
                    true
                );
                uranaiButton.Timer = 0.0f;
                uranaiButton.MaxTimer = 0.0f;

                uranaiButtons.Add(uranaiButton);
            }

        }

        private static void uranaiUpdate()
        {
            if(!PlayerControl.LocalPlayer.isRole(RoleId.Uranai)) return;

            foreach (PlayerControl p in PlayerControl.AllPlayerControls)
            {
                if (p.isDead()) continue;
                var uranai = PlayerControl.LocalPlayer;
                if(!progress.ContainsKey(p.PlayerId)) progress[p.PlayerId] = 0f;
                float distance = Vector3.Distance(p.transform.position, uranai.transform.position);
                // 障害物判定
                bool anythingBetween = PhysicsHelpers.AnythingBetween(p.GetTruePosition(), uranai.GetTruePosition(), Constants.ShipAndObjectsMask, false);
                if(!anythingBetween && distance <= 1 && progress[p.PlayerId] < progressMax)
                {
                    progress[p.PlayerId] += Time.fixedDeltaTime;
                }
            }
        }

        public static List<Arrow> arrows = new List<Arrow>();
        public static float updateTimer = 0f;
        public static void impostorArrowUpdate()
        {
            if(!PlayerControl.LocalPlayer.isImpostor()) return;
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

                // 占い師の位置を示すArrorwを描画
                foreach(PlayerControl p in PlayerControl.AllPlayerControls){
                    if(p.Data.IsDead) continue;
                    Arrow arrow;
                    // float distance = Vector2.Distance(p.transform.position, PlayerControl.LocalPlayer.transform.position);
                    if(p.isRole(RoleId.Uranai)){
                        arrow = new Arrow(Uranai.color);
                        arrow.arrow.SetActive(true);
                        arrow.Update(p.transform.position);
                        arrows.Add(arrow);
                    }
                }

                // タイマーに時間をセット
                updateTimer = 0.5f;
            }
            
        }
        public static void Clear()
        {
            players = new List<Uranai>();
            progress = new Dictionary<byte,float>();
            arrows = new List<Arrow>();
            impostorArrowFlag = false;
            numUsed = 0;
        }

        public static void divine(PlayerControl p)
        {
            if(progress[p.PlayerId] < progressMax) return;
            PlayerControl fortuneTeller = PlayerControl.LocalPlayer;
            var (tasksCompleted, tasksTotal) = TasksHandler.taskInfo(fortuneTeller.Data);
            int divineNum = ((int)tasksCompleted - (numTasks*numUsed))/numTasks;
            if(divineNum <= 0) return;
            string msg = "";
            if(!resultIsCrewOrNot){
                string roleNames = String.Join(" ", RoleInfo.getRoleInfoForPlayer(p).Select(x => Helpers.cs(x.color, x.name)).ToArray());
                roleNames = Regex.Replace(roleNames, "<[^>]*>", "");
                msg = $"{p.name}は{roleNames}";
            }else{
                string ret = p.isCrew() ? "クルー" : "クルー以外";
                msg = $"{p.name}は{ret}";
            }

            if (!string.IsNullOrWhiteSpace(msg))
            {   
                uranaiMessage(msg, 5f);
                
            }
            if(Constants.ShouldPlaySfx()) SoundManager.Instance.PlaySound(DestroyableSingleton<HudManager>.Instance.TaskCompleteSound, false, 0.8f);
            numUsed += 1;

            // 占いを実行したことで発火される処理を他クライアントに通知
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.FortuneTellerUsedDivine, Hazel.SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(p.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.fortuneTellerUsedDivine(PlayerControl.LocalPlayer.PlayerId, p.PlayerId);
        }


        private static TMPro.TMP_Text text;
        public static void uranaiMessage(string message, float duration) {
            RoomTracker roomTracker =  HudManager.Instance?.roomTracker;
            if (roomTracker != null) {
                GameObject gameObject = UnityEngine.Object.Instantiate(roomTracker.gameObject);
                
                gameObject.transform.SetParent(HudManager.Instance.transform);
                UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<RoomTracker>());
                text = gameObject.GetComponent<TMPro.TMP_Text>();
                text.text = ModTranslation.getString(message);

                // Use local position to place it in the player's view instead of the world location
                gameObject.transform.localPosition = new Vector3(0, -1.8f, gameObject.transform.localPosition.z);
                text.text = message;
                text.color = Color.white;

                HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) => {
                    if (p == 1f && text != null && text.gameObject != null) {
                        UnityEngine.Object.Destroy(text.gameObject);
                    }
                })));
            }
        }
    }

}