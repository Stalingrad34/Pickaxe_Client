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

public partial class RestartInfo : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve]
#endif
public RestartInfo() { }
	[Type(0, "string")]
	public string playerId = default(string);

	[Type(1, "ref", typeof(Player))]
	public Player player = null;
}

