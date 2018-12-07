using UnityEngine;

namespace MetaWearRPC.Unity
{
	public sealed class MetaWearUnityRPC_Test : MetaWearUnityRPC
	{
		public string testBoardMac = "F6:E9:DD:B4:CF:4A";
		private ulong _testBoardMac;

		public ushort vibrationDurationMs = 1;
		public float vibrationIntensity = 100.0f;

		/// <summary>
		/// The list of reachable MetaWear boards.
		/// </summary>
		//public List<string> metaWearBoards = new List<string>
		//{
		//	"F6:E9:DD:B4:CF:4A",
		//	"D2:80:93:BC:8C:FD",
		//	"DF:16:4D:D1:5D:58",
		//	"C2:48:ED:96:3B:74"
		//};

		protected override void Awake()
		{
			base.Awake();

			_testBoardMac = Global.MacFromString(testBoardMac);
			Debug.Log("[MetaWearUnityRPC_Test] Board Mac address : " + _testBoardMac);
		}

		protected override void Update()
		{
			base.Update();

			if(Input.GetKeyUp(KeyCode.Keypad0))
			{
				Client.CloseBoard(_testBoardMac);
				Debug.Log("[MetaWearUnityRPC_Test] Board " + testBoardMac + " closed.");
			}
			else if(Input.GetKeyUp(KeyCode.Keypad1))
			{
				Client.InitBoard(_testBoardMac);
				Debug.Log("[MetaWearUnityRPC_Test] Board " + testBoardMac + " initialized.");
			}
			else if (Input.GetKeyUp(KeyCode.Keypad2))
			{
				string model = Client.GetBoardModel(_testBoardMac);
				Debug.Log("[MetaWearUnityRPC_Test] Board " + testBoardMac + " model is : " + model);
			}
			else if (Input.GetKeyUp(KeyCode.Keypad3))
			{
				byte batLevel = Client.GetBatteryLevel(_testBoardMac);
				Debug.Log("[MetaWearUnityRPC_Test] Board " + testBoardMac + " battery left : " + batLevel);
			}
			else if(Input.GetKeyUp(KeyCode.Keypad4))
			{
				Client.StartBuzzer(_testBoardMac, vibrationDurationMs);
				Debug.Log("[MetaWearUnityRPC_Test] Board " + testBoardMac + " buzzering for " + vibrationDurationMs + " ms");
			}
			else if (Input.GetKeyUp(KeyCode.Keypad5))
			{
				Client.StartMotor(_testBoardMac, vibrationDurationMs, vibrationIntensity);
				Debug.Log("[MetaWearUnityRPC_Test] Board " + testBoardMac + " vibration for " + vibrationDurationMs + " ms");
			}
		}
	}
}