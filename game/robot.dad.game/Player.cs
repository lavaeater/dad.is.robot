namespace robot.dad.game
{
    public class Player : ImageEntity
    {
        public Player(float x, float y, string imagePath) : base (x, y, imagePath)
        {
            
            
        }

        public override void Update()
        {
            base.Update();
            X = Scene.CameraCenterX;
            Y = Scene.CameraCenterY;
        }
    }
}