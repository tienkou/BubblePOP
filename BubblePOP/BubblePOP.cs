using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

/// <summary>
/// @author Harja "tienkou" Jonne
/// @version 1.0 - gold
/// </summary>
public class BubblePOP : PhysicsGame
{
    

    private int BUBBLES = 75;
    private int pallojaCount = 0;

    private Image mapImage = LoadImage("map");  
    private Shape mapShape;

    private PhysicsObject Bubble;

    List<PhysicsObject> bubbles = new List<PhysicsObject> ();
    List<Color> bubbleColors = new List<Color> {Color.DarkForestGreen, Color.DarkBlue, Color.DarkMagenta, Color.Gold, Color.Red };

    IntMeter scoreCounter;
    Timer timer = new Timer();

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

    private void PlayerController()
    {
    //    Mouse.Listen(MouseButton.Left, ButtonState.Pressed, bubblePop, "Bubble(s) popped!");
    }
    /// <summary>
    /// Takes care everything to do with Bubble destruction.
    /// </summary>
    /// <param name="bubble"></param>
    private void BubblePop(PhysicsObject bubble)
    {
        bubble.Destroy();
        bubbles.Remove(bubble);
        double distance = 0.0;
        int scoreMultiplier = 1;

        for (int i = 0; i < bubbles.Count; i++)
        {
            distance = Vector.Distance(bubble.Position, bubbles[i].Position);
            if (bubble.Color == bubbles[i].Color && distance < 60) 
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
    /// <param name="position"></param>
    private void CreateBubble(Vector position)
    {
        
        Bubble = new PhysicsObject(50, 50);
        Bubble.Shape = Shape.Circle;
        Bubble.Color = RandomGen.SelectOne<Color>(bubbleColors); 
        Bubble.Position = position;
        Bubble.Mass = 15.0;
        Bubble.Restitution = 0.1;
        pallojaCount++;
        Add(Bubble);
        bubbles.Add(Bubble);
        Mouse.ListenOn(Bubble, MouseButton.Left, ButtonState.Pressed, BubblePop, null, Bubble);
       
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
    /// <returns></returns>
    private static int DifficultyAutomator(int maxObjects, double difficultyFactor)
    {
        int tempScore = maxObjects;
        
        return (int)(tempScore * difficultyFactor); //why is this always 15? wtf
    }

    /// <summary>
    /// Rewards winning player. Everybody is a winner.
    /// </summary>
    private void EndGame()
    {
        MessageDisplay.Add("You are the winner!");
    }

    private void Lost()
    {
        MessageDisplay.Add("Bubbles are gone? Try again! Longer chains help with score multipliers!");
    }

}