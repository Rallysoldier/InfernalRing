namespace TeamRitual.Core{
public class GCStateCountdown : GCState {
    public GCStateCountdown(GCStateMachine stateMachine, GCStateFactory stateFactory) : base(stateMachine,stateFactory) {
        this.stateMachine = stateMachine;
        this.stateFactory = stateFactory;
    }

    public override void UpdateState() {
        base.UpdateState();

        if (stateTime > 20) {
            this.SwitchState(this.stateFactory.Fight());
        }
    }
}
}