using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Group : System.Web.UI.Page
{


    // Right Panel GroupMeterDataSource
    private String DeleteCommandUserGroup = "DELETE FROM [UserGroup] WHERE [groupID] = {0}";
    private String DeleteCommandGroupMeter = "DELETE FROM [GroupMeter] WHERE [groupID] = {0}";
    private String InsertCommandGroupMeter = "INSERT INTO [GroupMeter] ([meterID],[groupID]) VALUES ({0},{1})";
    private String SelectCommandGroupMeter = "SELECT [Meter].[ID], [Name] FROM [GroupMeter] left join [Meter] on [Meter].[ID] = [GroupMeter].[meterID] where [GroupMeter].[groupID] = {0}";
    private String UpdateCommandGroupMeter = "";

    // Left Panel meterDataSource
    //private String DeleteCommandGroup = "DELETE FROM [Group] WHERE [ID] = @ID";
    //private String InsertCommandGroup = "INSERT INTO [Group] ([groupName], [Active]) VALUES (@groupName, @Active)";
    private String SelectCommandMeter = "SELECT [Meter].[ID], [Name] FROM [Meter] left join [GroupMeter] on [Meter].[ID] = [GroupMeter].[meterID] and [GroupMeter].[groupID] = {0} where [GroupMeter].[groupID] is NULL";
    //private String UpdateCommandGroup = "UPDATE [Group] SET [groupName] = @groupName, [Active] = @Active WHERE [ID] = @ID";

    // SELECT [ID], [Name] FROM [Meter] left join [GroupMeter] on [Meter].[ID] = [GroupMeter].[meterID] and [GroupMeter].[groupID] = {0} where [GroupMeter].[groupID] is NULL



    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void MoveItems(bool isAdd)
          {
            if (isAdd)// means if you add items to the right box
            {
                for (int i = ListBox1.Items.Count - 1; i >= 0; i--)
                {
                    if (ListBox1.Items[i].Selected)
                    {
                        ListBox2.Items.Add(ListBox1.Items[i]);
                        ListBox2.ClearSelection();
                        ListBox1.Items.Remove(ListBox1.Items[i]);
                    }
                }
            }
            else // means if you remove items from the right box and add it back to the left box
            {
                for (int i = ListBox2.Items.Count - 1; i >= 0; i--)
                {
                    if (ListBox2.Items[i].Selected)
                    {
                        ListBox1.Items.Add(ListBox2.Items[i]);
                        ListBox1.ClearSelection();
                        ListBox2.Items.Remove(ListBox2.Items[i]);
                    }
                }
            }
        }
     
        private void MoveAllItems(bool isAddAll)
        {
            if (isAddAll)// means if you add ALL items to the right box
            {
                for (int i = ListBox1.Items.Count - 1; i >= 0; i--)
                {
                        ListBox2.Items.Add(ListBox1.Items[i]);
                        ListBox2.ClearSelection();
                        ListBox1.Items.Remove(ListBox1.Items[i]);
                }
            }
            else // means if you remove ALL items from the right box and add it back to the left box
            {
                for (int i = ListBox2.Items.Count - 1; i >= 0; i--)
                {
                        ListBox1.Items.Add(ListBox2.Items[i]);
                        ListBox1.ClearSelection();
                        ListBox2.Items.Remove(ListBox2.Items[i]);
                }
            }
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            MoveItems(true);// true since we add
        }
     
        protected void ButtonRemove_Click(object sender, EventArgs e)
        {
            MoveItems(false); // false since we remove
        }
     
        protected void ButtonAddAll_Click(object sender, EventArgs e)
        {
            MoveAllItems(true); // true since we add all
        }
     
        protected void ButtonRemoveAll_Click(object sender, EventArgs e)
        {
            MoveAllItems(false); // false means re remove all
        }

        protected void ButtonEdit_Click(object sender, EventArgs e)
        {
            String stuff = "";
            Panel2.Visible = true;

            var btn = (Button)sender;
            var item = (ListViewItem)btn.NamingContainer;
            Label thelabel = (Label)item.FindControl("IDLabel");
            stuff = thelabel.Text;

            Session["groupid"] = stuff;

            ListBox1.Items.Clear();
            ListBox2.Items.Clear();

            meterDataSource.SelectCommand = String.Format(SelectCommandMeter, stuff);
            GroupMeterDataSource.SelectCommand = String.Format(SelectCommandGroupMeter, stuff);
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            String stuff = "";
            Panel2.Visible = false;

            var btn = (Button)sender;
            var item = (ListViewItem)btn.NamingContainer;
            Label thelabel = (Label)item.FindControl("IDLabel");
            stuff = thelabel.Text;
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Panel2.Visible = false;
        }

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            String groupid = "";
            Panel2.Visible = false;

            var btn = (Button)sender;
            var item = (ListViewItem)btn.NamingContainer;
            Label thelabel = (Label)item.FindControl("IDLabel");
            groupid = thelabel.Text;

            GroupMeterDataSource.DeleteCommand = String.Format(DeleteCommandGroupMeter, groupid);
            GroupMeterDataSource.Delete();

            GroupMeterDataSource.DeleteCommand = String.Format(DeleteCommandUserGroup, groupid);
            GroupMeterDataSource.Delete();
        }

        protected void ButtonInsert_Click(object sender, EventArgs e)
        {

            Panel2.Visible = false;
        }

        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            String groupid = "";
            Panel2.Visible = false;

            var btn = (Button)sender;
            var item = (ListViewItem)btn.NamingContainer;
            Label thelabel = (Label)item.FindControl("IDLabel1");
            groupid = thelabel.Text;

            // Clear all for this user first.

            GroupMeterDataSource.DeleteCommand = String.Format(DeleteCommandGroupMeter, groupid);
            GroupMeterDataSource.Delete();

            // iterate through Listbox2 entries adding each for this user.

            for (int i = 0; i < ListBox2.Items.Count; i++)
            {
                GroupMeterDataSource.InsertCommand = String.Format(InsertCommandGroupMeter, ListBox2.Items[i].Value, groupid);
                GroupMeterDataSource.Insert();
            }
        }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        ListBox1.Items.Clear();
        meterDataSource.SelectCommand = String.Format(SelectCommandMeter, Session["groupid"]) + String.Format(" and Name LIKE '%{0}%'", TextBox1.Text);
    }
}