namespace TeamRitual.Core{
public class GCStateEnd : GCState {
    public GCStateEnd(GCStateMachine stateMachine, GCStateFactory stateFactory) : base(stateMachine,stateFactory) {
        this.stateMachine = stateMachine;
        this.stateFactory = stateFactory;
    }

    public override void EnterState() {
        GameController.Instance.Pause(40);
    }

    public override void UpdateState() {
        base.UpdateState();

        //Slo-mo KO
        if (stateTime < 80 && stateTime%2 == 0) {
            GameController.Instance.Pause(1);
        }

        if (stateTime == 80) {
            PlayerGameObj P1 = GameController.Instance.Players[0];
            PlayerGameObj P2 = GameController.Instance.Players[1];

            if (P1.stateMachine.health > P2.stateMachine.health) {
                P1.wins++;
            } else if (P1.stateMachine.health < P2.stateMachine.health) {
                P2.wins++;
            }
        }

        if (stateTime > 160) {
            this.SwitchState(this.stateFactory.Intro());
        }
    }
}
}