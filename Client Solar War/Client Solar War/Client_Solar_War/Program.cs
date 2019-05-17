using System;

namespace Client_Solar_War
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
				((System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(game.Window.Handle)).Icon = new System.Drawing.Icon(@"Content\Icon\icon1.ico");
				((System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(game.Window.Handle)).Text = "Solar War";
				game.Run();
            }
        }
    }
#endif
}

