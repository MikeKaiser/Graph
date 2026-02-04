using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Graph;
using System.Drawing.Drawing2D;
using Graph.Compatibility;
using Graph.Items;

namespace GraphNodes
{
	public partial class ExampleForm : Form
	{
		public ExampleForm()
		{
			InitializeComponent();

			graphControl.CompatibilityStrategy = new TagTypeCompatibility();

			var someNode = new Node("My Title");
			someNode.Location = new Point(500, 100);
			var check1Item = new NodeCheckboxItem("Check1", "Check 1", NodeItem.InOutMode.INPUT) { NodeItemTag = 31337 };
			someNode.AddItem(check1Item);
			someNode.AddItem(new NodeCheckboxItem("Check2", "Check 2", NodeItem.InOutMode.INPUT) { NodeItemTag = 42f });
			
			graphControl.AddNode(someNode);

			var colorNode = new Node("Color");
			colorNode.Location = new Point(200, 50);
			var redChannel		= new NodeSliderItem("R", "R", 64.0f, 16.0f, 0, 1.0f, 0.0f, NodeItem.InOutMode.NONE);
			var greenChannel	= new NodeSliderItem("G", "G", 64.0f, 16.0f, 0, 1.0f, 0.0f, NodeItem.InOutMode.NONE);
			var blueChannel		= new NodeSliderItem("B", "B", 64.0f, 16.0f, 0, 1.0f, 0.0f, NodeItem.InOutMode.NONE);
			var colorItem		= new NodeColorItem("Color", "Color", Color.Black, NodeItem.InOutMode.OUTPUT) { NodeItemTag = 1337 };

			EventHandler<NodeItemEventArgs> channelChangedDelegate = delegate(object sender, NodeItemEventArgs args)
			{
				var red = redChannel.Value;
				var green = blueChannel.Value;
				var blue = greenChannel.Value;
				colorItem.Color = Color.FromArgb((int)Math.Round(red * 255), (int)Math.Round(green * 255), (int)Math.Round(blue * 255));
			};
			redChannel.ValueChanged		+= channelChangedDelegate;
			greenChannel.ValueChanged	+= channelChangedDelegate;
			blueChannel.ValueChanged	+= channelChangedDelegate;


			colorNode.AddItem(redChannel);
			colorNode.AddItem(greenChannel);
			colorNode.AddItem(blueChannel);

			colorItem.Clicked += new EventHandler<NodeItemEventArgs>(OnColClicked);
			colorNode.AddItem(colorItem);
			graphControl.AddNode(colorNode);

			var textureNode = new Node("Texture");
			textureNode.Location = new Point(300, 150);
			var imageItem = new NodeImageItem("Image", Properties.Resources.example, 64, 64, NodeItem.InOutMode.OUTPUT) { NodeItemTag = 1000f };
			imageItem.Clicked += new EventHandler<NodeItemEventArgs>(OnImgClicked);
			textureNode.AddItem(imageItem);
			graphControl.AddNode(textureNode);

			graphControl.ConnectionAdded	+= new EventHandler<AcceptNodeConnectionEventArgs>(OnConnectionAdded);
			graphControl.ConnectionAdding	+= new EventHandler<AcceptNodeConnectionEventArgs>(OnConnectionAdding);
			graphControl.ConnectionRemoving += new EventHandler<AcceptNodeConnectionEventArgs>(OnConnectionRemoved);
			graphControl.ShowElementMenu	+= new EventHandler<AcceptElementLocationEventArgs>(OnShowElementMenu);

			graphControl.Connect(colorItem, check1Item);
		}

		void OnImgClicked(object sender, NodeItemEventArgs e)
		{
			MessageBox.Show("IMAGE");
		}

		void OnColClicked(object sender, NodeItemEventArgs e)
		{
			MessageBox.Show("Color");
		}

		void OnConnectionRemoved(object sender, AcceptNodeConnectionEventArgs e)
		{
			//e.Cancel = true;
		}

		void OnShowElementMenu(object sender, AcceptElementLocationEventArgs e)
		{
			if (e.Element == null)
			{
				// Show a test menu for when you click on nothing
				testMenuItem.Text = "(clicked on nothing)";
				nodeMenu.Show(e.Position);
				e.Cancel = false;
			} else
			if (e.Element is Node)
			{
				// Show a test menu for a node
				testMenuItem.Text = ((Node)e.Element).Title;
				nodeMenu.Show(e.Position);
				e.Cancel = false;
			} else
			if (e.Element is NodeItem)
			{
				// Show a test menu for a nodeItem
				testMenuItem.Text = e.Element.GetType().Name;
				nodeMenu.Show(e.Position);
				e.Cancel = false;
			} else
			{
				// if you don't want to show a menu for this item (but perhaps show a menu for something more higher up) 
				// then you can cancel the event
				e.Cancel = true;
			}
		}

		void OnConnectionAdding(object sender, AcceptNodeConnectionEventArgs e)
		{
			//e.Cancel = true;
		}

		static int counter = 1;
		void OnConnectionAdded(object sender, AcceptNodeConnectionEventArgs e)
		{
			//e.Cancel = true;
			e.Connection.Name = "Connection " + counter ++;
			e.Connection.DoubleClick += new EventHandler<NodeConnectionEventArgs>(OnConnectionDoubleClick);
		}

		void OnConnectionDoubleClick(object sender, NodeConnectionEventArgs e)
		{
			e.Connection.Name = "Connection " + counter++;
		}

		private void SomeNode_MouseDown(object sender, MouseEventArgs e)
		{
			var node = new Node("Some node");
			node.AddItem(new NodeLabelItem("Entry 1", "Entry 1", NodeItem.InOutMode.INPUT));
			node.AddItem(new NodeLabelItem("Entry 2", "Entry 2", NodeItem.InOutMode.INPUT));
			node.AddItem(new NodeLabelItem("Entry 3", "Entry 3", NodeItem.InOutMode.OUTPUT));
			node.AddItem(new NodeTextBoxItem("TEXTTEXT", "TEXTTEXT", NodeItem.InOutMode.OUTPUT));
			node.AddItem(new NodeDropDownItem("Items", new string[] { "1", "2", "3", "4" }, 0, NodeItem.InOutMode.NONE));
			this.DoDragDrop(node, DragDropEffects.Copy);
		}

		private void TextureNode_MouseDown(object sender, MouseEventArgs e)
		{
			var textureNode = new Node("Texture");
			textureNode.Location = new Point(300, 150);
			var imageItem = new NodeImageItem("Image", Properties.Resources.example, 64, 64, NodeItem.InOutMode.OUTPUT) { NodeItemTag = 1337 };
			imageItem.Clicked += new EventHandler<NodeItemEventArgs>(OnImgClicked);
			textureNode.AddItem(imageItem);
			this.DoDragDrop(textureNode, DragDropEffects.Copy);
		}

		private void ColorNode_MouseDown(object sender, MouseEventArgs e)
		{
			var colorNode = new Node("Color");
			colorNode.Location = new Point(200, 50);
			var redChannel = new NodeSliderItem("R", "R", 64.0f, 16.0f, 0, 1.0f, 0.0f, NodeItem.InOutMode.NONE);
			var greenChannel = new NodeSliderItem("G", "G", 64.0f, 16.0f, 0, 1.0f, 0.0f, NodeItem.InOutMode.NONE);
			var blueChannel = new NodeSliderItem("B", "B", 64.0f, 16.0f, 0, 1.0f, 0.0f, NodeItem.InOutMode.NONE);
			var colorItem = new NodeColorItem("Color", "Color", Color.Black, NodeItem.InOutMode.OUTPUT) { NodeItemTag = 1337 };

			EventHandler<NodeItemEventArgs> channelChangedDelegate = delegate(object s, NodeItemEventArgs args)
			{
				var red = redChannel.Value;
				var green = blueChannel.Value;
				var blue = greenChannel.Value;
				colorItem.Color = Color.FromArgb((int)Math.Round(red * 255), (int)Math.Round(green * 255), (int)Math.Round(blue * 255));
			};
			redChannel.ValueChanged += channelChangedDelegate;
			greenChannel.ValueChanged += channelChangedDelegate;
			blueChannel.ValueChanged += channelChangedDelegate;


			colorNode.AddItem(redChannel);
			colorNode.AddItem(greenChannel);
			colorNode.AddItem(blueChannel);

			colorItem.Clicked += new EventHandler<NodeItemEventArgs>(OnColClicked);
			colorNode.AddItem(colorItem);

			this.DoDragDrop(colorNode, DragDropEffects.Copy);
		}

		private void OnShowLabelsChanged(object sender, EventArgs e)
		{
			graphControl.ShowLabels = showLabelsCheckBox.Checked;
		}
	}
}
