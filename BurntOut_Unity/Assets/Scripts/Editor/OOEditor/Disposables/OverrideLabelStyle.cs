using System;
using UnityEngine;

public class OverrideLabelStyle : IDisposable
{
    public int FontSize { get; set; }
    public FontStyle FontStyle { get; set; }

    public OverrideLabelStyle()
    {

    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
