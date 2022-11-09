using TeamRitual.Character;

namespace TeamRitual.Core{
public class GCStateEnd : GCState {
    public GCStateEnd(GCStateMachine stateMachine, GCStateFactory stateFactory) : base(stateMachine,stateFactory) {
        this.stateMachine = stateMachine;
        this.stateFactory = stateFactory;
    }

    public override void EnterState() {
        GameController.Instance.Pause(40);

        PlayerGameObj P1 = GameController.Instance.Players[0];
        PlayerGameObj P2 = GameController.Instance.Players[1];

        if (P1.stateMachine.health > P2.stateMachine.health) {
            P1.wins++;
        } else if (P1.stateMachine.health < P2.stateMachine.health) {
            P2.wins++;
        }
    }

    public override void UpdateState() {
        base.UpdateState();

        CharacterStateMachine P1 = GameController.Instance.Players[0].stateMachine;
        CharacterStateMachine P2 = GameController.Instance.Players[1].stateMachine;

        //Slo-mo KO
        if (stateTime < 80 && stateTime%2 == 0) {
            GameController.Instance.Pause(1);
        }

        if (stateTime > 160 && (P1.currentState is CommonStateLyingDown || P2.currentState is CommonStateLyingDown)) {
            if (P1.currentState is CommonStateLyingDown ? P1.currentState.stateTime > 20 : P2.currentState.stateTime > 20) {
                this.SwitchState(this.stateFactory.Intro());
            }
        }
    }
}
}