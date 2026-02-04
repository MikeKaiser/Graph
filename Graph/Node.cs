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
using System.ComponentModel;
using Graph.Items;
using System.IO;
using System.Windows.Forms;

namespace Graph
{
	public sealed class NodeEventArgs : EventArgs
	{
		public NodeEventArgs(Node node) { Node = node; }
		public Node Node { get; private set; }
	}

	public sealed class ElementEventArgs : EventArgs
	{
		public ElementEventArgs(IElement element) { Element = element; }
		public IElement Element { get; private set; }
	}

	public sealed class AcceptNodeEventArgs : CancelEventArgs
	{
		public AcceptNodeEventArgs(Node node) { Node = node; }
		public AcceptNodeEventArgs(Node node, bool cancel) : base(cancel) { Node = node; }
		public Node Node { get; private set; }
	}

	public sealed class AcceptElementLocationEventArgs : CancelEventArgs
	{
		public AcceptElementLocationEventArgs(IElement element, Point position) { Element = element; Position = position; }
		public AcceptElementLocationEventArgs(IElement element, Point position, bool cancel) : base(cancel) { Element = element; Position = position; }
		public IElement Element		{ get; private set; }
		public Point	Position	{ get; private set; }
	}

	public class Node : IElement
	{
		public string			Title			{ get { return titleItem.Title; } set { titleItem.Title = value; } }

		#region Collapsed
		internal bool			internalCollapsed;
		public bool				Collapsed		
		{ 
			get 
			{
				return (internalCollapsed && 
						((state & RenderState.DraggedOver) == 0)) ||
						nodeItems.Count == 0;
			} 
			set 
			{
				var oldValue = Collapsed;
				internalCollapsed = value;
				if (Collapsed != oldValue)
					titleItem.ForceResize();
			} 
		}
		#endregion

		public bool				HasNoItems		{ get { return nodeItems.Count == 0; } }

		public PointF			Location		{ get; set; }
		public object			                NodeTag				    { get; set; }

		public IEnumerable<NodeConnection>	Connections { get { return connections; } }
		public IEnumerable<NodeItem>		Items		{ get { return nodeItems; } }
		
		internal RectangleF		bounds;
		internal RectangleF		inputBounds;
		internal RectangleF		outputBounds;
		internal RectangleF		itemsBounds;
		internal RenderState	state			= RenderState.None;
		internal RenderState	inputState		= RenderState.None;
		internal RenderState	outputState		= RenderState.None;

		internal readonly List<NodeConnector>	inputConnectors		= new List<NodeConnector>();
		internal readonly List<NodeConnector>	outputConnectors	= new List<NodeConnector>();
		internal readonly List<NodeConnection>	connections			= new List<NodeConnection>();
		internal readonly NodeTitleItem			titleItem			= new NodeTitleItem();
		readonly List<NodeItem>					nodeItems			= new List<NodeItem>();
        private string                          m_guid              = "";
        public string                           Guid                { get { return m_guid; } set { m_guid = value; } }
        public int                              m_graphDepth        = 0;    // this is computed and used to figure out ordering information in the final CSV for easier Diffing

        public enum GUIDMode
        {
            NoGUID,
            GenerateGUID
        };

		public Node(string title, GUIDMode guidMode = Node.GUIDMode.GenerateGUID)
		{
			this.Title = title;
			titleItem.Node = this;
            if( guidMode == GUIDMode.GenerateGUID )
                NewGUID();
		}

		public void AddItem(NodeItem item)
		{
			if (nodeItems.Contains(item))
				return;
			if (item.Node != null)
				item.Node.RemoveItem(item);
			nodeItems.Add(item);
			item.Node = this;
		}

        public void AddItemTyped(NodeItem item, int type, bool vertical = false)
        {
            if (nodeItems.Contains(item))
                return;
            if (item.Node != null)
                item.Node.RemoveItem(item);
            item.SetItemType(type);
            item.SetVertical(vertical);
            nodeItems.Add(item);
            item.Node = this;
        }

		public void RemoveItem(NodeItem item)
		{
			if (!nodeItems.Contains(item))
				return;
			item.Node = null;
			nodeItems.Remove(item);
		}
		
		// Returns true if there are some connections that aren't connected
		public bool AnyConnectorsDisconnected
		{
			get
			{
				foreach (var item in nodeItems)
				{
					if (item.Input.Enabled && !item.Input.HasConnection)
						return true;
					if (item.Output.Enabled && !item.Output.HasConnection)
						return true;
				}
				return false;
			}
		}

		// Returns true if there are some output connections that aren't connected
		public bool AnyOutputConnectorsDisconnected
		{
			get
			{
				foreach (var item in nodeItems)
					if (item.Output.Enabled && !item.Output.HasConnection)
						return true;
				return false;
			}
		}

		// Returns true if there are some input connections that aren't connected
		public bool AnyInputConnectorsDisconnected
		{
			get
			{
				foreach (var item in nodeItems)
					if (item.Input.Enabled && !item.Input.HasConnection)
						return true;
				return false;
			}
		}

		public ElementType ElementType { get { return ElementType.Node; } }

        public void SetGUID( string s )
        {
            m_guid = s;
        }

        public void NewGUID()
        {
            m_guid = System.Guid.NewGuid().ToString();
        }

        public virtual void WriteNodeData(StreamWriter file)
        {
            file.WriteLine("CREATE_NODE \"" + Title + "\",\"" + m_guid + "\",\"" + Location.X.ToString() + "\",\"" + Location.Y.ToString() + "\"");
            foreach( NodeItem i in nodeItems )
            {
                i.WriteNodeItemData(file);
            }
        }

        public virtual void GetConnectionData(ref HashSet<string> links)
        {
		    foreach( NodeConnection c in connections )
            {
                links.Add( "\""+c.From.Node.m_guid+"\",\""+c.From.Item.Name+"\""+",\""+c.To.Node.m_guid+"\",\""+c.To.Item.Name+"\"" );
            }
        }

        public static void ShowError( string message )
        {
		    DialogResult result = MessageBox.Show(message, "Error", MessageBoxButtons.OK);
        }

        public string NodeItemsAsString()
        {
            string ret = "[";
            ret += nodeItems[0];
            for( int i=1; i<nodeItems.Count; ++i )
                ret += ", " + nodeItems[i].Name;
            ret += "]";
            return ret;
        }

        public void Set( string id, string val )
        {
            NodeItem item = nodeItems.Find(x => (x.Name == id));
            if( item != null )
                item.SetNodeItemData( val );
            else
            {
                ShowError( "Unable to find id:" + id + " in " + NodeItemsAsString() );
            }
        }

        public string GetItemString( string id )
        {
            NodeItem item = nodeItems.Find(x => (x.Name == id));
            if (item != null)
                return item.GetNodeItemData();
            return "";
        }

        public int GetItemInt( string id )
        {
            NodeItem item = nodeItems.Find(x => (x.Name == id));
            if (item != null)
            {
                int ret = 0;
                int.TryParse( item.GetNodeItemData(), out ret );
                return ret;
            }
            return 0;
        }

        public NodeConnector GetOutputConnector(string id)
        {
            NodeItem item = nodeItems.Find(x => (x.Name == id));
            if( item != null)
                return item.Output;
            else
            {
                ShowError( "Unable to find id:" + id + " in " + NodeItemsAsString() );
                return null;
            }
        }

        public NodeConnector GetInputConnector(string id)
        {
            NodeItem item = nodeItems.Find(x => (x.Name == id));
            if( item != null)
                return item.Input;
            else
            {
                ShowError( "Unable to find id:" + id + " in " + NodeItemsAsString() );
                return null;
            }
        }

        public string GetInputLinkGuid(string id)
        {
            foreach (NodeConnection c in connections)
            {
                if (c.To.Node == this && c.To.Item.Name == id)
                    return c.From.Node.Guid;
            }
            return "00000000-0000-0000-0000-000000000000";
        }

        public string GetOutputLinkGuid(string id)
        {
            foreach (NodeConnection c in connections)
            {
                if (c.From.Node == this && c.From.Item.Name == id)
                    return c.To.Node.Guid;
            }
            return "00000000-0000-0000-0000-000000000000";
        }

        public Node GetOutputNode(string id)
        {
            foreach (NodeConnection c in connections)
            {
                if (c.From.Node == this && c.From.Item.Name == id)
                    return c.To.Node;
            }
            return null;
        }
	}
}
