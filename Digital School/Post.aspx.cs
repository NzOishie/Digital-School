﻿using AspNet.Identity.MySQL;
using Digital_School.User_Control;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Digital_School
{
	public partial class Post : Page
	{
		protected void Page_Init(object sender, EventArgs e) {

			MySQLDatabase db = new MySQLDatabase();
			if (Request.QueryString["posttype"] != null) {
				ddlCatagory.SelectedValue = Request.QueryString["posttype"].ToString();
			}

			LoadPostsAccordingToDDL();

			#region Load Post Detail
			if (Request.QueryString["postid"] != null) {
				LoadPost(Convert.ToInt32(Request.QueryString["postid"]));
			} else {
				int? postId = null;
				foreach (var child in PostList.Controls) {
					if (child is PostListItem) {
						postId = (child as PostListItem).PostID;
						(child as PostListItem).SetActive();
						break;
					}
				}

				LoadPost(postId);
			}
			#endregion

			PostList.Style["max-height"] = postBody.Attributes["height"];
		}

		private void LoadPostsAccordingToDDL() {
			MySQLDatabase db = new MySQLDatabase();
			List<Dictionary<string, string>> res = null;
			if (ddlCatagory.SelectedValue == "All") {
				res = db.Query("getAllIdTitle", null, true);
			} else {
				Dictionary<string, object> dict = new Dictionary<string, object>(1);
				dict.Add("@ptype", int.Parse(ddlCatagory.SelectedValue));
				res = db.Query("getIdTitleByType", dict, true);
			}

			PostList.Controls.Clear();
			int? postId = Convert.ToInt32(Request.QueryString["postid"]);
			foreach (var item in res) {
				PostListItem post = LoadControl("~/User Control/PostListItem.ascx") as PostListItem;
				post.Title = item["title"];
				post.PostID = Convert.ToInt32(item["id"]);
				post.Body = item["date"];
				if (postId != null && postId == post.PostID)
					post.SetActive();
				post.PostClick += delegate {
					Response.Redirect(Request.Url.AbsolutePath + "?postid=" + item["id"] + "&posttype=" + ddlCatagory.SelectedValue, true);
				};
				PostList.Controls.Add(post);
			}

		}

		private void LoadPost(int? id) {
			if (id == null)
				Server.Transfer("~/Error.html", true);

			MySQLDatabase db = new MySQLDatabase();
			Dictionary<string, object> dict = new Dictionary<string, object>(1);
			dict.Add("@pid", id);
			List<Dictionary<string, string>> res = db.Query("getPostById", dict, true);
			if (res.Count > 0) {
				postTitle.InnerText = res[0]["title"];
				postBody.InnerText = res[0]["body"];
				Title = res[0]["title"];
			} else {
				Server.Transfer(Statics.Error);
			}
		}

		protected void ddlCatagory_SelectedIndexChanged(object sender, EventArgs e) {
			Response.Redirect(Request.Url.AbsolutePath + "?posttype=" + ddlCatagory.SelectedValue);
		}
	}
}