/******************************************************************************************************************************************************
* MIT License																																		  *
*																																					  *
* Copyright (c) 2020																																  *
* Emmanuel Badier <emmanuel.badier@gmail.com>																										  *
* 																																					  *
* Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),  *
* to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,  *
* and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:          *
* 																																					  *
* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.					  *
* 																																					  *
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, *
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 																							  *
* IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 		  *
* TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.							  *
******************************************************************************************************************************************************/

using UnityEngine;

namespace MetaWearRPC.Unity
{
	public sealed class MetaWearUnityRPC_Test : MetaWearUnityRPC
	{
		/// <summary>
		/// The board that currently receive commands.
		/// </summary>
		[Range(0, 3)]
		public int currentBoardIndex = 0;
		[Range(0.0f, 100.0f)]
		public float vibrationIntensity = 100.0f;
		public ushort vibrationBuzzDurationMs = 200;
		public ushort vibrationPatternSleepDurationMs = 110;
		public int vibrationPatternIterations = 5;

		protected override void Update()
		{
			base.Update();

			string boardStr = _boardsMac[currentBoardIndex];
			ulong board = _boards[currentBoardIndex];

			if (Input.GetKeyUp(KeyCode.Keypad0))
			{
				string model = Client.GetBoardModel(board);
				Debug.Log("[MetaWearUnityRPC_Test] Board " + boardStr + " model is : " + model);
			}
			else if (Input.GetKeyUp(KeyCode.Keypad1))
			{
				byte batLevel = Client.GetBatteryLevel(board);
				Debug.Log("[MetaWearUnityRPC_Test] Board " + boardStr + " battery left : " + batLevel);
			}
			else if (Input.GetKeyUp(KeyCode.Keypad2))
			{
				Client.StartBuzzer(board, vibrationBuzzDurationMs);
				Debug.Log("[MetaWearUnityRPC_Test] Board " + boardStr + " buzzering " + vibrationBuzzDurationMs + " ms");
			}
			else if (Input.GetKeyUp(KeyCode.Keypad4))
			{
				Client.StartMotor(board, vibrationBuzzDurationMs, vibrationIntensity);
				Debug.Log("[MetaWearUnityRPC_Test] Board " + boardStr + " vibrating " + vibrationBuzzDurationMs + " ms");
			}
			else if (Input.GetKeyUp(KeyCode.Keypad5))
			{
				Client.StartMotorPattern(board, vibrationBuzzDurationMs, vibrationIntensity, vibrationPatternSleepDurationMs, vibrationPatternIterations);
				Debug.Log("[MetaWearUnityRPC_Test] Board " + boardStr + " vibrating " + vibrationPatternIterations + " times");
			}

		}
	}
}