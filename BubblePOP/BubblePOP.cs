using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class BubblePOP : PhysicsGame
{
    

    private int BUBBLES = 5;
    public int pallojaCount = 0;

    private Image mapImage = LoadImage("map");  
    private Shape mapShape;

    private PhysicsObject Bubble;

    List<PhysicsObject> balls = new List<PhysicsObject> ();

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
        balls.Remove(bubble);
        //if (bubble.Color) BubblePOP(this);
        scoreCounter.Value += 1;
        scoreCounter.UpperLimit += KaikkiKeratty;
    }

    private void CreateLevel()
    {
        PhysicsObject map = PhysicsObject.CreateStaticObject(800, 600, mapShape);
        map.Image = mapImage;
        Add(map);
    }

    void Populate()
    {
        Vector position = new Vector(0, 400);

        Timer.CreateAndStart(0.2, delegate { CreateBubble(position); }
            );
        if (pallojaCount < BUBBLES) timer.Stop();
        /*  
        for (int i = 0; i < BUBBLES; i++)
          {
            timer.Stop();  
            //Timer.SingleShot(0.2,
                 // delegate { CreateBubble(position); }
                 // );
          }
        */

    }

    private void CreateBubble(Vector position)
    {
        
        Bubble = new PhysicsObject(50, 50);
        Bubble.Shape = Shape.Circle;
        Bubble.Color = RandomGen.NextColor(); 
        Bubble.Position = position;
        Bubble.Mass = 15.0;
        Bubble.Restitution = 0.1;
        pallojaCount++;
        Add(Bubble);
        balls.Add(Bubble);
        Mouse.ListenOn(Bubble, MouseButton.Left, ButtonState.Pressed, BubblePop, null, Bubble);
        


        // Softness voi olla olennainen game feeling kannalta? https://trac.cc.jyu.fi/projects/npo/wiki/OliotJaSelitykset#Tärkeimmätominaisuudet5
    }

    void CreateScoreCounter ()
    {
        scoreCounter = new IntMeter(0);
        scoreCounter.MaxValue = 1;

        Label scoreLabel = new Label();
        scoreLabel.Y  =+ 300;
        scoreLabel.TextColor = Color.Yellow;
        scoreLabel.Color = Color.Black;

        scoreLabel.BindTo(scoreCounter);
        Add(scoreLabel);
    }


    void KaikkiKeratty()
    {
        MessageDisplay.Add("Pelaaja 1 voitti pelin.");
    }

}

