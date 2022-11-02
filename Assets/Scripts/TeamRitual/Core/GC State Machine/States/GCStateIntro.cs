namespace TeamRitual.Core{
public class GCStateIntro : GCState {
    public GCStateIntro(GCStateMachine stateMachine, GCStateFactory stateFactory) : base(stateMachine,stateFactory) {
        this.stateMachine = stateMachine;
        this.stateFactory = stateFactory;
    }

    public override void EnterState() {
        base.EnterState();

        GameController.Instance.remainingTimerTime = GameController.Instance.maxTimerTime;
    }

    public override void UpdateState() {
        base.UpdateState();

        if (stateTime > 80) {
            this.SwitchState(this.stateFactory.Countdown());
        }
    }
}
}