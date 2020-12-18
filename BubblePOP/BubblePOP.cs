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
    List<Color> bubbleColors = new List<Color> {Color.Yellow, Color.White, Color.Blue, Color.Green, Color.HotPink };

    IntMeter scoreCounter;
    Timer timer = new Timer();

    public override void Begin()
    {
        Gravity = new Vector(0.0, -200);
        CreateScoreCounter();

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

    private void BubblePop(PhysicsObject bubble)
    {
        bubble.Destroy();
        bubbles.Remove(bubble);
        double distance = 0.0;



        for (int i = 0; i < bubbles.Count; i++)
        {
            distance = Vector.Distance(bubble.Position, bubbles[i].Position);
            if (bubble.Color == bubbles[i].Color && distance < 60) 
            {
                BubblePop(bubbles[i]);
                i--;
            }
        }
        

        scoreCounter.Value += 1;
        scoreCounter.UpperLimit += KaikkiKeratty;
    }

    private void CreateLevel()
    {
        PhysicsObject map = PhysicsObject.CreateStaticObject(800, 600, mapShape);
        map.Image = mapImage;
        Add(map);
    }

    private void Populate()
    {
        Vector position = new Vector(0, 400);

        for (int i = 0; i < BUBBLES; i++)
          {
            //timer.Stop();  
            Timer.SingleShot(0.1+0.1*i,
                 delegate { CreateBubble(position); }
                 );
          }
       

    }
    /// <summary>
    /// 
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
        


        // Softness voi olla olennainen game feeling kannalta? https://trac.cc.jyu.fi/projects/npo/wiki/OliotJaSelitykset#Tärkeimmätominaisuudet5
    }

    private void CreateScoreCounter ()
    {
        int tempScore = BUBBLES;
        scoreCounter = new IntMeter(0);
        scoreCounter.MaxValue = tempScore - (int)(tempScore * 0.75);

        Label scoreLabel = new Label();
        scoreLabel.Y += 300;
        scoreLabel.TextColor = Color.Yellow;
        scoreLabel.Color = Color.Black;

        scoreLabel.BindTo(scoreCounter);
        Add(scoreLabel);
    }


    //#TODO: Funktio jolla on attribuutteja ja palauttaa sen
    //scoreCounter.MaxValue = tempScore - (int) (tempScore* 0.75);

    private void KaikkiKeratty()
    {
        MessageDisplay.Add("Pelaaja 1 voitti pelin.");
    }

}