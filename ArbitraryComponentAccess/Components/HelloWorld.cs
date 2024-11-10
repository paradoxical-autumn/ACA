using FrooxEngine;

namespace ArbitraryComponentAccess.Components;

[Category( "ACA" )]
public class HelloWorld : Component
{
    public readonly Sync<string> HelloWorldText = new Sync<string>();

    protected override void OnAttach()
    {
        base.OnAttach();
        HelloWorldText.Value = "HelloWorld";
    }

    // FrooxEngine Weaver Stuff:
    protected override void InitializeSyncMembers()
    {
        base.InitializeSyncMembers();
    }

    public override ISyncMember GetSyncMember(int index)
    {
        return (ISyncMember)(index switch
        {
            0 => this.persistent, 
            1 => this.updateOrder, 
            2 => this.EnabledField, 
            3 => HelloWorldText, 
            _ => throw new ArgumentOutOfRangeException(), 
        });
    }

    public static HelloWorld __New()
    {
        return new HelloWorld();
    }
}
