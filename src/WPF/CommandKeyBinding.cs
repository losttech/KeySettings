namespace LostTech.App
{
    using System.Collections.Generic;
    using System.Linq;
    using LostTech.App.Input;

    public sealed class CommandKeyBinding
    {
        public string CommandName { get; set; }
        public KeyStroke Shortcut { get; set; }
    }
}
