﻿using System;
using System.Web.UI;

namespace Digital_School.User_Control
{
	public partial class Tile : UserControl
	{
		public event EventHandler TitleClick;

		public string WidthClass {
			get { return divwidth.Attributes["class"]; }
			set { divwidth.Attributes["class"] = value; }
		}
		public string Title {
			get { return heading.InnerText; }
			set { heading.InnerText = value; }
		}


		public string Detail {
			get { return body.InnerText; }
			set { body.InnerText = value; }
		}

		public string Footer {
			get { return footer.InnerText; }
			set { footer.InnerText = value; }
		}

		public int PostID {
			get { return int.Parse(hfPostID.Value); }
			set { hfPostID.Value = value.ToString(); }
		}

		public int Type {
			get { return int.Parse(hfType.Value); }
			set { hfType.Value = value.ToString(); }
		}

		public string CssClass {
			get { return panel.Attributes["class"]; }
			set { panel.Attributes["class"] = value; }
		}

		protected void heading_Click(object sender, EventArgs e) {
			OnTitleClick(new EventArgs());
		}

		protected virtual void OnTitleClick(EventArgs e) {
			TitleClick?.Invoke(this, e);
		}
	}
}