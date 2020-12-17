using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class BubblePOP : PhysicsGame
{
    

    private int BUBBLES = 1;

    private Image mapImage = LoadImage("map");  
    private Shape mapShape;

    private PhysicsObject Bubble;

    List<PhysicsObject> balls = new List<PhysicsObject> ();



    public override void Begin()
    {
        Gravity = new Vector(0.0, -200);
        Timer timer = new Timer(); // kuinka ajastaa väli Populatelle

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

    private void bubblePop(PhysicsObject bubble)
    {
        bubble.Destroy();
        balls.Remove(bubble);
        //connecting bubbles
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
            CreateBubble(position);
           
        }

    }

    private void CreateBubble(Vector position)
    {
        
        Bubble = new PhysicsObject(50, 50);
        Bubble.Shape = Shape.Circle;
        Bubble.Color = RandomGen.NextColor(); 
        Bubble.Position = position;
        Bubble.Mass = 15.0;
        Bubble.Restitution = 0.1;
        Add(Bubble);
        balls.Add(Bubble);
        Mouse.ListenOn(Bubble, MouseButton.Left, ButtonState.Pressed, bubblePop, null, Bubble);



        // Softness voi olla olennainen game feeling kannalta? https://trac.cc.jyu.fi/projects/npo/wiki/OliotJaSelitykset#Tärkeimmätominaisuudet5
    }


    


}

