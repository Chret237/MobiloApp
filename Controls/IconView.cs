namespace MobiloApp.Controls;

/// <summary>Draws one of the design-system icons (see IconData) with a rounded stroke.</summary>
public class IconView : GraphicsView
{
	public static readonly BindableProperty IconProperty = Make(nameof(Icon), "");
	public static readonly BindableProperty ColorProperty = Make(nameof(Color), Theme.Ink);
	public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(nameof(IconSize), typeof(double), typeof(IconView), 20d,
		propertyChanged: (b, _, n) => { var v = (IconView)b; v.WidthRequest = v.HeightRequest = (double)n; v.Invalidate(); });
	public static readonly BindableProperty StrokeWidthProperty = Make(nameof(StrokeWidth), 1.8f);
	public static readonly BindableProperty FilledProperty = Make(nameof(Filled), false);

	static BindableProperty Make<T>(string name, T def) =>
		BindableProperty.Create(name, typeof(T), typeof(IconView), def, propertyChanged: (b, _, _) => ((IconView)b).Invalidate());

	public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }
	public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }
	public double IconSize { get => (double)GetValue(IconSizeProperty); set => SetValue(IconSizeProperty, value); }
	public float StrokeWidth { get => (float)GetValue(StrokeWidthProperty); set => SetValue(StrokeWidthProperty, value); }
	public bool Filled { get => (bool)GetValue(FilledProperty); set => SetValue(FilledProperty, value); }

	public IconView()
	{
		Drawable = new IconDrawable(this);
		WidthRequest = HeightRequest = 20;
		InputTransparent = true;
		HorizontalOptions = VerticalOptions = LayoutOptions.Center;
	}

	sealed class IconDrawable(IconView owner) : IDrawable
	{
		static readonly Dictionary<string, PathF> Cache = new();

		public void Draw(ICanvas canvas, RectF rect)
		{
			var path = Get(owner.Icon);
			if (path is null) return;

			canvas.SaveState();
			var scale = Math.Min(rect.Width, rect.Height) / 24f;
			canvas.Scale(scale, scale);
			canvas.StrokeColor = owner.Color;
			canvas.StrokeSize = owner.StrokeWidth;
			canvas.StrokeLineCap = LineCap.Round;
			canvas.StrokeLineJoin = LineJoin.Round;
			if (owner.Filled)
			{
				canvas.FillColor = owner.Color;
				canvas.FillPath(path);
			}
			canvas.DrawPath(path);
			canvas.RestoreState();
		}

		static PathF? Get(string name)
		{
			if (string.IsNullOrEmpty(name) || !IconData.Paths.TryGetValue(name, out var data)) return null;
			if (Cache.TryGetValue(name, out var cached)) return cached;

			var path = new PathF();
			var t = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			float F(int i) => float.Parse(t[i], System.Globalization.CultureInfo.InvariantCulture);
			for (var i = 0; i < t.Length;)
			{
				switch (t[i])
				{
					case "M": path.MoveTo(F(i + 1), F(i + 2)); i += 3; break;
					case "L": path.LineTo(F(i + 1), F(i + 2)); i += 3; break;
					case "C": path.CurveTo(F(i + 1), F(i + 2), F(i + 3), F(i + 4), F(i + 5), F(i + 6)); i += 7; break;
					case "Z": path.Close(); i++; break;
					default: i++; break;
				}
			}
			return Cache[name] = path;
		}
	}
}
