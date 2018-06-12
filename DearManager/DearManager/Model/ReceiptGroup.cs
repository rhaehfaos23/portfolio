using System;
using System.Collections.ObjectModel;

namespace DearManager.Model
{
    class ReceiptGroup : ObservableCollection<Receipt>
    {
        public string TagTitle { get; set; }
        public DateTime Date { get; set; }
        public ReceiptGroup(string title, DateTime date)
        {
            TagTitle = title;
            Date = date;
        }
    }
}
