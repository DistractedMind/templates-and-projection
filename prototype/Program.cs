public interface IPrototype<T>
{
    T Clone();
}

public class ShapeStyle : IPrototype<ShapeStyle>
{
    public string Color { get; set; }
    public int BorderSize { get; set; }

    public ShapeStyle(string color, int borderSize)
    {
        Color = color;
        BorderSize = borderSize;
    }

    public ShapeStyle Clone()
    {
        Console.WriteLine($"Клонирование ShapeStyle (Color: {Color}, Border: {BorderSize})");
        return (ShapeStyle)this.MemberwiseClone();
    }

    public override string ToString()
    {
        return $"Style[Color={Color}, Border={BorderSize}]";
    }
}

public class Circle : IPrototype<Circle>
{
    public int Radius { get; set; }
    public Point Center { get; set; }

    public Circle(int radius, Point center)
    {
        Radius = radius;
        Center = center;
    }

    public Circle Clone()
    {
        Console.WriteLine($"Клонирование Circle (Radius: {Radius}, Center: {Center})");
        return (Circle)this.MemberwiseClone();
    }

    public override string ToString()
    {
        return $"Circle[Radius={Radius}, Center={Center}]";
    }
}

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"({X},{Y})";
    }
}

public class Rectangle : IPrototype<Rectangle>
{
    public int Width { get; set; }
    public int Height { get; set; }
    public ShapeStyle Style { get; set; }

    public Rectangle(int width, int height, ShapeStyle style)
    {
        Width = width;
        Height = height;
        Style = style;
    }

    public Rectangle Clone()
    {
        Console.WriteLine($"Клонирование Rectangle (Width: {Width}, Height: {Height}, Style: {Style})");
        Rectangle clone = (Rectangle)this.MemberwiseClone();
        clone.Style = this.Style?.Clone();
        Console.WriteLine("Глубокое копирование Style завершено.");
        return clone;
    }

    public override string ToString()
    {
        return $"Rectangle[Width={Width}, Height={Height}, Style={Style}]";
    }
}

public class Triangle : IPrototype<Triangle>
{
    public int SideA { get; set; }
    public int SideB { get; set; }
    public int SideC { get; set; }

    public Triangle(int a, int b, int c)
    {
        SideA = a;
        SideB = b;
        SideC = c;
    }

    public Triangle Clone()
    {
        Console.WriteLine($"Клонирование Triangle (Sides: {SideA}, {SideB}, {SideC})");
        return (Triangle)this.MemberwiseClone();
    }

    public override string ToString()
    {
        return $"Triangle[Sides={SideA},{SideB},{SideC}]";
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Создание прототипов");

        Circle originalCircle = new Circle(10, new Point(5, 5));
        Console.WriteLine($"Original Circle: {originalCircle}");

        ShapeStyle originalStyle = new ShapeStyle("Red", 2);
        Rectangle originalRectangle = new Rectangle(20, 30, originalStyle);
        Console.WriteLine($"Original Rectangle: {originalRectangle}");

        Triangle originalTriangle = new Triangle(3, 4, 5);
        Console.WriteLine($"Original Triangle: {originalTriangle}\n");


        Console.WriteLine("Клонирование объектов");
        Circle clonedCircle = originalCircle.Clone();
        Rectangle clonedRectangle = originalRectangle.Clone();
        Triangle clonedTriangle = originalTriangle.Clone();

        Console.WriteLine("\nИсходные и клонированные объекты ПОСЛЕ клонирования");
        Console.WriteLine($"Original Circle: {originalCircle} (HashCode: {originalCircle.GetHashCode()})");
        Console.WriteLine($"Cloned Circle:   {clonedCircle} (HashCode: {clonedCircle.GetHashCode()})");
        Console.WriteLine($"Original Rect:   {originalRectangle} (HashCode: {originalRectangle.GetHashCode()}, Style HashCode: {originalRectangle.Style?.GetHashCode()})");
        Console.WriteLine($"Cloned Rect:     {clonedRectangle} (HashCode: {clonedRectangle.GetHashCode()}, Style HashCode: {clonedRectangle.Style?.GetHashCode()})");
        Console.WriteLine($"Original Tri:    {originalTriangle} (HashCode: {originalTriangle.GetHashCode()})");
        Console.WriteLine($"Cloned Tri:      {clonedTriangle} (HashCode: {clonedTriangle.GetHashCode()})\n");

        Console.WriteLine("Изменение клонированных объектов");
        clonedCircle.Radius = 15;
        clonedCircle.Center = new Point(1, 1);
        clonedRectangle.Width = 25;
        if (clonedRectangle.Style != null) {
            clonedRectangle.Style.Color = "Blue";
        }
        clonedTriangle.SideA = 6;

        Console.WriteLine("\nИсходные и клонированные объекты ПОСЛЕ изменения клонов");
        Console.WriteLine("оригиналы не изменились");
        Console.WriteLine($"Original Circle: {originalCircle}");
        Console.WriteLine($"Cloned Circle:   {clonedCircle}");
        Console.WriteLine($"Original Rect:   {originalRectangle}");
        Console.WriteLine($"Cloned Rect:     {clonedRectangle}");
        Console.WriteLine($"Original Tri:    {originalTriangle}");
        Console.WriteLine($"Cloned Tri:      {clonedTriangle}");

        Console.WriteLine("\nИзменение оригинального объекта ПОСЛЕ клонирования");
        originalRectangle.Height = 35;
        if (originalRectangle.Style != null) {
             originalRectangle.Style.BorderSize = 5;
        }

        Console.WriteLine("\nИсходные и клонированные объекты ПОСЛЕ изменения ОРИГИНАЛА");
        Console.WriteLine("клон не изменился");
        Console.WriteLine($"Original Rect: {originalRectangle}");
        Console.WriteLine($"Cloned Rect:   {clonedRectangle}");
    }
}
