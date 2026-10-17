using System.Collections.Generic;
using UnityEngine;

// Pointing-hand mouse cursor, drawn in code so no image asset is needed.
// Objects call Show(this) when the mouse is over them (or while dragging) and
// Hide(this) when done; the hand stays while anyone still wants it.
public static class HandCursor
{
    // X = black outline, o = white fill, . = transparent; top row is the fingertip
    static readonly string[] shape =
    {
        "......XX.........",
        ".....XooX........",
        ".....XooX........",
        ".....XooX........",
        ".....XooX........",
        ".....XooXXX......",
        ".....XooXooXXX...",
        ".....XooXooXooXX.",
        "..XX.XooXooXooXoX",
        ".XooXXooooooooXoX",
        ".XoooXooooooooooX",
        "..XoooooooooooooX",
        "...XooooooooooooX",
        "...XoooooooooooX.",
        "....XooooooooooX.",
        "....XoooooooooX..",
        ".....XooooooooX..",
        ".....XooooooooX..",
        ".....XXXXXXXXXX..",
    };

    const int scale = 2;
    static Texture2D texture;
    static readonly HashSet<Object> wanting = new HashSet<Object>();

    public static void Show(Object who)
    {
        if (wanting.Add(who) && wanting.Count == 1)
        {
            if (texture == null)
                texture = build();

            // hotspot = tip of the pointing finger
            Cursor.SetCursor(texture, new Vector2(6.5f * scale, 0f), CursorMode.Auto);
        }
    }

    public static void Hide(Object who)
    {
        if (wanting.Remove(who) && wanting.Count == 0)
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    static Texture2D build()
    {
        int width = shape[0].Length * scale;
        int height = shape.Length * scale;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;

        for (int row = 0; row < shape.Length; row++)
        {
            for (int col = 0; col < shape[row].Length; col++)
            {
                char c = shape[row][col];
                Color color = c == 'X' ? Color.black : c == 'o' ? Color.white : Color.clear;

                for (int dy = 0; dy < scale; dy++)
                    for (int dx = 0; dx < scale; dx++)
                        // texture rows go bottom-up; the shape is written top-down
                        tex.SetPixel(col * scale + dx, height - 1 - (row * scale + dy), color);
            }
        }

        tex.Apply();
        return tex;
    }
}
