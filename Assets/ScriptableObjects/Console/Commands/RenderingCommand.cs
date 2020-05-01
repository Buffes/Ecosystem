using System;
using UnityEngine;

namespace Ecosystem.Console
{
    [CreateAssetMenu(menuName = "Console/Commands/RenderingCommand")]
    public class RenderingCommand : Command
    {
        private static readonly string[] ENABLE_WORDS = { "true", "on" };
        private static readonly string[] DISABLE_WORDS = { "false", "off" };

        private Camera camera = null;

        public override void Execute(ICommandSender sender, string[] args)
        {
            string m = null;

            if (args.Length == 0) m = ResultMessage(ToggleRendering());
            else if (Array.Exists(ENABLE_WORDS, e => e == args[0])) m = ResultMessage(Rendering(true));
            else if (Array.Exists(DISABLE_WORDS, e => e == args[0])) m = ResultMessage(Rendering(false));

            if (m != null) sender.SendMessage(m);
        }

        private bool? ToggleRendering()
        {
            Camera mainCam = Camera.main;
            return Rendering(mainCam != null ? !mainCam.enabled : true, mainCam);
        }

        private bool? Rendering(bool enabled) => Rendering(enabled, Camera.main);

        private bool? Rendering(bool enabled, Camera camera)
        {
            if (camera == null) camera = this.camera;
            else this.camera = camera;

            if (camera != null) camera.enabled = enabled;
            return camera?.enabled;
        }

        private static string ResultMessage(bool? renderingEnabled)
        {
            if (!renderingEnabled.HasValue) return null;
            return "Rendering: " + (renderingEnabled.Value ? "On" : "Off");
        }
    }
}
