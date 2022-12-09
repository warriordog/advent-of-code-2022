namespace AdventOfCode.Common;

// y / row / vertical
// x / col / horizontal
public class Plane<T>
{
    public int YMin => _yAxis.Min;
    public int YMax => _yAxis.Max;
    
    public int XMin { get; private set; }
    public int XMax { get; private set; }


    public Axis<T>? this[int y]
    {
        get => GetXAxis(y);
        set => SetXAxis(y, value);
    }

    public T? this[int y, int x]
    {
        get => Get(y, x);
        set => Set(y, x, value);
    }
    
    private readonly Axis<Axis<T>> _yAxis = new();

    public void Set(int y, int x, T? value)
    {
        // Get or create the X axis
        var xAxis = _yAxis[y];
        if (xAxis == null)
        {
            xAxis = new Axis<T>();
            _yAxis[y] = xAxis;
        }

        // Update value
        xAxis[x] = value;

        // Update ranges
        if (x > XMax)
            XMax = x;
        if (x < XMin)
            XMin = x;
    }

    public T? Get(int y, int x)
    {
        var xAxis = _yAxis[y];
        return xAxis == null ? default : xAxis[x];
    }

    public Axis<T>? GetXAxis(int y) => _yAxis[y];

    private void SetXAxis(int y, Axis<T>? xAxis)
    {
        _yAxis[y] = xAxis;

        if (xAxis != null)
        {
            if (xAxis.Max > XMax)
                XMax = xAxis.Max;
            if (xAxis.Min < XMin)
                XMin = xAxis.Min;
        }
    }
    
    public void Clear()
    {
        _yAxis.Clear();
        XMin = 0;
        XMax = 0;
    }
}