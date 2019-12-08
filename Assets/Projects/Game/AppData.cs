using CsExtensions;

namespace Game {
    public class AppData {
        public readonly Signal OnStartGame = new Signal();
        public readonly Signal OnGameOver = new Signal();
        public readonly Signal OnAppClose = new Signal();
    }
}