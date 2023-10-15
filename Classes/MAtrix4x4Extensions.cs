using System;
using System.Numerics;

namespace HgmViewer.Classes
{
    public static class Matrix4x4Extensions
    {
        public static float GetElementAt(this Matrix4x4 m, int index)
        {
            return index switch
            {
                0 => m.M11,
                1 => m.M12,
                2 => m.M13,
                3 => m.M14,
                4 => m.M21,
                5 => m.M22,
                6 => m.M23,
                7 => m.M24,
                8 => m.M31,
                9 => m.M32,
                10 => m.M33,
                11 => m.M34,
                12 => m.M41,
                13 => m.M42,
                14 => m.M43,
                15 => m.M44,
                _ => throw new ArgumentException($"{nameof(Matrix4x4Extensions)}.{nameof(GetElementAt)}: Index ({index}) out of range"),
            };
        }
    }
}