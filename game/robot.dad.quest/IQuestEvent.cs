namespace robot.dad.quest
{
    //The context for the state machine and quest
    /* Can a quest be abandoned? - Yes
     * 
     * So, we have a UI that can be acted on. Any action
     * will act upon... the state machine? A controller
     * 
     * We shall have a controller. This controller holds 
     * the context, the UI and the state machine
     * 
     * The controller "is" the scene for now.
     * 
     * When an action is taken in the UI, teh controller fires the necessary
     * event on the state machine. This will affect the context
     * which is the basis for the UI.
     */

    public interface IQuestEvent
    {

    }
}
