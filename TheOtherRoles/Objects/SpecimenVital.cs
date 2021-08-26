
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

namespace TheOtherRoles {

    public class SpecimenVital {
		public static Vector3 pos = new Vector3(35.39f, -22.10f, 1.0f);
	    public static bool flag = false;
		public static void clearAndReload(){
			System.Console.WriteLine("SpecimenVitalClearAndReload");
			flag = false;
		}

        public static void moveVital(){
            if(SpecimenVital.flag) return;
            if(PlayerControl.GameOptions.MapId == 2 && CustomOptionHolder.polusSpecimenVital.getBool()){
                var panel = GameObject.Find("panel_vitals");
                if(panel != null){
                    TheOtherRolesPlugin.Instance.Log.LogInfo("Move Vital to Specimen");
                    var transform = panel.GetComponent<Transform>();
                    transform.SetPositionAndRotation(SpecimenVital.pos, transform.rotation);
                    SpecimenVital.flag = true;
                }
            }
        }
    }
}