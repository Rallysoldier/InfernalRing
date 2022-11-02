namespace TeamRitual.Core{
public class GCStateFight : GCState {
    public GCStateFight(GCStateMachine stateMachine, GCStateFactory stateFactory) : base(stateMachine,stateFactory) {
        this.stateMachine = stateMachine;
        this.stateFactory = stateFactory;
    }

    public override void UpdateState() {
        base.UpdateState();

        PlayerGameObj P1 = GameController.Instance.Players[0];
        PlayerGameObj P2 = GameController.Instance.Players[1];
        if (GameController.Instance.remainingTimerTime == 0 || P1.stateMachine.health == 0 || P2.stateMachine.health == 0) {
            this.SwitchState(this.stateFactory.End());
        }
    }
}
}