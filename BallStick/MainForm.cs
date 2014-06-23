using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.IO;
namespace BallStick
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			
			
			InitializeBlock();
			InitializeBall();
			InitializePaddle();
			//assign properties to style
			SetStyle(ControlStyles.SupportsTransparentBackColor,true);
			this.BackColor = Color.Transparent;
			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Opaque, false);
			SetStyle(ControlStyles.UserPaint,true);
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint,true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			
			
			
			
			
			MessageBox.Show("Welcom to this small application made by Liu Zhe.");
			
			this.Height = 600;
			this.Width = 400;
			
		}
		
		
		
		static int topPad = 20;
		static  int bottomPad = 20;
		Random ballRandom = new Random();
		
		
		
		
		int blockHeight = 80;
		int blockRows = 2;
		int blockColumns = 5;
		
		Color blockColor = Color.White;
		
		Rectangle[] blocks;
		Graphics[] blockimgs;
		
		
		static  int ballWidth = 20;
		int ballMovement_x;
		int ballMovement_y;
		
		Rectangle ball;
		Color ballColor = Color.Yellow;
		
		static int paddleWidth = 50;
		static int paddleHeight = 10;
		int paddleSpeed;
		Rectangle paddle;
		Color paddleColor = Color.White;
		
		
		int ballPosition;
		
		int maxBallnumber = 3;
		
		
		
		
		void InitializeBlock() {
			
			blocks=new Rectangle[blockRows*blockColumns];
			blockimgs = new Graphics[blockRows*blockColumns];
			
			
			Rectangle rect = new Rectangle(0,0,0,0);
			rect.Width =  ClientRectangle.Width/blockColumns-2;
			rect.Height = blockHeight-2;
			for (int i = 0; i < blockRows; i++)
			{
				
				for (int r = 0; r < blockColumns; r++)
				{
					rect.Location = new Point( r*ClientRectangle.Width/blockColumns,
					                          i*blockHeight+topPad);
					
					blocks[i * blockColumns + r] = rect;
					
					
					
				}
			}
		}
		
		void AnotherBall(){
			
			ball = new Rectangle(
				ClientRectangle.Width/2,
				ClientRectangle.Height/3,
				ballWidth,
				ballWidth
			);
			
			
			
			
		}
		
		
		
		void InitializeBall() {
			ball = new Rectangle(
				ClientRectangle.Width/2,
				ClientRectangle.Height/3,
				ballWidth,
				ballWidth
			);
			
			ballMovement_x = ballRandom.Next(10) - 3;
			ballMovement_y = 12 - ballMovement_x;
			
			
		}
		
		void InitializePaddle() {
			paddle = new Rectangle(
				ClientRectangle.Width/2,
				ClientRectangle.Height-bottomPad-paddleHeight,
				paddleWidth,
				paddleHeight
				
			);
			paddleSpeed = ClientRectangle.Width / 50;
		}
		
		
		protected override void OnPaint(PaintEventArgs e)
		{
			var g=e.Graphics;
			
			
			System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			Stream myStream = myAssembly.GetManifestResourceStream("BallStick.baba.jpg");
			Image img = new Bitmap(myStream);
			using (var bitBuf=new Bitmap(ClientRectangle.Width,ClientRectangle.Height))
				using (var buffer=Graphics.FromImage(bitBuf))
					using (var blockBrush=new TextureBrush(img))
						
						using (var ballBrush= new LinearGradientBrush(new PointF(0,0),new PointF(0,9),Color.White,Color.Green))
							
							
							using (var paddleBrush=new SolidBrush(paddleColor)){
				
			
				
				buffer.SmoothingMode = SmoothingMode.AntiAlias;
				
				buffer.Clear(this.BackColor);
				buffer.FillEllipse(ballBrush,ball);
				buffer.FillRectangle(paddleBrush, paddle);
				
				for (int i = 0; i < blockColumns*blockRows; i++)
				{
					buffer.FillRectangle(blockBrush,blocks[i]);
					
				}
				
				g.DrawImage(bitBuf, 0, 0);
				
				
			}
			
			
			base.OnPaint(e);
		}
		
		
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Left)
			{
				paddle.X -= paddleSpeed;
				if (paddle.X < 1)
					paddle.X = 1;
				
			}
			else if (e.KeyCode == Keys.Right)
			{
				paddle.X += paddleSpeed;
				if (paddle.X > ClientRectangle.Width - paddle.Width - 1)
					paddle.X = ClientRectangle.Width - paddleWidth - 1;
				
				
			}
			else if  (e.KeyCode == Keys.Up)
			{
				AnotherBall();
				
				
			}
			
			base.OnKeyDown(e);
		}
		
		protected override void OnResize(EventArgs e)
		{
			InitializeBlock();
			InitializeBall();
			InitializePaddle();
			
			
			
			
			
			
			base.OnResize(e);
		}
		
//
		
		void Timer1Tick(object sender, EventArgs e)
		{
			for (int i = 0; i <blockRows*blockColumns; i++)
			{
				if(ball.IntersectsWith(blocks[i])){
					
					
					blocks[i] = new Rectangle();
					
					ballMovement_y = -ballMovement_y;
					
					
					
				}
			}
			
			if (ball.IntersectsWith(paddle)) {
				
				if (ball.X - paddle.X < paddle.Width / 3) {
					
					ballMovement_x = ballMovement_x-2;
					ballMovement_y -= 2;
					
				}
				else if(ball.X-paddle.X>2*paddle.Width/3){
					
					
					ballMovement_x =ballMovement_x+ 2;
					ballMovement_y += 2;
					
				}
				ballMovement_y = -ballMovement_y;
				ballPosition = ball.X;
			}
			
			if (ball.X < 0 || ball.X > ClientRectangle.Width)
				ballMovement_x = -ballMovement_x;
			if (ball.Y < 0)
				ballMovement_y = -ballMovement_y;
			if (ball.Y > ClientRectangle.Height)
			{
				InitializeBall();
			}
			
			ball.X += ballMovement_x;
			ball.Y += ballMovement_y;
			
			this.Invalidate();
		}
		
		
	}
}
