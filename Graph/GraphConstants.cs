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

namespace Graph
{
	public static class GraphConstants
	{
		public const int MinimumItemWidth		= 64+8;
		public const int MinimumItemHeight		= 16;
		public const int TitleHeight			= 12;
		public const int ItemSpacing			= 3;
		public const int TopHeight				= 4;
		public const int BottomHeight			= 4;
		public const int CornerSize				= 4;
		public const int ConnectorSize			= 8;
		public const int HorizontalSpacing		= 2;
		public const int NodeExtraWidth			= ((int)GraphConstants.ConnectorSize + (int)GraphConstants.HorizontalSpacing) * 2;
		
		internal const TextFormatFlags TitleTextFlagsVerticalCenter	=	TextFormatFlags.ExternalLeading |
															TextFormatFlags.GlyphOverhangPadding |
															TextFormatFlags.HorizontalCenter |
															TextFormatFlags.NoClipping |
															TextFormatFlags.NoPadding |
															TextFormatFlags.NoPrefix |
															TextFormatFlags.VerticalCenter;

		internal const TextFormatFlags CenterTextFlagsVerticalCenter =	TextFormatFlags.ExternalLeading |
															TextFormatFlags.GlyphOverhangPadding |
															TextFormatFlags.HorizontalCenter |
															TextFormatFlags.NoClipping |
															TextFormatFlags.NoPadding |
															TextFormatFlags.NoPrefix |
															TextFormatFlags.VerticalCenter;

		internal const TextFormatFlags LeftTextFlagsVerticalCenter	=	TextFormatFlags.ExternalLeading |
															TextFormatFlags.GlyphOverhangPadding |
															TextFormatFlags.Left |
															TextFormatFlags.NoClipping |
															TextFormatFlags.NoPadding |
															TextFormatFlags.NoPrefix |
															TextFormatFlags.VerticalCenter;

		internal const TextFormatFlags RightTextFlagsVerticalCenter	=	TextFormatFlags.ExternalLeading |
															TextFormatFlags.GlyphOverhangPadding |
															TextFormatFlags.Right |
															TextFormatFlags.NoClipping |
															TextFormatFlags.NoPadding |
															TextFormatFlags.NoPrefix |
															TextFormatFlags.VerticalCenter;

		internal const TextFormatFlags TitleTextFlagsVerticalTop	=	TextFormatFlags.ExternalLeading |
															            TextFormatFlags.GlyphOverhangPadding |
															            TextFormatFlags.HorizontalCenter |
															            TextFormatFlags.NoClipping |
															            TextFormatFlags.NoPadding |
															            TextFormatFlags.NoPrefix |
															            TextFormatFlags.Top;

		internal const TextFormatFlags CenterTextFlagsVerticalTop =	TextFormatFlags.ExternalLeading |
															            TextFormatFlags.GlyphOverhangPadding |
															            TextFormatFlags.HorizontalCenter |
															            TextFormatFlags.NoClipping |
															            TextFormatFlags.NoPadding |
															            TextFormatFlags.NoPrefix |
															            TextFormatFlags.Top;

		internal const TextFormatFlags LeftTextFlagsVerticalTop	=	TextFormatFlags.ExternalLeading |
															            TextFormatFlags.GlyphOverhangPadding |
															            TextFormatFlags.Left |
															            TextFormatFlags.NoClipping |
															            TextFormatFlags.NoPadding |
															            TextFormatFlags.NoPrefix |
															            TextFormatFlags.Top;

		internal const TextFormatFlags RightTextFlagsVerticalTop	=	TextFormatFlags.ExternalLeading |
															            TextFormatFlags.GlyphOverhangPadding |
															            TextFormatFlags.Right |
															            TextFormatFlags.NoClipping |
															            TextFormatFlags.NoPadding |
															            TextFormatFlags.NoPrefix |
															            TextFormatFlags.Top;

		internal const TextFormatFlags TitleTextFlagsVerticalBottom	=	TextFormatFlags.ExternalLeading |
															            TextFormatFlags.GlyphOverhangPadding |
															            TextFormatFlags.HorizontalCenter |
															            TextFormatFlags.NoClipping |
															            TextFormatFlags.NoPadding |
															            TextFormatFlags.NoPrefix |
															            TextFormatFlags.Bottom;

		internal const TextFormatFlags CenterTextFlagsVerticalBottom =  TextFormatFlags.ExternalLeading |
															            TextFormatFlags.GlyphOverhangPadding |
															            TextFormatFlags.HorizontalCenter |
															            TextFormatFlags.NoClipping |
															            TextFormatFlags.NoPadding |
															            TextFormatFlags.NoPrefix |
															            TextFormatFlags.Bottom;

		internal const TextFormatFlags LeftTextFlagsVerticalBottom	=   TextFormatFlags.ExternalLeading |
															            TextFormatFlags.GlyphOverhangPadding |
															            TextFormatFlags.Left |
															            TextFormatFlags.NoClipping |
															            TextFormatFlags.NoPadding |
															            TextFormatFlags.NoPrefix |
															            TextFormatFlags.Bottom;

		internal const TextFormatFlags RightTextFlagsVerticalBottom	=	TextFormatFlags.ExternalLeading |
															            TextFormatFlags.GlyphOverhangPadding |
															            TextFormatFlags.Right |
															            TextFormatFlags.NoClipping |
															            TextFormatFlags.NoPadding |
															            TextFormatFlags.NoPrefix |
															            TextFormatFlags.Bottom;
		internal static readonly StringFormat TitleStringFormatVerticalCenter;
		internal static readonly StringFormat CenterTextStringFormatVerticalCenter;
		internal static readonly StringFormat LeftTextStringFormatVerticalCenter;
		internal static readonly StringFormat RightTextStringFormatVerticalCenter;
		internal static readonly StringFormat TitleMeasureStringFormatVerticalCenter;
		internal static readonly StringFormat CenterMeasureTextStringFormatVerticalCenter;
		internal static readonly StringFormat LeftMeasureTextStringFormatVerticalCenter;
		internal static readonly StringFormat RightMeasureTextStringFormatVerticalCenter;

		internal static readonly StringFormat LeftTextStringFormatVerticalTop;
		internal static readonly StringFormat LeftTextStringFormatVerticalBottom;
		internal static readonly StringFormat LeftMeasureStringFormatVerticalTop;
		internal static readonly StringFormat LeftMeasureStringFormatVerticalBottom;

		static GraphConstants()
		{
			var defaultFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap | StringFormatFlags.LineLimit;
			TitleStringFormatVerticalCenter							= new StringFormat(defaultFlags);
			TitleMeasureStringFormatVerticalCenter					= new StringFormat(defaultFlags);
			TitleMeasureStringFormatVerticalCenter.Alignment		=
			TitleStringFormatVerticalCenter.Alignment				= StringAlignment.Center;
			TitleMeasureStringFormatVerticalCenter.LineAlignment	= 
			TitleStringFormatVerticalCenter.LineAlignment			= StringAlignment.Center;
			TitleStringFormatVerticalCenter.Trimming				= StringTrimming.EllipsisCharacter;
			TitleMeasureStringFormatVerticalCenter.Trimming			= StringTrimming.None;

			CenterTextStringFormatVerticalCenter					= new StringFormat(defaultFlags);
			CenterMeasureTextStringFormatVerticalCenter				= new StringFormat(defaultFlags);
			CenterMeasureTextStringFormatVerticalCenter.Alignment	= 
			CenterTextStringFormatVerticalCenter.Alignment			= StringAlignment.Center;
			CenterMeasureTextStringFormatVerticalCenter.LineAlignment = 
			CenterTextStringFormatVerticalCenter.LineAlignment		= StringAlignment.Center;
			CenterTextStringFormatVerticalCenter.Trimming			= StringTrimming.EllipsisCharacter;
			CenterMeasureTextStringFormatVerticalCenter.Trimming	= StringTrimming.None;

			LeftTextStringFormatVerticalCenter						= new StringFormat(defaultFlags);
			LeftMeasureTextStringFormatVerticalCenter				= new StringFormat(defaultFlags);
			LeftMeasureTextStringFormatVerticalCenter.Alignment		= 
			LeftTextStringFormatVerticalCenter.Alignment			= StringAlignment.Near;
			LeftMeasureTextStringFormatVerticalCenter.LineAlignment	= 
			LeftTextStringFormatVerticalCenter.LineAlignment		= StringAlignment.Center;
			LeftTextStringFormatVerticalCenter.Trimming				= StringTrimming.EllipsisCharacter;
			LeftMeasureTextStringFormatVerticalCenter.Trimming		= StringTrimming.None;

			RightTextStringFormatVerticalCenter						= new StringFormat(defaultFlags);
			RightMeasureTextStringFormatVerticalCenter				= new StringFormat(defaultFlags);
			RightMeasureTextStringFormatVerticalCenter.Alignment	= 
			RightTextStringFormatVerticalCenter.Alignment			= StringAlignment.Far;
			RightMeasureTextStringFormatVerticalCenter.LineAlignment= 
			RightTextStringFormatVerticalCenter.LineAlignment		= StringAlignment.Center;
			RightTextStringFormatVerticalCenter.Trimming			= StringTrimming.EllipsisCharacter;
			RightMeasureTextStringFormatVerticalCenter.Trimming		= StringTrimming.None;

			LeftTextStringFormatVerticalTop 				        = new StringFormat(defaultFlags);
			LeftTextStringFormatVerticalTop.Alignment		        = StringAlignment.Near;
			LeftTextStringFormatVerticalTop.LineAlignment	        = StringAlignment.Near;
			LeftTextStringFormatVerticalTop.Trimming		        = StringTrimming.EllipsisCharacter;

			LeftMeasureStringFormatVerticalTop				        = new StringFormat(defaultFlags);
			LeftMeasureStringFormatVerticalTop.Alignment		    = StringAlignment.Near;
			LeftMeasureStringFormatVerticalTop.LineAlignment	    = StringAlignment.Near;
			LeftMeasureStringFormatVerticalTop.Trimming		        = StringTrimming.None;

			LeftTextStringFormatVerticalBottom 				        = new StringFormat(defaultFlags);
			LeftTextStringFormatVerticalBottom.Alignment		    = StringAlignment.Near;
			LeftTextStringFormatVerticalBottom.LineAlignment	    = StringAlignment.Far;
			LeftTextStringFormatVerticalBottom.Trimming		        = StringTrimming.EllipsisCharacter;

			LeftMeasureStringFormatVerticalBottom				    = new StringFormat(defaultFlags);
			LeftMeasureStringFormatVerticalBottom.Alignment		    = StringAlignment.Near;
			LeftMeasureStringFormatVerticalBottom.LineAlignment	    = StringAlignment.Far;
			LeftMeasureStringFormatVerticalBottom.Trimming		    = StringTrimming.None;
		}
	}
}
