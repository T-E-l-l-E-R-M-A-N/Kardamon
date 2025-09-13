using System.Globalization;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Styling;

namespace Kardamon.Controls;

public class Panorama : ItemsControl
{

    private ItemsPresenter _itemsPresenter;
    private double _translateValue = 0;

    
    public static readonly StyledProperty<double> TranslateValueProperty = AvaloniaProperty.Register<Panorama, double>(
        nameof(TranslateValue));

    public static readonly StyledProperty<double> OldTranslatePropertyProperty = AvaloniaProperty.Register<Panorama, double>(
        nameof(OldTranslateProperty));

    public double OldTranslateProperty
    {
        get => GetValue(OldTranslatePropertyProperty);
        set => SetValue(OldTranslatePropertyProperty, value);
    }

    public double TranslateValue
    {
        get => GetValue(TranslateValueProperty);
        set => SetValue(TranslateValueProperty, value);
    }
    
    public static readonly StyledProperty<object> TitleProperty = AvaloniaProperty.Register<Panorama, object>(
        nameof(Title));

    public static readonly StyledProperty<int> CurrentPageIndexProperty = AvaloniaProperty.Register<Panorama, int>(
        nameof(CurrentPageIndex));

    public static readonly StyledProperty<int> TotalPageCountProperty = AvaloniaProperty.Register<Panorama, int>(
        nameof(TotalPageCount));

    public int TotalPageCount
    {
        get => GetValue(TotalPageCountProperty);
        private set => SetValue(TotalPageCountProperty, value);
    }

    public int CurrentPageIndex
    {
        get => GetValue(CurrentPageIndexProperty);
        set => SetValue(CurrentPageIndexProperty, value);
    }
    

    public object Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        CurrentPageIndex = 0;
        
        _itemsPresenter = e.NameScope.Find<ItemsPresenter>("PART_ItemsPresenter");
        TotalPageCount = ItemCount;
        var right = e.NameScope.Find<Border>("Button_Right");
        var left = e.NameScope.Find<Border>("Button_Left");
        right.PointerPressed += async (sender, args) =>
        {
            Classes.Remove("animate");
            if(CurrentPageIndex < TotalPageCount-1)
            {
                CurrentPageIndex++;
                TranslateValue -= 200;
                Classes.Add("animate");
            }
        };
        left.PointerPressed += async (sender, args) =>
        {
            Classes.Remove("animate");
            if(CurrentPageIndex > 0)
            {
                var forwardAnim = new Animation()
                {
                    Duration = TimeSpan.FromSeconds(1),
                    IterationCount = IterationCount.Parse("1"),
                    FillMode = FillMode.Both,
                };
                CurrentPageIndex--;
                TranslateValue += 200;
                Classes.Add("animate");
            }
        };
    }
}