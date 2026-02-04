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
using System.Drawing;
using System.IO;

namespace Graph
{
	public sealed class NodeItemEventArgs : EventArgs
	{
		public NodeItemEventArgs(NodeItem item) { Item = item; }
		public NodeItem Item { get; private set; }
	}

	public abstract class NodeItem : IElement
	{
		public enum InOutMode
		{
			NONE,
			INPUT,
			OUTPUT,
			INOUT
		};

		public NodeItem()
		{
			this.Input		= new NodeInputConnector(this, false);
			this.Output		= new NodeOutputConnector(this, false);
		}

		public NodeItem(string name, InOutMode ioMode)
		{
			this.Input		= new NodeInputConnector(this, ioMode == InOutMode.INPUT || ioMode == InOutMode.INOUT);
			this.Output		= new NodeOutputConnector(this, ioMode == InOutMode.OUTPUT || ioMode == InOutMode.INOUT);
            this.Name       = name;
		}

		public Node					Node			{ get; internal set; }
		public object				NodeItemTag		{ get; set; }
        int                         type            = 0;
        bool                        vertical        = false;
		public string               Name    		{ get; set;	}

		public NodeConnector		Input			{ get; private set; }
		public NodeConnector		Output			{ get; private set; }

		internal RectangleF			bounds;
		internal RenderState		state			= RenderState.None;

		public virtual bool			OnClick()					 { return false; }
		public virtual bool			OnDoubleClick()				 { return false; }
		public virtual bool			OnStartDrag(PointF location, out PointF original_location) { original_location = Point.Empty; return false; }
		public virtual bool			OnDrag(PointF location)		 { return false; }		
		public virtual bool			OnEndDrag() 				 { return false; }
		internal abstract SizeF		Measure(Graphics context);
		internal abstract void		Render(Graphics graphics, SizeF minimumSize, PointF position);

        public void SetItemType( int t ) { type = t; }
        public int GetItemType() { return type;  }
        public void SetVertical( bool v ) { vertical = v; }

		public ElementType ElementType { get { return ElementType.NodeItem; } }

        public virtual void WriteNodeItemData( StreamWriter file ) { }
        public virtual void SetNodeItemData(string val)
        {
            Console.WriteLine("NodeColorItem.SetNodeItemData not yet implemented.");
        }
        public virtual string GetNodeItemData()
        {
            Console.WriteLine("NodeColorItem.GetNodeItemData not yet implemented.");
            return "";
        }
	}
}
