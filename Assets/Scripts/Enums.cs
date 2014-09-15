using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public enum ItemType
    {
        Nothing,
        Wood,
        Iron
    }
    
    public enum WorkTask
    {
        ChopWood, Unload, Mine
    }

    public enum Job
    {
        None,
        WoodCutter,
        Miner
    }
}
