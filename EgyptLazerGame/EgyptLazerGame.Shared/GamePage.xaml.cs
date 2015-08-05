using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EgyptLazerGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel
    {
        readonly Game1 _game;

        public GamePage(string launchArguments)
        {
            this.InitializeComponent();

            // Create the game.

            _game = XamlGame<Game1>.Create(launchArguments, Window.Current.CoreWindow, this);
        }
    }
}
