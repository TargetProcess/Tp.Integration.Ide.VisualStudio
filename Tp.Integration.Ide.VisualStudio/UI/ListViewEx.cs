// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tp.Integration.Ide.VisualStudio.UI
{
	public partial class ListViewEx : ListView
	{
		private const int PADDING = 2;

		private const int WM_HSCROLL = 0x114;
		private const int WM_VSCROLL = 0x115;

		public ListViewEx()
		{
			InitializeComponent();
		}

		protected override void WndProc(ref Message msg)
		{
			// Look for the WM_VSCROLL or the WM_HSCROLL messages.
			if ((msg.Msg == WM_VSCROLL) || (msg.Msg == WM_HSCROLL))
			{
				// Move focus to the ListView to cause ComboBox to lose focus.
				Focus();
			}

			// Pass message to default handler.
			base.WndProc(ref msg);
		}

		protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
			e.DrawDefault = true;
			base.OnDrawColumnHeader(e);
		}

		protected override void OnDrawItem(DrawListViewItemEventArgs eventArgs)
		{
			if (eventArgs.State != 0)
			{
				using (var brush = new LinearGradientBrush(
					eventArgs.Bounds, Color.AliceBlue, Color.CornflowerBlue, LinearGradientMode.Horizontal))
				{
					// Draw the background for a selected item.
					eventArgs.Graphics.FillRectangle(brush, eventArgs.Bounds);
					var highlightBrush = new SolidBrush(SystemColors.Highlight);
					var iconRect = eventArgs.Item.GetBounds(ItemBoundsPortion.Icon);
					var iconsSpacerRect = GetIconsSpacerBounds(eventArgs.Item);
					// Draw selection rectangle for an item if item is selected.
					if (eventArgs.Item.Selected)
					{
						var selectRect = new Rectangle(eventArgs.Bounds.X, eventArgs.Bounds.Y,
							iconsSpacerRect.X - eventArgs.Bounds.X, eventArgs.Bounds.Height);
						if (selectRect.Width != 0)
							eventArgs.Graphics.FillRectangle(highlightBrush, selectRect);
						selectRect = new Rectangle(iconsSpacerRect.X + iconsSpacerRect.Width, eventArgs.Bounds.Y,
							eventArgs.Bounds.Width - iconsSpacerRect.X - iconsSpacerRect.Width, eventArgs.Bounds.Height);
						if (selectRect.Width != 0)
							eventArgs.Graphics.FillRectangle(highlightBrush, selectRect);
					}
					// Draw focus rectangle for an item if item is focused.
					if (eventArgs.Item.Focused)
						ControlPaint.DrawFocusRectangle(eventArgs.Graphics, eventArgs.Bounds);
					// Draw the image for an item if there is one.
					if (eventArgs.Item.ImageList != null)
					{
						var itemImage = eventArgs.Item.ImageList.Images[eventArgs.Item.ImageIndex];
						var sourceRect = new Rectangle(0, 0, itemImage.Width, itemImage.Height);
						var destinationRect = new Rectangle(iconRect.Location, sourceRect.Size);
						if ((iconsSpacerRect.Width - (iconRect.X - iconsSpacerRect.X)) < destinationRect.Width)
						{
							destinationRect.Width = iconsSpacerRect.Width - (iconRect.X - iconsSpacerRect.X);
							sourceRect.Width = iconsSpacerRect.Width - (iconRect.X - iconsSpacerRect.X);
						}
						eventArgs.Graphics.DrawImage(itemImage, destinationRect, sourceRect, GraphicsUnit.Pixel);
					}
					var labelBounds = eventArgs.Item.GetBounds(ItemBoundsPortion.Label);
					var textRect = new Rectangle(labelBounds.X, eventArgs.Bounds.Y, labelBounds.Width, eventArgs.Bounds.Height);
					TextRenderer.DrawText(eventArgs.Graphics, eventArgs.Item.Text, eventArgs.Item.Font, textRect,
						eventArgs.Item.Selected ? SystemColors.HighlightText : SystemColors.WindowText,
						GetTextFormatFlags(Columns[0]) | TextFormatFlags.EndEllipsis);
				}
			}
			base.OnDrawItem(eventArgs);
		}

		protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
		{
			if (e.ItemState != 0)
			{
				Rectangle rect = e.Bounds;
				var x = PADDING;
				var textColor = e.Item.Selected ? SystemColors.HighlightText : SystemColors.WindowText;
				if (e.ColumnIndex != 0)
				{
					var item = e.SubItem;
					if (item.GetType() == typeof(ListViewSubItemEx))
					{
						var imageitem = (ListViewSubItemEx)item;
						if (imageitem.Image != null)
						{
							var img = imageitem.Image;
							var imagy = e.Bounds.Y + ((e.Bounds.Height / 2)) - ((img.Height / 2));
							e.Graphics.DrawImage(img, e.Bounds.X + x, imagy, img.Width, img.Height);
							x = img.Width + 2;
						}
					}
					rect.Offset(x, 0);
					rect.Width -= x;
					TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.SubItem.Font, rect, textColor, GetTextFormatFlags(e.Header));
				}
			}
			base.OnDrawSubItem(e);
		}

		private static Rectangle GetIconsSpacerBounds(ListViewItem item)
		{
			if (item == null)
				return Rectangle.Empty;
			var xPosition = item.Bounds.X;
			for (var i = 1; i < item.SubItems.Count; i++)
			{
				var iPos = item.SubItems[i].Bounds.X + item.SubItems[i].Bounds.Width;
				if (item.Position.X > iPos && xPosition < iPos)
					xPosition = iPos;
			}
			var iconBounds = item.GetBounds(ItemBoundsPortion.Icon);
			var labelBounds = item.GetBounds(ItemBoundsPortion.Label);
			var width = iconBounds.X - xPosition + iconBounds.Width + (labelBounds.Width < 0 ? labelBounds.Width : 0);
			return new Rectangle(xPosition, item.Bounds.Y, width, item.Bounds.Height);
		}

		private static TextFormatFlags GetTextFormatFlags(ColumnHeader header)
		{
			var flags = TextFormatFlags.Default | TextFormatFlags.EndEllipsis;
			switch (header.TextAlign)
			{
				case HorizontalAlignment.Center:
					flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis;
					break;
				case HorizontalAlignment.Right:
					flags = TextFormatFlags.Right | TextFormatFlags.EndEllipsis;
					break;
			}
			return flags;
		}

		private void OnComboBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13 || e.KeyChar == 27)
			{
				comboBox.Hide();
				ClearComboBox();
			}
		}

		private void OnComboBoxFocusExit(object sender, EventArgs e)
		{
			comboBox.Hide();
			ClearComboBox();
		}

		public void ShowComboBoxForSubItem(ListViewItem.ListViewSubItem item)
		{
			comboBox.Size = new Size(item.Bounds.Width + 1, item.Bounds.Height - 2);
			comboBox.Location = new Point(item.Bounds.X, item.Bounds.Y - 2);
			comboBox.Show();
			comboBox.Text = item.Text;
			comboBox.SelectAll();
			comboBox.Focus();
		}

		public void AddComboBoxItem(object item)
		{
			comboBox.Items.Add(item);
		}

		public void ClearComboBox()
		{
			comboBox.Items.Clear();
		}

		private void OnComboBoxSelectedValueChanged(object sender, EventArgs e)
		{
			if (ComboBoxValueChange != null)
			{
				var i = comboBox.SelectedIndex;
				if (i >= 0 && SelectedItems.Count == 1)
				{
					var str = comboBox.Items[i].ToString();
					ComboBoxValueChange(sender, new ComboBoxEventArgs(str));
				}
			}
		}

		/// <summary>
		/// Notifies that listview combobox value has changed.
		/// </summary>
		public event EventHandler<ComboBoxEventArgs> ComboBoxValueChange;
	}

	public sealed class ComboBoxEventArgs : EventArgs
	{
		private readonly string _value;

		public ComboBoxEventArgs(string value)
		{
			_value = value;
		}

		public string Value
		{
			get { return _value; }
		}
	}

	public class ListViewSubItemEx : ListViewItem.ListViewSubItem
	{
		public ListViewSubItemEx()
		{
		}

		public ListViewSubItemEx(string text)
		{
			Text = text;
		}

		public Image Image { get; set; }

		//return the new x coordinate
		public int DrawImage(DrawListViewSubItemEventArgs e, int x)
		{
			if (Image != null)
			{
				var img = Image;
				var imagy = e.Bounds.Y + ((e.Bounds.Height / 2)) - ((img.Height / 2));
				e.Graphics.DrawImage(img, x, imagy, img.Width, img.Height);
				x += img.Width + 2;
			}
			return x;
		}
	}

	public class RankLabel
	{
		private const int HEIGHT = 11;
		private const int WIDTH = 40;
		private int _maxRank;
		private int _rank;

		public RankLabel()
		{
		}

		public RankLabel(int rankLabel, int maxRank)
		{
			_rank = rankLabel;
			_maxRank = maxRank;
		}

		public int MaxRank
		{
			set { _maxRank = value; }
			get { return _maxRank; }
		}

		public int Rank
		{
			set { _rank = value; }
			get { return _rank; }
		}

		public Image GetRankImage()
		{
			var bitmap = new Bitmap(WIDTH + 2, HEIGHT);
			var bitmapGraphics = Graphics.FromImage(bitmap);
			bitmapGraphics.DrawRectangle(Pens.Gray, 0, 0, WIDTH + 1, HEIGHT - 1);
			bitmapGraphics.FillRectangle(Brushes.White, 1, 1, WIDTH, HEIGHT - 2);

			if (_rank <= 0 || _maxRank <= 0)
				return Image.FromHbitmap(bitmap.GetHbitmap());

			var width = (WIDTH * Rank) / MaxRank;
			bitmapGraphics.FillRectangle(Brushes.GreenYellow, 1, 1, width, HEIGHT - 2);
			return Image.FromHbitmap(bitmap.GetHbitmap());
		}
	}
}