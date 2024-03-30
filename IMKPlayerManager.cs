using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minikit.InputAndInterface
{
    public interface IMKPlayerManager
    {
        public static IMKPlayerManager instance;


        public List<IMKPlayer> GetPlayers();
    }
}
