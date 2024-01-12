using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared.Radio.Components;

/// <summary>
/// Handles handheld radio ui and is authoritative on the channels a radio can access.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class HandheldRadioComponent : Component
{
    /// <summary>
    /// Does this radio require power to function
    /// </summary>
    [DataField("requiresPower"), ViewVariables(VVAccess.ReadWrite)]
    public bool RequiresPower = false;

    /// <summary>
    /// The list of radio channel prototypes this radio can choose between.
    /// </summary>
    [DataField("supportedChannels", customTypeSerializer: typeof(PrototypeIdListSerializer<RadioChannelPrototype>))]
    public List<string> SupportedChannels = new();
}