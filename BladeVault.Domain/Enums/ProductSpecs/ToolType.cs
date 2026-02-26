using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Enums.ProductSpecs
{
    public enum ToolType
    {
        // Ріжучі
        Knife = 1,
        Scissors = 2,
        Saw = 3,
        Hawkbill = 4,

        // Плоскогубці
        Pliers = 5,
        NeedleNosePliers = 6,
        WireCutter = 7,
        WireStripper = 8,
        CrimpTool = 9,

        // Викрутки
        PhillipsScrewdriver = 10,
        FlatScrewdriver = 11,
        TorxScrewdriver = 12,

        // Відкривачки
        CanOpener = 13,
        BottleOpener = 14,

        // Інше
        FileNail = 15,
        MetalFile = 16,
        Ruler = 17,
        Awl = 18,
        Tweezers = 19,
        Toothpick = 20,
        Hook = 21,
        Magnifier = 22,
        Pen = 23,
        USBDrive = 24,
        BitDriver = 25
    }
}
