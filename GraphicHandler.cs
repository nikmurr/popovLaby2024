using System;
using System.Windows.Forms;
using ZedGraph;

public static class GraphicHandler
{
    private static GraphPane initialGraphPane;

    public static void InitialiseGraph(ZedGraphControl zedGraphControl)
    {
        if (zedGraphControl == null)
        {
            throw new ArgumentNullException(nameof(zedGraphControl), "ZedGraphControl cannot be null");
        }

        // Сохраняем начальное состояние GraphPane
        initialGraphPane = zedGraphControl.GraphPane.Clone();

        // Очищаем график перед построением новых данных
        zedGraphControl.GraphPane.CurveList.Clear();

        // Строим график функции Y1=sin(x)
        LineItem curveY1 = zedGraphControl.GraphPane.AddCurve("Y1=sin(x)", GenerateY1Data(), System.Drawing.Color.Blue, SymbolType.None);

        // Строим график функции Y2=x^2
        LineItem curveY2 = zedGraphControl.GraphPane.AddCurve("Y2=x^2", GenerateY2Data(), System.Drawing.Color.Red, SymbolType.None);

        // Вызываем функцию для определения точек пересечения
        FindAndMarkIntersectionPoints(zedGraphControl.GraphPane, curveY1, curveY2);

        zedGraphControl.GraphPane.XAxis.Scale.MinAuto = false;
        zedGraphControl.GraphPane.XAxis.Scale.MaxAuto = false;
        zedGraphControl.GraphPane.YAxis.Scale.MinAuto = false;
        zedGraphControl.GraphPane.YAxis.Scale.MaxAuto = false;

        // Обновляем график
        zedGraphControl.AxisChange();
        zedGraphControl.Invalidate();
    }

    public static void ResetView(ZedGraphControl zedGraphControl)
    {
        if (zedGraphControl == null)
        {
            throw new ArgumentNullException(nameof(zedGraphControl), "ZedGraphControl cannot be null");
        }

        // Восстанавливаем начальное состояние GraphPane
        zedGraphControl.GraphPane = initialGraphPane.Clone();

        // Обновляем график
        zedGraphControl.AxisChange();
        zedGraphControl.Invalidate();
    }

    private static PointPairList GenerateY1Data()
    {
        PointPairList list = new PointPairList();
        for (double x = -10; x <= 10; x += 0.1)
        {
            double y = Math.Sin(x);
            list.Add(x, y);
        }
        return list;
    }

    private static PointPairList GenerateY2Data()
    {
        PointPairList list = new PointPairList();
        for (double x = -10; x <= 10; x += 0.1)
        {
            double y = Math.Pow(x, 2);
            list.Add(x, y);
        }
        return list;
    }

    private static void FindAndMarkIntersectionPoints(GraphPane graphPane, LineItem curveY1, LineItem curveY2)
    {
        PointPairList intersectionPoints = FindIntersectionPoints(curveY1, curveY2);

        // Выводим MessageBox с точками пересечения
        string message = $"Точки пересечения:\n";
        foreach (PointPair point in intersectionPoints)
        {
            message += $"({point.X}, {point.Y})\n";

            // Устанавливаем толщину обводки точек
            LineItem intersectionPoint = graphPane.AddCurve(null, new double[] { point.X }, new double[] { point.Y }, System.Drawing.Color.Green, SymbolType.Circle);
            intersectionPoint.Symbol.Border.Width = 3;
        }

        MessageBox.Show(message, "Точки пересечения", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }


    private static PointPairList FindIntersectionPoints(LineItem curve1, LineItem curve2)
    {
        PointPairList intersectionPoints = new PointPairList();

        for (int i = 0; i < curve1.Points.Count - 1; i++)
        {
            double x1 = curve1.Points[i].X;
            double y1 = curve1.Points[i].Y;
            double x2 = curve1.Points[i + 1].X;
            double y2 = curve1.Points[i + 1].Y;

            for (int j = 0; j < curve2.Points.Count - 1; j++)
            {
                double x3 = curve2.Points[j].X;
                double y3 = curve2.Points[j].Y;
                double x4 = curve2.Points[j + 1].X;
                double y4 = curve2.Points[j + 1].Y;

                if (LineIntersects(x1, y1, x2, y2, x3, y3, x4, y4, out double intersectionX, out double intersectionY))
                {
                    intersectionPoints.Add(intersectionX, intersectionY);
                }
            }
        }

        return intersectionPoints;
    }

    private static bool LineIntersects(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, out double intersectionX, out double intersectionY)
    {
        intersectionX = double.NaN;
        intersectionY = double.NaN;

        double d = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
        if (d == 0)
            return false;

        double pre = (x1 * y2 - y1 * x2);
        double post = (x3 * y4 - y3 * x4);
        double x = (pre * (x3 - x4) - (x1 - x2) * post) / d;
        double y = (pre * (y3 - y4) - (y1 - y2) * post) / d;

        if (x < Math.Min(x1, x2) || x > Math.Max(x1, x2) || x < Math.Min(x3, x4) || x > Math.Max(x3, x4))
            return false;
        if (y < Math.Min(y1, y2) || y > Math.Max(y1, y2) || y < Math.Min(y3, y4) || y > Math.Max(y3, y4))
            return false;

        intersectionX = x;
        intersectionY = y;
        return true;
    }
}
