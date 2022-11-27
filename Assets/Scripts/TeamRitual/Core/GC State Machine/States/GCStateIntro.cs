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

        PlayerGameObj P1 = GameController.Instance.Players[0];
        PlayerGameObj P2 = GameController.Instance.Players[1];
        if (stateTime > 80 && P1.paletteSelected && P2.paletteSelected && P1.modeSelected && P2.modeSelected) {
            this.SwitchState(this.stateFactory.Countdown());
        }
    }
}
}