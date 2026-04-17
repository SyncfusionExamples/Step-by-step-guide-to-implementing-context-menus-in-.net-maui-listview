# Step-by-step-guide-to-implementing-context-menus-in-.net-maui-listview

This example demonstrates a step‑by‑step guide to implementing context menus in a .NET MAUI ListView.

## Sample

```xaml
<ContentPage.Resources>
        <local:FavoriteConverter x:Key="favoriteIconConverter" />
        <local:FavoriteColorConverter x:Key="favoriteIconColorConverter" />
        <local:TextOpacityConverter x:Key="textOpacityConverter" />

        <popup:SfPopup
            x:Key="contextMenu"
            x:Name="contextMenuPopup"
            HeightRequest="225"
            ShowHeader="False"
            WidthRequest="250">
            <popup:SfPopup.ContentTemplate>
                <DataTemplate>
                    <syncfusion:SfListView
                        Margin="10"
                        ItemSize="40"
                        ItemsSource="{Binding ContextMenuActions}"
                        TapCommand="{Binding ContextMenuCommand}">
                        <syncfusion:SfListView.ItemTemplate>
                            <DataTemplate>
                                <Grid>
                                    <Grid.ColumnDefinitions>
                                        <ColumnDefinition Width="30" />
                                        <ColumnDefinition Width="Auto" />
                                    </Grid.ColumnDefinitions>
                                    <Label
                                        Grid.Column="0"
                                        Margin="5"
                                        FontAttributes="Bold"
                                        FontFamily="MauiSampleFontIcon"
                                        FontSize="18"
                                        Text="{Binding ActionIcon}"
                                        VerticalOptions="Center"
                                        VerticalTextAlignment="Center" />
                                    <Label
                                        x:Name="label"
                                        Grid.Column="1"
                                        FontSize="16"
                                        Text="{Binding ActionName}"
                                        VerticalTextAlignment="Center" />
                                </Grid>
                            </DataTemplate>
                        </syncfusion:SfListView.ItemTemplate>
                    </syncfusion:SfListView>
                </DataTemplate>
            </popup:SfPopup.ContentTemplate>
            <popup:SfPopup.PopupStyle>
                <popup:PopupStyle CornerRadius="0" />
            </popup:SfPopup.PopupStyle>
        </popup:SfPopup>
    </ContentPage.Resources>

    <ContentPage.BindingContext>
        <local:ViewModel ContextMenuPopup="{StaticResource contextMenu}" />
    </ContentPage.BindingContext>

    <Grid x:Name="mainGrid">
        <Grid.RowDefinitions>
            <RowDefinition Height="40" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
        <Label
            Grid.Row="0"
            Margin="16,0,0,0"
            FontFamily="Roboto-Medium"
            FontSize="18"
            Text="Inbox"
            VerticalOptions="Center" />

        <syncfusion:SfListView
            x:Name="listView"
            Grid.Row="1"
            AutoFitMode="Height"
            ItemsSource="{Binding InboxInfo}"
            LongPressCommand="{Binding LongPressCommand}"
            RightTapCommand="{Binding RightTappedCommand}"
            SelectionMode="Single">

            <syncfusion:SfListView.ItemTemplate>
                <DataTemplate>
                    ...
                </DataTemplate>
            </syncfusion:SfListView.ItemTemplate>
        </syncfusion:SfListView>

        <Frame
            Grid.Row="1"
            Margin="16,0,16,5"
            Padding="0"
            CornerRadius="4"
            HeightRequest="40"
            IsVisible="{Binding IsDeleted}"
            VerticalOptions="End">
            <Grid BackgroundColor="#3D454A" HeightRequest="40">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*" />
                    <ColumnDefinition Width="Auto" />
                </Grid.ColumnDefinitions>
                <Label
                    Grid.Column="0"
                    Margin="16,0,0,0"
                    CharacterSpacing="0.25"
                    FontSize="14"
                    Text="{Binding PopUpText}"
                    TextColor="#E6E1E5"
                    VerticalOptions="Center"
                    VerticalTextAlignment="Center" />
                <Label
                    Grid.Column="1"
                    Margin="0,0,16,0"
                    CharacterSpacing="0.25"
                    FontAttributes="Bold"
                    FontSize="14"
                    HorizontalOptions="End"
                    Text="Undo"
                    TextColor="#E6E1E5"
                    VerticalOptions="Center"
                    VerticalTextAlignment="Center">
                    <Label.GestureRecognizers>
                        <TapGestureRecognizer Command="{Binding UndoCommand}" />
                    </Label.GestureRecognizers>
                </Label>
            </Grid>
        </Frame>
    </Grid>
```

```c#
public class FavoriteConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value)
            return "\ue7CF";
        else
            return "\ue73A";
    }
}

public class FavoriteColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value)
            return Color.FromArgb("#F9BC16");
        else
            return Color.FromArgb("#666666");
    }
}

public class TextOpacityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
        {
            if ((bool)value)
                return 0.8;
            else
                return 1;
        }
        
        return 1;
    }
}
```

## Requirements to run the demo

* [Visual Studio 2017](https://visualstudio.microsoft.com/downloads/) or [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/)
* Xamarin add-ons for Visual Studio (available via the Visual Studio installer).

## Troubleshooting

### Path too long exception

If you are facing path too long exception when building this example project, close Visual Studio and rename the repository to short and build the project.
