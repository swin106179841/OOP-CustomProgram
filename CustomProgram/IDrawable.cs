namespace CustomProgram
{
    interface IDrawable
    {
        public float X {get; set;}
        public float Y {get; set;}
        public void Draw();
        public void Update();
    }
    interface IAnimated
    {
        public int AnimationFrame {get; set;}
        public void Animate();
    }
}