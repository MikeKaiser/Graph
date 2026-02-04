#region License
// Copyright (c) 2009 Sander van Rossen
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.IO;

namespace Graph.Items
{
	public sealed class AcceptNodeTextChangedEventArgs : CancelEventArgs
	{
		public AcceptNodeTextChangedEventArgs(string old_text, string new_text) { PreviousText = old_text; Text = new_text; }
		public AcceptNodeTextChangedEventArgs(string old_text, string new_text, bool cancel) : base(cancel) { PreviousText = old_text; Text = new_text; }
		public string			PreviousText	{ get; private set; }
		public string			Text			{ get; set; }
	}

	public sealed class NodeTextBoxItem : NodeItem
	{
		public event EventHandler<AcceptNodeTextChangedEventArgs> TextChanged;

        static Font m_font;
		public NodeTextBoxItem(string name, string text, InOutMode ioMode = InOutMode.NONE) :
			base(name, ioMode)
		{
			this.Text = text;
            this.Name = name;
            if (m_font == null)
                m_font = new Font(SystemFonts.MenuFont.OriginalFontName, SystemFonts.MenuFont.Height / 4, FontStyle.Regular);
		}

		public NodeTextBoxItem(string name, string text) :
			this(name, text, InOutMode.NONE)
		{
			this.Text = text;
            this.Name = name;
		}

		#region Text
		string internalText = string.Empty;
		public string Text
		{
			get { return internalText; }
			set
			{
				if (internalText == value)
					return;
				if (TextChanged != null)
				{
					var eventArgs = new AcceptNodeTextChangedEventArgs(internalText, value);
					TextChanged(this, eventArgs);
					if (eventArgs.Cancel)
						return;
					internalText = eventArgs.Text;
				} else
					internalText = value;
				TextSize = Size.Empty;
			}
		}
		#endregion

		internal SizeF TextSize;
        internal SizeF LabelSize;

		public override bool OnDoubleClick()
		{
			base.OnDoubleClick();
			var form = new TextEditForm();
			form.Text = Name ?? "Edit text";
			form.InputText = Text;
			var result = form.ShowDialog();
			if (result == DialogResult.OK)
				Text = form.InputText;
			return true;
		}

		internal override SizeF Measure(Graphics graphics)
		{
            SizeF labelSize;
            SizeF textSize;
			if (!string.IsNullOrWhiteSpace(this.Text))
			{
				if (this.TextSize.IsEmpty)
				{
					var size = new Size(GraphConstants.MinimumItemWidth, GraphConstants.MinimumItemHeight);

					this.TextSize = graphics.MeasureString(this.Text, SystemFonts.MenuFont, size, GraphConstants.LeftMeasureStringFormatVerticalBottom);
					
					this.TextSize.Width  = Math.Max(size.Width, this.TextSize.Width + 8);
					this.TextSize.Height = Math.Max(size.Height, this.TextSize.Height + 2);
				}
				textSize = this.TextSize;
            }
            else
            {
                textSize = new SizeF(GraphConstants.MinimumItemWidth, 0);
            }

			if (!string.IsNullOrWhiteSpace(this.Name))
			{
				if (this.LabelSize.IsEmpty)
				{
					var size = new Size(GraphConstants.MinimumItemWidth, GraphConstants.MinimumItemHeight/2);

					this.LabelSize = graphics.MeasureString(this.Name, m_font, size, GraphConstants.LeftMeasureStringFormatVerticalTop);
					
					this.LabelSize.Width  = Math.Max(size.Width, this.LabelSize.Width + 8);
					this.LabelSize.Height = Math.Max(size.Height, this.LabelSize.Height + 2);
				}
				labelSize = this.LabelSize;
			}
            else
			{
				labelSize = new SizeF(GraphConstants.MinimumItemWidth, GraphConstants.MinimumItemHeight/2);
			}

            return new SizeF( Math.Max( textSize.Width, labelSize.Width ), textSize.Height + labelSize.Height );
		}

		internal override void Render(Graphics graphics, SizeF minimumSize, PointF location)
		{
			var size = Measure(graphics);
			size.Width  = Math.Max(minimumSize.Width, size.Width);
			size.Height = Math.Max(minimumSize.Height, size.Height);

			var path = GraphRenderer.CreateRoundedRectangle(size, location);

			location.Y += 1;
			location.X += 1;

            Pen outlinePen = Pens.White;
			if ((state & RenderState.Hover) == RenderState.Hover)
			{
                outlinePen = Pens.White;
			}
            else
			{
                outlinePen = Pens.Black;
			}

			graphics.DrawPath(outlinePen, path);
			graphics.DrawString(this.Name, m_font,               Brushes.Black, new RectangleF(location, size), GraphConstants.LeftTextStringFormatVerticalTop);
			graphics.DrawString(this.Text, SystemFonts.MenuFont, Brushes.Black, new RectangleF(location, size), GraphConstants.LeftTextStringFormatVerticalBottom);
			}

        public override void WriteNodeItemData( StreamWriter file )
        {
            file.WriteLine("SET \"" + Name + "\"" + ",\"" + Text + "\"");
        }
        public override void SetNodeItemData(string val)
        {
            Text = val;
        }
        public override string GetNodeItemData()
        {
            return Text;
		}
	}
}
