using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TheOtherRoles.Objects;
using TheOtherRoles.Patches;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.GameHistory;

namespace TheOtherRoles
{
    [HarmonyPatch]
    public class CreatedMadmate : ModifierBase<CreatedMadmate>
    {
        public static Color color = Palette.ImpostorRed;

        public enum CreatedMadmateType
        {
            Simple = 0,
            WithRole = 1,
            Random = 2,
        }

        public enum CreatedMadmateAbility
        {
            None = 0,
            Fanatic = 1,
        }

        public static bool canEnterVents { get { return CustomOptionHolder.createdMadmateCanEnterVents.getBool(); } }
        public static bool hasImpostorVision { get { return CustomOptionHolder.createdMadmateHasImpostorVision.getBool(); } }
        public static bool canSabotage { get { return CustomOptionHolder.createdMadmateCanSabotage.getBool(); } }
        public static bool canFixComm { get { return CustomOptionHolder.createdMadmateCanFixComm.getBool(); } }

        public static CreatedMadmateType madmateType { get { return CreatedMadmateType.Simple; } }
        public static CreatedMadmateAbility madmateAbility { get { return (CreatedMadmateAbility)CustomOptionHolder.madmateAbility.getSelection(); } }

        public static int numTasks { get { return (int)CustomOptionHolder.createdMadmateNumTasks.getFloat(); } }

        public static bool hasTasks { get { return madmateAbility == CreatedMadmateAbility.Fanatic; } }
        public static bool exileCrewmate { get {return CustomOptionHolder.createdMadmateExileCrewmate.getBool(); } }

        public static string prefix
        {
            get
            {
                return ModTranslation.getString("madmatePrefix");
            }
        }

        public static string fullName
        {
            get
            {
                return ModTranslation.getString("madmate");
            }
        }

        public static List<RoleType> validRoles = new List<RoleType>
        {
            RoleType.Crewmate,
            RoleType.Shifter,
            RoleType.Mayor,
            RoleType.Engineer,
            RoleType.Sheriff,
            RoleType.Lighter,
            RoleType.Detective,
            RoleType.TimeMaster,
            RoleType.Medic,
            RoleType.Swapper,
            RoleType.Seer,
            RoleType.Hacker,
            RoleType.Tracker,
            RoleType.SecurityGuard,
            RoleType.Bait,
            RoleType.Medium,
            RoleType.FortuneTeller,
            RoleType.Mini,
            RoleType.NiceGuesser,
            RoleType.Watcher,
        };

        public static List<PlayerControl> candidates
        {
            get
            {
                List<PlayerControl> crewHasRole = new List<PlayerControl>();
                List<PlayerControl> crewNoRole = new List<PlayerControl>();
                List<PlayerControl> validCrewmates = new List<PlayerControl>();

                foreach (var player in PlayerControl.AllPlayerControls.ToArray().Where(x => x.isCrew() && !hasModifier(x)).ToList())
                {
                    var info = RoleInfo.getRoleInfoForPlayer(player);
                    if (info.Contains(RoleInfo.crewmate))
                    {
                        crewNoRole.Add(player);
                    }
                    else if (info.Any(x => validRoles.Contains(x.roleType)))
                    {
                        crewHasRole.Add(player);
                    }
                    validCrewmates.Add(player);
                }

                if (madmateType == CreatedMadmateType.Simple) return crewNoRole;
                else if (madmateType == CreatedMadmateType.WithRole && crewHasRole.Count > 0) return crewHasRole;
                else if (madmateType == CreatedMadmateType.Random) return validCrewmates;
                return validCrewmates;
            }
        }

        public CreatedMadmate()
        {
            ModType = modId = ModifierType.CreatedMadmate;
        }

        public override void OnMeetingStart() { }
        public override void OnMeetingEnd() { }
        public override void FixedUpdate() { }
        public override void OnKill(PlayerControl target) { }

        public override void OnDeath(PlayerControl killer = null)
        {
            player.clearAllTasks();
        }

        public override void HandleDisconnect(PlayerControl player, DisconnectReasons reason) { }

        public static void MakeButtons(HudManager hm) { }
        public static void SetButtonCooldowns() { }

        public void assignTasks()
        {
            player.generateAndAssignTasks(0, numTasks, 0);
        }
        public static bool knowsImpostors(PlayerControl player)
        {
            return hasTasks && hasModifier(player) && tasksComplete(player);
        }

        public static bool tasksComplete(PlayerControl player)
        {
            if (!hasTasks) return false;

            int counter = 0;
            int totalTasks = numTasks;
            if (totalTasks == 0) return true;
            foreach (var task in player.Data.Tasks)
            {
                if (task.Complete)
                {
                    counter++;
                }
            }
            return counter == totalTasks;
        }

        public static void Clear()
        {
            players = new List<CreatedMadmate>();
        }
    }
}