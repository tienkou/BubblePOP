using System.Collections.Generic;
using Jypeli;
/// <summary>
/// @author Harja "tienkou" Jonne
/// @version 1.0 - gold
/// </summary>
public class BubblePOP : PhysicsGame
{
    private int BUBBLES = 75;

    private Image mapImage = LoadImage("map");  
    private Shape mapShape;

    private List<PhysicsObject> bubbles = new List<PhysicsObject> ();
    private List<Color> bubbleColors = new List<Color> {Color.DarkForestGreen, Color.DarkBlue, Color.DarkMagenta, Color.Gold, Color.Red };

    private IntMeter scoreCounter;
    private Timer timer = new Timer();

    public override void Begin()
    {
        Gravity = new Vector(0.0, -200);
        PlayerMotivator();

        mapShape = Shape.FromImage(mapImage);
        CreateLevel();
        Populate();

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }


    /// <summary>
    /// Takes care everything to do with bubble destruction.
    /// </summary>
    /// <param name="bubble">Bubble that is going to burst and take down it's mates</param>
    private void BubblePop(PhysicsObject bubble)
    {
        bubble.Destroy();
        bubbles.Remove(bubble);
        double distance = 0.0;
        int scoreMultiplier = 1;
        int maxDistance = 60;

        for (int i = 0; i < bubbles.Count; i++)
        {
            distance = Vector.Distance(bubble.Position, bubbles[i].Position);
            if (bubble.Color == bubbles[i].Color && distance < maxDistance) 
            {
                BubblePop(bubbles[i]);
                i--;
                scoreMultiplier++;
            }
        }

        scoreCounter.Value += 1 * scoreMultiplier;
        scoreCounter.UpperLimit += EndGame;
        if (bubbles.Count <= 0) Lost();
    }


    /// <summary>
    /// Sets up the level.
    /// </summary>
    private void CreateLevel()
    {
        PhysicsObject map = PhysicsObject.CreateStaticObject(800, 600, mapShape);
        map.Image = mapImage;
        Add(map);
    }


    /// <summary>
    /// Populates the game with predetermined colors and amounts of bubbles.
    /// </summary>
    private void Populate()
    {
        Vector position = new Vector(0, 400);

        for (int i = 0; i < BUBBLES; i++)
            {
            Timer.SingleShot(0.1+0.1*i,
                delegate { CreateBubble(position); }
                );
            }
    }


    /// <summary>
    /// One bubble is created.
    /// </summary>
    /// <param name="position">Position where the bubble is created first</param>
    private void CreateBubble(Vector position)
    {
        PhysicsObject bubble = new PhysicsObject(50, 50);
        bubble.Shape = Shape.Circle;
        bubble.Color = RandomGen.SelectOne<Color>(bubbleColors); 
        bubble.Position = position;
        bubble.Mass = 15.0;
        bubble.Restitution = 0.1;
        Add(bubble);
        bubbles.Add(bubble);
        Mouse.ListenOn(bubble, MouseButton.Left, ButtonState.Pressed, BubblePop, null, bubble);
    }


    /// <summary>
    /// Takes care of score counter, goal label and other UI. 
    /// </summary>
    private void PlayerMotivator ()
    {
        scoreCounter = new IntMeter(0);
        scoreCounter.MaxValue = DifficultyAutomator(BUBBLES, 2);

        Label scoreLabel = new Label();
        scoreLabel.Y += 300;
        scoreLabel.TextColor = Color.Yellow;
        scoreLabel.Color = Color.Black;
        scoreLabel.Title = "Score";

        scoreLabel.BindTo(scoreCounter);
        Add(scoreLabel);

        Label goalLabel = new Label();
        goalLabel.Y += 200;
        goalLabel.TextColor = Color.BrightGreen;
        goalLabel.Color = Color.Black;

        goalLabel.Title = "Goal";
        goalLabel.Text = scoreCounter.MaxValue.ToString();
        Add(goalLabel);

        Label goalLabelText = new Label();
        goalLabelText.Y += 225;
        goalLabelText.TextColor = Color.BrightGreen;
        goalLabelText.Color = Color.Black;

        goalLabelText.Text = "Reach Goal to WIN";
        Add(goalLabelText);
    }


    /// <summary>
    /// Calculates SCIENTIFIC OPTIMUM FUN™ for the player.
    /// </summary>
    /// <param name="maxObjects">Maximum interactable objects in the game</param>
    /// <param name="difficultyFactor">Magic number to adjust player difficulty</param>
    /// <returns>Optimized difficulty score</returns>
    private static int DifficultyAutomator(int maxObjects, double difficultyFactor)
    {
        int tempScore = maxObjects;
        
        return (int)(tempScore * difficultyFactor); 
    }


    /// <summary>
    /// Rewards winning player. Everybody is a winner.
    /// </summary>
    private void EndGame()
    {
        MessageDisplay.Add("You are the winner!");
    }


    /// <summary>
    /// If player doesn't reach the winning threshold this useful helpmessage is triggered.
    /// </summary>
    private void Lost()
    {
        MessageDisplay.Add("Bubbles are gone? Try again! Longer chains help with score multipliers!");
    }
}