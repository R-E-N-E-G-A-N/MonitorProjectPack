using System;
using System.Linq;
using System.Windows.Forms;
using MonitorLogic;


namespace MonitorWinForms
{
    /// <summary>Главная форма.</summary>
    public class MainForm : Form
    {
        private readonly Logic _logic;
        private readonly ListView _listView;
        private readonly Button _btnAdd;
        private readonly Button _btnEdit;
        private readonly Button _btnDelete;
        private readonly Button _btnGroup;
        private readonly Button _btnWarranty;
        private readonly Button _btnRefresh;
        private readonly StatusStrip _statusStrip;
        private readonly ToolStripStatusLabel _statusLabel;

        public MainForm(Logic logic)
        {
            _logic = logic ?? throw new ArgumentNullException(nameof(logic));
            Text = "Мониторы";
            Width = 900;
            Height = 600;
            MinimumSize = new System.Drawing.Size(640, 480);
            StartPosition = FormStartPosition.CenterScreen;

            _listView = new ListView
            {
                View = View.Details,
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                MultiSelect = false
            };
            _listView.Columns.Add("Производитель", 150);
            _listView.Columns.Add("Модель", 150);
            _listView.Columns.Add("Диаг.", 60);
            _listView.Columns.Add("Разрешение", 100);
            _listView.Columns.Add("Панель", 80);
            _listView.Columns.Add("Покупка", 100);
            _listView.Columns.Add("Гарантия (мес)", 90);
            _listView.Columns.Add("Примечание", 150);
            _listView.DoubleClick += (s, e) => EditSelected();

            var panelButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5)
            };

            _btnAdd = new Button { Text = "Добавить", AutoSize = true };
            _btnEdit = new Button { Text = "Изменить", AutoSize = true };
            _btnDelete = new Button { Text = "Удалить", AutoSize = true };
            _btnGroup = new Button { Text = "Группировать по производителю", AutoSize = true };
            _btnWarranty = new Button { Text = "Просроченные по гарантии", AutoSize = true };
            _btnRefresh = new Button { Text = "Обновить", AutoSize = true };

            _btnAdd.Click += (s, e) => ShowEditDialog(null);
            _btnEdit.Click += (s, e) => EditSelected();
            _btnDelete.Click += (s, e) => DeleteSelected();
            _btnGroup.Click += (s, e) => ShowGroup();
            _btnWarranty.Click += (s, e) => ShowOutOfWarranty();
            _btnRefresh.Click += (s, e) => RefreshList();

            panelButtons.Controls.AddRange(new Control[] { _btnAdd, _btnEdit, _btnDelete, _btnGroup, _btnWarranty, _btnRefresh });

            _statusStrip = new StatusStrip();
            _statusLabel = new ToolStripStatusLabel { Text = "Готово" };
            _statusStrip.Items.Add(_statusLabel);
            _statusStrip.Dock = DockStyle.Bottom;

            // Добавляем контролы в форме. Важно: добавляем статус-стрип как Control, а не статус-лейбл.
            Controls.Add(_listView);
            Controls.Add(panelButtons);
            Controls.Add(_statusStrip);

            RefreshList();
        }

        private void RefreshList()
        {
            _listView.Items.Clear();
            var monitors = _logic.GetAllMonitors().ToList();
            foreach (var m in monitors)
            {
                var item = new ListViewItem(new[]
                {
                    m.Manufacturer,
                    m.Model,
                    m.SizeInInches.ToString("0.##"),
                    m.Resolution,
                    m.PanelType,
                    m.PurchaseDate?.ToString("yyyy-MM-dd") ?? "",
                    m.WarrantyMonths.ToString(),
                    m.Note
                })
                {
                    Tag = m
                };
                _listView.Items.Add(item);
            }
            _statusLabel.Text = $"Всего: {monitors.Count}";
        }

        private void ShowEditDialog(MonitorItem? monitor)
        {
            using var dlg = new MonitorDialog(monitor);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (monitor == null) _logic.CreateMonitor(dlg.Monitor);
                else _logic.UpdateMonitor(dlg.Monitor);
                RefreshList();
            }
        }

        private void EditSelected()
        {
            var sel = _listView.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
            if (sel == null) { MessageBox.Show("Выберите элемент."); return; }
            if (sel.Tag is MonitorItem m) ShowEditDialog(m);
        }

        private void DeleteSelected()
        {
            var sel = _listView.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
            if (sel == null) { MessageBox.Show("Выберите элемент."); return; }
            if (sel.Tag is MonitorItem m)
            {
                var answer = MessageBox.Show($"Удалить {m.Manufacturer} {m.Model}?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (answer == DialogResult.Yes)
                {
                    _logic.DeleteMonitor(m.Id);
                    RefreshList();
                }
            }
        }

        private void ShowGroup()
        {
            var groups = _logic.GroupMonitorsByManufacturer();
            var msg = string.Join(Environment.NewLine, groups.Select(g => $"{g.Key}: {g.Value.Count}"));
            MessageBox.Show(msg, "Группировка по производителю");
        }

        private void ShowOutOfWarranty()
        {
            var outOfWarranty = _logic.GetOutOfWarrantyMonitors().ToList();
            if (!outOfWarranty.Any()) MessageBox.Show("Нет мониторов с истёкшей гарантией.", "Информация");
            else MessageBox.Show(string.Join(Environment.NewLine, outOfWarranty.Select(m => m.ToString())), "Просроченные по гарантии");
        }
    }
}
