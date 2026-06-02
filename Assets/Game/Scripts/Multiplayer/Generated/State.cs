// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 3.0.76
// 

using Colyseus.Schema;
#if UNITY_5_3_OR_NEWER
using UnityEngine.Scripting;
#endif

public partial class State : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve]
#endif
public State() { }
	[Type(0, "map", typeof(MapSchema<Player>))]
	public MapSchema<Player> players = null;

	[Type(1, "boolean")]
	public bool isRingActivated = default(bool);

	[Type(2, "number")]
	public float headPosProgress = default(float);

	[Type(3, "string")]
	public string headType = default(string);

	[Type(4, "array", typeof(ArraySchema<ChatMessage>))]
	public ArraySchema<ChatMessage> chat = null;
}

