using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Kardamon.Controls;

public partial class FlowView : SelectingItemsControl
{
    private StackPanel _previousStack;
    private StackPanel _nextStack;
    private Grid _selected;
    private List<ContentControl> _items = new List<ContentControl>();
    

    public static readonly StyledProperty<double> ItemSpacingProperty = AvaloniaProperty.Register<FlowView, double>(
        nameof(ItemSpacing));

    public static readonly StyledProperty<double> ItemScaleProperty = AvaloniaProperty.Register<FlowView, double>(
        nameof(ItemScale));

    public static readonly StyledProperty<double> SelectedItemScaleProperty = AvaloniaProperty.Register<FlowView, double>(
        nameof(SelectedItemScale));

    public static readonly StyledProperty<double> DistanceCenterProperty = AvaloniaProperty.Register<FlowView, double>(
        nameof(DistanceCenter));

    public static readonly StyledProperty<double> ItemRotationAngleProperty = AvaloniaProperty.Register<FlowView, double>(
        nameof(ItemRotationAngle));

    public static readonly StyledProperty<double> ItemRotationDepthProperty = AvaloniaProperty.Register<FlowView, double>(
        nameof(ItemRotationDepth));

    public double ItemRotationDepth
    {
        get => GetValue(ItemRotationDepthProperty);
        set => SetValue(ItemRotationDepthProperty, value);
    }

    public double ItemRotationAngle
    {
        get => GetValue(ItemRotationAngleProperty);
        set => SetValue(ItemRotationAngleProperty, value);
    }

    public double DistanceCenter
    {
        get => GetValue(DistanceCenterProperty);
        set => SetValue(DistanceCenterProperty, value);
    }

    public double SelectedItemScale
    {
        get => GetValue(SelectedItemScaleProperty);
        set => SetValue(SelectedItemScaleProperty, value);
    }

    public double ItemScale
    {
        get => GetValue(ItemScaleProperty);
        set => SetValue(ItemScaleProperty, value);
    }

    public double ItemSpacing
    {
        get => GetValue(ItemSpacingProperty);
        set => SetValue(ItemSpacingProperty, value);
    }
    
    public FlowView()
    {
        InitializeComponent();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
        _previousStack = e.NameScope.Find<StackPanel>("PreviousStack");
        _nextStack = e.NameScope.Find<StackPanel>("NextStack");
        _selected = e.NameScope.Find<Grid>("Selected");
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property.Name == nameof(ItemsSource))
        {
            SelectedIndex = 0;
            
        }
        else if (change.Property.Name == nameof(SelectedItem))
        {
            if (_previousStack != null)
            {
                _previousStack.Children.Clear();
                _nextStack.Children.Clear();
                int previousIndex = 0;
                int nextIndex = 0;

                if (ItemsSource != null)
                {
                    foreach (var item in ItemsSource)
                    {
                        var nextItemControl = new ContentControl()
                        {
                            Content = item,
                            VerticalAlignment = VerticalAlignment.Center,
                            VerticalContentAlignment = VerticalAlignment.Center,
                            ContentTemplate = ItemTemplate,
                        };
                        _items.Add(nextItemControl);
                        /*if (Items.IndexOf(item) < SelectedIndex)
                    {
                        if (item is Control c)
                        {
                            _previousStack.Children.Add(c);
                        }
                        else
                        {
                            var previousItemControl = new ContentControl()
                            {
                                Content = item,
                                ContentTemplate = ItemTemplate,
                                ZIndex = previousIndex++
                            };
                            _previousStack.Children.Insert(0, previousItemControl);
                        }
                    }
                    else if (Items.IndexOf(item) > SelectedIndex)
                    {
                        if (item is Control c)
                        {
                            _nextStack.Children.Add(c);
                        }
                        else
                        {
                            var nextItemControl = new ContentControl()
                            {
                                Content = item,
                                ContentTemplate = ItemTemplate,

                            };
                            _nextStack.Children.Add(nextItemControl);
                        }
                    }
                    else
                    {
                        var selectedItemControl = new ContentControl()
                        {
                            Content = item,
                            ContentTemplate = ItemTemplate,

                        };
                        _selected.Children.Clear();
                        _selected.Children.Insert(0, selectedItemControl);
                    }*/
                    }

                    foreach (var control in _items)
                    {
                        if (_items.IndexOf(control) == SelectedIndex)
                        {
                            _selected.Children.Clear();
                            control.ClipToBounds = false;
                            _selected.Children.Add(control);
                        }
                        else if (_items.IndexOf(control) < SelectedIndex)
                        {
                            if (_selected.Children.Contains(control))
                                _selected.Children.Remove(control);
                            control.ZIndex = previousIndex++;
                            _previousStack.Children.Insert(0, control);
                        }
                        else if (_items.IndexOf(control) > SelectedIndex)
                        {
                            if (_selected.Children.Contains(control))
                                _selected.Children.Remove(control);
                            control.ZIndex = nextIndex++;
                            _nextStack.Children.Add(control);
                        }
                    }

                    foreach (var control in _nextStack.Children.Reverse())
                    {
                        control.ZIndex = nextIndex++;
                    }
                }
            }
        }
    }
}