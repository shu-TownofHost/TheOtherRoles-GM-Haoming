
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
    }
}