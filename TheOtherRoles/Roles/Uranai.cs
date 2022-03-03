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
        public static float duration {get { return CustomOptionHolder.uranaiDuration.getFloat();}}
        public static float distance {get { return CustomOptionHolder.uranaiDistance.getFloat();}}

        public static Dictionary<byte, float> progress = new Dictionary<byte, float>();
        public static bool impostorArrowFlag = false;
        public static bool meetingFlag = true;
        public static Dictionary<byte, bool> playerStatus = new Dictionary<byte, bool>();
        public static bool endGameFlag = false;
        public static bool startGameFlag = true;


        public Uranai()
        {
            RoleType = roleId = RoleType.Uranai;
        }

        public override void OnMeetingStart()
        {
            meetingFlag = true;
        }

        public override void OnMeetingEnd()
        {
            HudManager.Instance.StartCoroutine(Effects.Lerp(5.0f, new Action<float>((p) =>
            {
                if (p == 1f)
                {
                    meetingFlag = false;
                }
            })));
            foreach(var p in PlayerControl.AllPlayerControls)
            {
                playerStatus[p.PlayerId] = p.isAlive();
            }
        }

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
        public static bool canDivine(byte index)
        {
            bool status = true;
            if (playerStatus.ContainsKey(index)){
                status = playerStatus[index];
            }
            return Uranai.progress[index] >= Uranai.duration || !status;
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
                    if(PlayerControl.LocalPlayer.CanMove && numUsed < 1 && canDivine(index))
                    {
                        PlayerControl p = Helpers.playerById(index);
                        Uranai.divine(p, resultIsCrewOrNot);
                    }
                };
            };

            Func<bool> uranaiHasButton(byte index)
            {
                return () =>
                {
                    var p = PlayerControl.LocalPlayer;
                    if(!p.isRole(RoleType.Uranai)) return false;
                    bool isDummy = (Puppeteer.dummy != null && Puppeteer.dummy.PlayerId == index);
                    if (p.CanMove && p.isAlive() && p.PlayerId != index && !isDummy
                        && MapOptions.playerIcons.ContainsKey(index) && isCompletedNumTasks(p) && numUsed < 1)
                    {
                        return true;
                    } 
                    else
                    {
                        if(MapOptions.playerIcons.ContainsKey(index))
                            MapOptions.playerIcons[index].gameObject.SetActive(false);
                        if(uranaiButtons.Count > index)
                            uranaiButtons[index].setActive(false);

                        return false;
                    }
                };
            }

            void setButtonPos(byte index)
            {
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
            }
            void setIconPos(byte index, bool transparent)
            {
                MapOptions.playerIcons[index].transform.localScale = Vector3.one * 0.25f;
                MapOptions.playerIcons[index].gameObject.SetActive(PlayerControl.LocalPlayer.CanMove);
                MapOptions.playerIcons[index].setSemiTransparent(transparent);
            }
            Func<bool> uranaiCouldUse(byte index)
            {
                return () =>
                {
                    //　占い師以外の場合、リソースがない場合はボタンを表示しない
                    var p = Helpers.playerById(index);
                    if (!MapOptions.playerIcons.ContainsKey(index) ||
                        !PlayerControl.LocalPlayer.isRole(RoleType.Uranai)) 
                    {
                        return false;
                    }

                    // ボタンの位置を変更
                    setButtonPos(index);


                    // ボタンにテキストを設定
                    bool status = true;
                    if (playerStatus.ContainsKey(index)){
                        status = playerStatus[index];
                    }
                    if(status)
                    {
                        uranaiButtons[index].buttonText = $"{Uranai.progress[index]:.0}/{Uranai.duration}";
                    }
                    else
                    {
                        uranaiButtons[index].buttonText = ModTranslation.getString("fortuneTeller2ndDead");
                    }

                    // アイコンの位置と透明度を変更
                    setIconPos(index, !canDivine(index));
                    
                    return PlayerControl.LocalPlayer.CanMove && numUsed < 1 && canDivine(index);
                };
            }


            for (byte i = 0; i < 15; i++)
            {
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
            if(meetingFlag || startGameFlag) return;
            if(!PlayerControl.LocalPlayer.isRole(RoleType.Uranai)) return;

            foreach (PlayerControl p in PlayerControl.AllPlayerControls)
            {
                if(!progress.ContainsKey(p.PlayerId)) progress[p.PlayerId] = 0f;
                if (p.isDead()) continue;
                if (p == Puppeteer.dummy) continue;
                if (p.isRole(RoleType.Puppeteer) && Puppeteer.stealthed) continue;
                var uranai = PlayerControl.LocalPlayer;
                float distance = Vector3.Distance(p.transform.position, uranai.transform.position);
                // 障害物判定
                bool anythingBetween = PhysicsHelpers.AnythingBetween(p.GetTruePosition(), uranai.GetTruePosition(), Constants.ShipAndObjectsMask, false);
                if(!anythingBetween && distance <= Uranai.distance && progress[p.PlayerId] < duration)
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
            else if(!impostorArrowFlag) return;
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
                    if(p.isRole(RoleType.Uranai)){
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
            meetingFlag = true;
            endGameFlag = false;
            startGameFlag = true;
            playerStatus = new Dictionary<byte, bool>();
        }

        public static void divine(PlayerControl p, bool isTwoSelections)
        {
            string msg = "";
            Color color = Color.white;
            if(!isTwoSelections){
                string roleNames = String.Join(" ", RoleInfo.getRoleInfoForPlayer(p).Select(x => Helpers.cs(x.color, x.name)).ToArray());
                // roleNames = Regex.Replace(roleNames, "<[^>]*>", "");
                msg = $"{p.name}" + ModTranslation.getString("fortuneTeller2ndIs") + $"{roleNames}";
            }else{
                string ret;
                if(p.isCrew())
                {
                    ret = ModTranslation.getString("fortuneTeller2ndCrew");
                    color = Color.white;
                }
                else
                {
                    ret = ModTranslation.getString("fortuneTeller2ndNotCrew");
                    color = Palette.ImpostorRed;
                }
                msg = $"{p.name}" + ModTranslation.getString("fortuneTeller2ndIs") + $"{ret}";
            }

            if (!string.IsNullOrWhiteSpace(msg))
            {   
                uranaiMessage(msg, 5f, color);
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
        public static void uranaiMessage(string message, float duration, Color color) {
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
                text.color = color;

                HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) => {
                    if (p == 1f && text != null && text.gameObject != null) {
                        UnityEngine.Object.Destroy(text.gameObject);
                    }
                })));
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
        class IntroCutsceneOnDestroyPatch
        {
            public static void Prefix(IntroCutscene __instance)
            {
                
                HudManager.Instance.StartCoroutine(Effects.Lerp(10.0f, new Action<float>((p) =>
                {
                    if (p == 1f)
                    {
                        startGameFlag = false;
                    }
                })));
            }
        }
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
        public class OnGameEndPatch
        {

            public static void Prefix(AmongUsClient __instance, [HarmonyArgument(0)] ref EndGameResult endGameResult)
            {
                    Uranai.endGameFlag = true;
            }
        }
    }

}