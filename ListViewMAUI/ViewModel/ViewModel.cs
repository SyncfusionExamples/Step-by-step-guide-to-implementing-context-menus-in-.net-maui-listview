using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;


namespace ListViewMAUI
{
    public class ViewModel : INotifyPropertyChanged
{
        #region Fields

    private ObservableCollection<ListViewInboxInfo>? inboxInfo;
    private ObservableCollection<ListViewInboxInfo>? archivedMessages;
    private Command? favoritesImageCommand;
    private Command? undoCommand;        
    private Command<object> longPressCommand;
    private Command<object> rightTappedCommand;
    private Command<object> contextMenuCommand;
    private bool? isDeleted;
    private ListViewInboxInfo? listViewItem;
    private int listViewItemIndex;        
    private string? popUpText;        

    public Syncfusion.Maui.Popup.SfPopup ContextMenuPopup { get; set; }
    public ObservableCollection<ContextMenuAction> ContextMenuActions { get; set; }

    #endregion

    #region Interface Member

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string name)
    {
        if (this.PropertyChanged != null)
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

    #endregion

    #region Constructor

    public ViewModel()
    {
        GenerateSource();
        DefineCommands();
        GenerateContextMenuActions();
    }

    #endregion

    #region Properties

    public ObservableCollection<ListViewInboxInfo>? InboxInfo
    {
        get { return inboxInfo; }
        set { this.inboxInfo = value; OnPropertyChanged("InboxInfo"); }
    }

    public ObservableCollection<ListViewInboxInfo>? ArchivedMessages
    {
        get { return archivedMessages; }
        set { this.archivedMessages = value; OnPropertyChanged("ArchivedMessages"); }
    }

    public Command? FavoritesImageCommand
    {
        get { return favoritesImageCommand; }
        protected set { favoritesImageCommand = value; }
    }
    public Command? UndoCommand
    {
        get { return undoCommand; }
        protected set { undoCommand = value; }
    }

    public Command<object> LongPressCommand 
    {
        get { return  longPressCommand; }
        set
        {
            longPressCommand = value;
        }
    }
    public Command<object> RightTappedCommand
    {
        get { return rightTappedCommand; }
        set
        {
            rightTappedCommand = value;
        }
    }

    public Command<object> ContextMenuCommand
    {
        get { return contextMenuCommand; }
        set
        {
            contextMenuCommand = value;
        }
    }

    public bool? IsDeleted
    {
        get { return isDeleted; }
        set { isDeleted = value; OnPropertyChanged("IsDeleted"); }
    }

    public string? PopUpText
    {
        get { return popUpText; }
        set { popUpText = value;OnPropertyChanged("PopUpText"); }
    }
            

    #endregion

    #region Generate Source

    private void GenerateSource()
    {
        this.IsDeleted = false;
        ListViewInboxInfoRepository inboxinfo = new ListViewInboxInfoRepository();
        archivedMessages = new ObservableCollection<ListViewInboxInfo>();
        inboxInfo = inboxinfo.GetInboxInfo();            

    }

    private void DefineCommands()
    {
        undoCommand = new Command(OnUndoAction);
        favoritesImageCommand = new Command(OnSetFavorites);
        LongPressCommand = new Command<object>(OnLongPress);
        rightTappedCommand = new Command<object>(OnRightTap);
        contextMenuCommand = new Command<object>(OnContextMenuClicked);
    }

    private void GenerateContextMenuActions()
    {
        ContextMenuActions = new ObservableCollection<ContextMenuAction>();
        ContextMenuActions.Add(new ContextMenuAction() { ActionName = "Copy", ActionIcon = "\uE737" });
        ContextMenuActions.Add(new ContextMenuAction() { ActionName = "Delete" , ActionIcon= "\uE73C" });
        ContextMenuActions.Add(new ContextMenuAction() { ActionName = "Archive", ActionIcon = "\uE777" });
        ContextMenuActions.Add(new ContextMenuAction() { ActionName = "Mark as Read/Unread", ActionIcon= "\uE75C" });
        ContextMenuActions.Add(new ContextMenuAction() { ActionName = "Mark as Favorite", ActionIcon= "\uE73A" });
    }

    private async void OnDelete(object item)
    {
        PopUpText = "Deleted";            
        listViewItemIndex = inboxInfo!.IndexOf(listViewItem);   
        inboxInfo!.Remove(listViewItem);
        this.IsDeleted = true;
        await Task.Delay(3000);
        this.IsDeleted = false;                    
    }

    private async void OnLongPress(object eventArgs)
    {
        var eventData = eventArgs as Syncfusion.Maui.ListView.ItemLongPressEventArgs;
        await Task.Delay(200);
        ContextMenuActions = null;
        GenerateContextMenuActions();
        listViewItem = (ListViewInboxInfo)eventData.DataItem;
        ContextMenuPopup.Show((int)eventData.Position.X, (int)eventData.Position.Y);
    }

    private async void OnRightTap(object eventArgs)
    {
        var eventData = eventArgs as Syncfusion.Maui.ListView.ItemRightTappedEventArgs;
        await Task.Delay(200);
        ContextMenuActions = null;
        GenerateContextMenuActions();
        listViewItem = (ListViewInboxInfo)eventData.DataItem;
        ContextMenuPopup.Show((int)eventData.Position.X, (int)eventData.Position.Y);
    }


    private async void OnContextMenuClicked(object eventArgs)
    {
        var eventData = eventArgs as Syncfusion.Maui.ListView.ItemTappedEventArgs;
        if (eventData == null)
            return;

        var action = (eventData.DataItem as ContextMenuAction).ActionName;
        switch(action)
        {
            case "Copy":
                OnExcuteCopyCommand(); 
                break;
            case "Delete":
                OnDelete(listViewItem);
                break;
            case "Archive":
                OnArchive(listViewItem);
                break;
            case "Mark as Read/Unread":
                listViewItem.IsOpened = !listViewItem.IsOpened;
                break;
            case "Mark as Favorite":
                OnSetFavorites(listViewItem);
                break;
        }
        ContextMenuPopup.IsOpen = false;
    }

    private void OnExcuteCopyCommand()
    {
        string text = "From" + ":" + listViewItem.ProfileName + "\n" + "Subject" + ":" + listViewItem.Subject + "\n" + "Received" + ":" + listViewItem.Date;
        text = Regex.Replace(text, "<.*?>|&nbsp;", string.Empty);
        Clipboard.SetTextAsync(text);
    }
    private async void OnArchive(object item)
    {
        PopUpText = "Archived";
        listViewItem = (ListViewInboxInfo)item;
        listViewItemIndex = inboxInfo!.IndexOf(listViewItem);
        inboxInfo!.Remove(listViewItem);
        archivedMessages!.Add(listViewItem);
        this.IsDeleted = true;
        await Task.Delay(3000);
        this.IsDeleted = false;
    }

    private void OnUndoAction()
    {
        this.IsDeleted = false;
        if (listViewItem != null)
        {
            inboxInfo!.Insert(listViewItemIndex, listViewItem);
        }
        listViewItemIndex = 0;
        listViewItem = null;
    }

    private void OnSetFavorites(object item)
    {
        var listViewItem = item as ListViewInboxInfo;
        if ((bool)listViewItem!.IsFavorite)
        {
            listViewItem.IsFavorite = false;
        }
        else
        {
            listViewItem.IsFavorite = true;
        }          
    }

    #endregion
}

    public class ContextMenuAction
    {
        public ContextMenuAction()
        {
        
        }

        public string ActionName { get; set; }

        public string ActionIcon { get; set; }
    }
}

