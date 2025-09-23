using System;
using System.Windows.Forms;
using MonitorLogic;

namespace MonitorWinForms
{
    /// <summary>Диалог добавления/редактирования монитора.</summary>
    public class MonitorDialog : Form
    {
        public MonitorItem Monitor { get; private set; }

        private readonly TextBox _txtManufacturer;
        private readonly TextBox _txtModel;
        private readonly NumericUpDown _numSize;
        private readonly TextBox _txtResolution;
        private readonly TextBox _txtPanel;
        private readonly DateTimePicker _dtpPurchase;
        private readonly NumericUpDown _numWarranty;
        private readonly TextBox _txtNote;
        private readonly Button _btnOk;
        private readonly Button _btnCancel;
        private readonly CheckBox _chkHasPurchaseDate;

        public MonitorDialog(MonitorItem? monitor = null)
        {
            Monitor = monitor != null ? Clone(monitor) : new MonitorItem();

            Text = monitor == null ? "Добавить монитор" : "Редактировать монитор";
            Width = 420;
            Height = 360;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;

            var lblManufacturer = new Label { Text = "Производитель", Left = 10, Top = 10, Width = 120 };
            _txtManufacturer = new TextBox { Left = 140, Top = 10, Width = 240, Text = Monitor.Manufacturer };

            var lblModel = new Label { Text = "Модель", Left = 10, Top = 40, Width = 120 };
            _txtModel = new TextBox { Left = 140, Top = 40, Width = 240, Text = Monitor.Model };

            var lblSize = new Label { Text = "Диагональ (дюймы)", Left = 10, Top = 70, Width = 120 };
            _numSize = new NumericUpDown { Left = 140, Top = 70, Width = 100, DecimalPlaces = 1, Minimum = 1, Maximum = 100, Value = (decimal)Math.Max(1, Monitor.SizeInInches) };

            var lblResolution = new Label { Text = "Разрешение", Left = 10, Top = 100, Width = 120 };
            _txtResolution = new TextBox { Left = 140, Top = 100, Width = 240, Text = Monitor.Resolution };

            var lblPanel = new Label { Text = "Тип панели", Left = 10, Top = 130, Width = 120 };
            _txtPanel = new TextBox { Left = 140, Top = 130, Width = 240, Text = Monitor.PanelType };

            var lblPurchase = new Label { Text = "Дата покупки", Left = 10, Top = 160, Width = 120 };
            _dtpPurchase = new DateTimePicker { Left = 140, Top = 160, Width = 240, Format = DateTimePickerFormat.Short };
            _chkHasPurchaseDate = new CheckBox { Left = 140, Top = 185, Width = 240, Text = "Есть дата покупки", Checked = Monitor.PurchaseDate.HasValue };
            _dtpPurchase.Value = Monitor.PurchaseDate ?? DateTime.Today;
            _dtpPurchase.Enabled = _chkHasPurchaseDate.Checked;
            _chkHasPurchaseDate.CheckedChanged += (s, e) => _dtpPurchase.Enabled = _chkHasPurchaseDate.Checked;

            var lblWarranty = new Label { Text = "Гарантия (месяцев)", Left = 10, Top = 210, Width = 120 };
            _numWarranty = new NumericUpDown { Left = 140, Top = 210, Width = 100, Minimum = 0, Maximum = 120, Value = Monitor.WarrantyMonths };

            var lblNote = new Label { Text = "Примечание", Left = 10, Top = 240, Width = 120 };
            _txtNote = new TextBox { Left = 140, Top = 240, Width = 240, Text = Monitor.Note };

            _btnOk = new Button { Text = "ОК", Left = 200, Top = 280, Width = 80 };
            _btnCancel = new Button { Text = "Отмена", Left = 290, Top = 280, Width = 80 };

            _btnOk.Click += (s, e) => { if (ApplyChanges()) DialogResult = DialogResult.OK; };
            _btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            Controls.AddRange(new Control[] {
                lblManufacturer, _txtManufacturer, lblModel, _txtModel, lblSize, _numSize,
                lblResolution, _txtResolution, lblPanel, _txtPanel, lblPurchase, _dtpPurchase, _chkHasPurchaseDate,
                lblWarranty, _numWarranty, lblNote, _txtNote, _btnOk, _btnCancel
            });

            var tt = new ToolTip();
            tt.SetToolTip(_numSize, "Диагональ в дюймах, например 24.0");
            tt.SetToolTip(_numWarranty, "Срок гарантии в месяцах");

            _txtManufacturer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtModel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtResolution.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _dtpPurchase.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtNote.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        }

        private bool ApplyChanges()
        {
            if (string.IsNullOrWhiteSpace(_txtManufacturer.Text)) { MessageBox.Show("Производитель обязателен"); return false; }
            if (string.IsNullOrWhiteSpace(_txtModel.Text)) { MessageBox.Show("Модель обязательна"); return false; }
            Monitor.Manufacturer = _txtManufacturer.Text.Trim();
            Monitor.Model = _txtModel.Text.Trim();
            Monitor.SizeInInches = (double)_numSize.Value;
            Monitor.Resolution = _txtResolution.Text.Trim();
            Monitor.PanelType = _txtPanel.Text.Trim();
            Monitor.PurchaseDate = _chkHasPurchaseDate.Checked ? _dtpPurchase.Value.Date : null;
            Monitor.WarrantyMonths = (int)_numWarranty.Value;
            Monitor.Note = _txtNote.Text.Trim();
            return true;
        }

        private static MonitorItem Clone(MonitorItem src) => new MonitorItem
        {
            Id = src.Id,
            Manufacturer = src.Manufacturer,
            Model = src.Model,
            SizeInInches = src.SizeInInches,
            Resolution = src.Resolution,
            PanelType = src.PanelType,
            PurchaseDate = src.PurchaseDate,
            WarrantyMonths = src.WarrantyMonths,
            Note = src.Note
        };
    }
}
