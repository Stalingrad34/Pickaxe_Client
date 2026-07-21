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
	public partial class Player : Schema {
#if UNITY_5_3_OR_NEWER
		[Preserve]
#endif
		public Player() { }
		[Type(0, "ref", typeof(Vector3Float))]
		public Vector3Float position = null;

		[Type(1, "number")]
		public float rotationY = default(float);

		[Type(2, "number")]
		public float speed = default(float);

		[Type(3, "string")]
		public string weapon = default(string);

		[Type(4, "string")]
		public string body = default(string);

		[Type(5, "string")]
		public string face = default(string);

		[Type(6, "string")]
		public string name = default(string);
	}
}

