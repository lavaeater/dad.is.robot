using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace robot.dad.game.Scenes
{
    /// <summary>
    /// My own custom class for managing scenes and transitions.
    /// </summary>
    public class SceneManager
    {
        public Scene CurrentScene { get; set; }
        public Action<Scene> SceneDone { get; set; }
        public MainScene MainScene { get; set; }
    }
}
