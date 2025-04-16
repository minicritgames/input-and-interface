using System.Collections.Generic;

namespace Minikit.InputAndInterface
{
    public interface IMKPlayerManager
    {
        public static IMKPlayerManager instance;


        public List<IMKPlayer> GetPlayers();
    }
}
