
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

namespace TheOtherRoles {

    public class AdditionalVents {
            public Vent vent;
	    public static System.Collections.Generic.List<AdditionalVents> AllVents = new System.Collections.Generic.List<AdditionalVents>();
	    public static bool flag = true;
	    public  AdditionalVents(Vector3 p){

		// Create the vent
		var referenceVent = UnityEngine.Object.FindObjectOfType<Vent>();
		vent = UnityEngine.Object.Instantiate<Vent>(referenceVent);
		vent.transform.position = p;
		vent.Left = null;
		vent.Right = null;
		vent.Center = null;
		Vent tmp = ShipStatus.Instance.AllVents[0];
		vent.EnterVentAnim = tmp.EnterVentAnim;
		vent.ExitVentAnim = tmp.ExitVentAnim;
		vent.Offset = new Vector3(0f, 0.25f, 0f);
		vent.Id = ShipStatus.Instance.AllVents.Select(x => x.Id).Max() + 1; // Make sure we have a unique id
		var allVentsList = ShipStatus.Instance.AllVents.ToList();
		allVentsList.Add(vent);
		ShipStatus.Instance.AllVents = allVentsList.ToArray();
		vent.gameObject.SetActive(true);
		vent.name = "AdditionalVent_" + vent.Id;
		AllVents.Add(this);
	    }
	    public static void ClearAndReload(){
                    System.Console.WriteLine("additionalVentsClearAndReload");
		    flag = false;
		    AllVents = new List<AdditionalVents>();
	    }
    }
}