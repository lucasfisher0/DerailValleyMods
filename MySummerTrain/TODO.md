

Get player game object: 
    CustomFPSControllerNonVR(Clone)  -- or a clone of the VR variant



Manual:
    GameObject "Navigation tree"
        Children: Viewport -> Navigation -> Manual Hierarchy Button



CustomFPSControllerNonVR(Clone)

BedSleepingController


DV.Customization.WorldCustomization
WorldStreamingInit.LoadingFinishing += LoadingFinished;
https://dv-mapify.readthedocs.io/en/latest/usage/#changing-maps
https://github.com/Insprill/dv-mapify/releases

this.playerScripts.Add(PlayerManager.PlayerTransform.GetComponent<CameraSmoothing>());



Sample Item:
    UnityEngine.Transform
    InventoryItemSpec
    UnityEngine.LODGroup
    DV.CabControls.Spec.Item
    DV.Items.FovBasedNonVRGrabAnchor
    DV.CabControls.NonVR.ItemNonVR
    ItemBuoyancy
    UnityEngine.RigidBody
    CabItemRigidBody
    RespawnOnDrop
    CollisionSound
    NonAABBReflectionProbeSampler
    DV.Utils.DVConvertToEntity
    DV.Interaction.GrabHandlerItem
    DV.CabControls.NonVR.ItemReparentingNonVR

Hook into UI:
	DV.UI.GameUISetup
		private IEnumerator Start(); 	This method sets an child to active: vr/nonVR.
		Component: DV.InteractionText	Could add more interaction texts here by patching component
		Child: [NewCanvasController] is where the alt hotbar, cursor
			-> [MAIN]/[GameUI]/[NewCanvasController]/crosshair and /TopMount

		Possibly use DV.UIFramework.UIOptimizedEnableDisable?
