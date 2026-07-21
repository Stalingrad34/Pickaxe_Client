// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 3.0.76
// 

using Colyseus.Schema;
using UnityEngine.Scripting;

namespace Game.Scripts.Multiplayer.Generated
{
	public partial class Vector3Float : Schema {
#if UNITY_5_3_OR_NEWER
		[Preserve]
#endif
		public Vector3Float() { }
		[Type(0, "number")]
		public float x = default(float);

		[Type(1, "number")]
		public float y = default(float);

		[Type(2, "number")]
		public float z = default(float);
	}
}

