﻿#region Using Directives

using System;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace LabelPlus
{
    public class WorkspaceControlAdpter
    {

        #region Fields

        ToolStripButton editlabelbutton; 
        ToolStripComboBox combo;
        TextBox textbox;       
        PicView picview;        
        GroupBox textboxgroupbox;
        ContextMenuStrip menuquicktext;

        ListViewAdpter listviewapt;
        GroupButtonAdaptor groupbuttons;
        ToolStrip toolstrip;
        Workspace wsp;

        bool editLabelMode = false;
        int itemIndex = -1;
        string fileName = "";

        #endregion

        #region Properties
        public string FileName { get { return fileName; } }
        public int ItemIndex { get { return itemIndex; } }
        //public int NewLabelCcategory { 
        //    get { return newLabelCcategory; }
        //    set { newLabelCcategory = value; } 
        //}
        #endregion

        #region Methods
        public void page_left()
        {
            try
            {
                if (combo.SelectedIndex != 0)
                    combo.SelectedIndex--;
            }
            catch { }
        }
        public void page_right()
        {
            try
            {
                if (combo.SelectedIndex !=
                    combo.Items.Count - 1)
                    combo.SelectedIndex++;
            }
            catch { }
        }
        public void NewFile()
        {
            fileName = "";
            picview.Image = null;
            picview.LoadImage(Application.StartupPath + "\\default_image.png");
            textboxgroupbox.Text = "";
            setTextboxText("");
        }
        
        private void refreshListViewAdaptor()
        {
            listviewapt.ReloadItems(wsp.Store[fileName]);
        }
        private void setTextboxText(string text)
        {
            textbox.TextChanged -= textbox_TextChanged;

            textbox.Text = text;
            textbox.SelectionLength = 0;
            textbox.SelectionStart = textbox.Text.Length;

            textbox.TextChanged += new EventHandler(textbox_TextChanged);
        }

        private void picView_UserClickAction(object sender, PicView.LabelUserActionEventArgs e)
        {
            bool ctrlBePush = editLabelMode || Control.ModifierKeys == Keys.Control ;

            switch (e.Type) { 
                case PicView.LabelUserActionEventArgs.ClickType.left:
                    if (ctrlBePush)
                    {
                        //add
                        wsp.Store.AddLabelItem(FileName,
                            new LabelItem(e.X_percent, e.Y_percent, "", groupbuttons.SelectIndex + 1),
                            listviewapt.Count);

                        listviewapt.SelectedIndex = listviewapt.Count -1;
                    }
                    else 
                    { 
                        //normal click
                        if (e.Index == -1)
                            return;

                        listviewapt.SelectedIndex = e.Index;
                        textbox.Focus();
                    }
                    break;
                case PicView.LabelUserActionEventArgs.ClickType.right:
                    if (ctrlBePush)
                    {
                        //del
                        wsp.Store.DelLabelItem(FileName, e.Index);

                        listviewapt.SelectedIndex = -1;
                    }
                    break;
            }
        } 
 
        private void picView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                page_left();
            else if (e.KeyCode == Keys.Right)
                page_right();
            else if(e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9){
                //SetCategoryButton_Click(categorybutton1,null);

                int index = e.KeyCode - Keys.D1;
                if (index <= wsp.GroupDefine.UserGroupCount)
                {
                    groupbuttons.SelectIndex = index;
                }
            }

            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.Tab)  
                //Ctrl+Tab
                page_right();               
            
        }

        private void listViewSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listviewapt.SelectedIndex == -1)
                {
                    //did not select item
                    setTextboxText("");
                    itemIndex = -1;
                    textboxgroupbox.Text = "";
                }
                else
                {
                    itemIndex = listviewapt.SelectedIndex;
                    setTextboxText(wsp.Store[fileName][itemIndex].Text);
                    textboxgroupbox.Text = (itemIndex + 1).ToString();
                }
            }
            catch { }
        }

        private void labelItemTextChanged(object sender, EventArgs e)
        {
            try
            {
                listviewapt.ReloadItems(wsp.Store[fileName]);
            }
            catch { }
        }
        private void labelItemListChanged(object sender, EventArgs e)
        {
            try
            {
                if (wsp.Store.Filenames.Contains(fileName))
                {
                    listviewapt.ReloadItems(wsp.Store[fileName]);
                    picview.SetLabels(wsp.Store[fileName], wsp.GroupDefine.GetColors());
                    listviewapt.SelectedIndex = -1;
                }
                else {
                    listviewapt.ReloadItems(null);
                    picview.SetLabels(null,null);                    
                }
            }
            catch { }
        }        
        private void fileListChanged(object sender, EventArgs e)
        {
            try
            {
                //Set comboo items

                combo.SelectedIndexChanged -= comboSelectedIndexChanged;

                string beforeFile = combo.Text;

                combo.Items.Clear();

                var keys = wsp.Store.Filenames;
                if (keys != null)
                {
                    foreach (string name in keys)
                    {
                        combo.Items.Add(name);
                    }
                }

                int n = combo.FindStringExact(beforeFile);
                if (n != -1)
                {
                    combo.SelectedIndex = n;
                }

                combo.SelectedIndexChanged += comboSelectedIndexChanged;
            }
            catch { }
        }

        private void textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if(e.KeyCode == Keys.Enter){
                    //Ctrl+enter
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }else if(e.KeyCode == Keys.Up){
                    //Ctrl+up
                    listviewapt.SelectedIndex--;
                }
                else if (e.KeyCode == Keys.Down) {
                    //Ctrl+down
                    listviewapt.SelectedIndex++;
                }
                else if (e.KeyCode == Keys.Left) {
                    //Ctrl+left
                    page_left();
                }
                else if (e.KeyCode == Keys.Right) {
                    //Ctrl+right
                    page_right();
                }
            }

            if (e.Alt) {
                if (e.KeyCode == Keys.A) { 
                    //Alt+A
                    menuquicktext.Show(textbox,textbox.Location);
                    e.SuppressKeyPress = true;
                }
            }
        }    
        private void textboxPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //Ctrl+enter
                    try
                    {
                        listviewapt.SelectedIndex += 1;
                    }
                    catch
                    {
                        listviewapt.SelectedIndex = -1;
                    }
                    e.IsInputKey = true;
                }
            }
        }
        private void textbox_TextChanged(object sender, EventArgs e)
        {
            if (itemIndex < 0) {
                textbox.Text = "";
                return; 
            }
            wsp.Store.UpdateLabelItemText(fileName, itemIndex, textbox.Text);
        } 

        private void comboSelectedIndexChanged(object sender, EventArgs e)
        {
            fileName = combo.Text;
            picview.LoadImage(wsp.DirPath + @"\" + combo.Text);
            labelItemListChanged(null, null);

        }

        private void userGroupChanged(object sender, EventArgs e)
        {
            groupbuttons.Refresh(wsp.GroupDefine);
        }

        private void editLabelButton_Click(object sender, EventArgs e)
        {
            editLabelMode = editlabelbutton.Checked;
        }

        private void picView_MouseMove(object sender, MouseEventArgs e)
        {
            //鼠标样式
            if (Control.ModifierKeys == Keys.Control || editLabelMode)
            {
                picview.Cursor = Cursors.Cross;
            }
            else
            {
                picview.Cursor = Cursors.Default;
            }
        }

        private void picView_MosueClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            { 
                //中键翻页
                page_right();
            }
        }

        private void listViewUserSetCategory(object sender, ListViewAdpter.UserSetCategoryEventArgs e)
        {
            if (e.Category <= wsp.GroupDefine.UserGroupCount)
                wsp.Store.UpdateLabelCategory(fileName, e.Index, e.Category);
        }


        private void quickTextItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            textbox.AppendText(e.ClickedItem.ToolTipText);
        }

        private void quickTextClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            textbox.ImeMode = ImeMode.NoControl;
        }

        private void quickTextOpened(object sender, EventArgs e)
        {
            textbox.ImeMode = ImeMode.Off;
        }
        #endregion

        #region Constructors
        public WorkspaceControlAdpter(ToolStripButton toolStripButtonEditLabelMode, 
            ToolStripComboBox FileSelectComboBox, 
            TextBox TranslateTextBox, 
            GroupBox TextBoxGroupBox,
            ListViewAdpter LabelListViewAPT, 
            PicView picView, 
            ContextMenuStrip contextMenuQuictText,
            ToolStrip toolStrip,
            Workspace workspace)
        {

            wsp = workspace;
            wsp.UserGroupDefineChanged += new EventHandler(userGroupChanged);

            LabelFileManager.FileListChanged += new EventHandler(fileListChanged);
            LabelFileManager.LabelItemListChanged += new EventHandler(labelItemListChanged);
            LabelFileManager.LabelItemTextChanged += new EventHandler(labelItemTextChanged);
            LabelFileManager.GroupListChanged += new EventHandler(labelItemTextChanged);
            textboxgroupbox = TextBoxGroupBox;

            picview = picView;
            picview.Image = null;
            picview.Refresh();
            //picview.LabelUserAddAction += new PicView.UserActionEventHandler(picView_UserActionEventAdd);
            //picview.LabelUserDelAction += new PicView.UserActionEventHandler(picView_UserActionEventDel);            
            picview.LabelUserClickAction += new PicView.UserActionEventHandler(picView_UserClickAction);
            picView.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(picView_PreviewKeyDown);
            picview.MouseMove += new MouseEventHandler(picView_MouseMove);
            picview.MouseClick += new MouseEventHandler(picView_MosueClick);
            
            combo = FileSelectComboBox;
            combo.Items.Clear();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.SelectedIndexChanged += new EventHandler(comboSelectedIndexChanged);

            textbox = TranslateTextBox;
            textbox.PreviewKeyDown += new PreviewKeyDownEventHandler(textboxPreviewKeyDown);
            textbox.KeyDown += new KeyEventHandler(textbox_KeyDown);
            textbox.TextChanged += new EventHandler(textbox_TextChanged);

            listviewapt = LabelListViewAPT;
            listviewapt.ListViewSelectedIndexChanged += new EventHandler(listViewSelectedIndexChanged);
            listviewapt.UserSetCategory += new ListViewAdpter.UserActionEventHandler(listViewUserSetCategory);

            editlabelbutton = toolStripButtonEditLabelMode;
            editlabelbutton.Click += new EventHandler(editLabelButton_Click);

            menuquicktext = contextMenuQuictText;
            foreach(GlobalVar.QuickTextItem item in GlobalVar.QuickTextItems){
                string menuItemStr = item.Text + "(&" + item.Key + ")";
                menuquicktext.Items.Add(menuItemStr).ToolTipText = item.Text;

            }
            menuquicktext.ItemClicked += new ToolStripItemClickedEventHandler(quickTextItemClicked);
            menuquicktext.Opened += new EventHandler(quickTextOpened);
            menuquicktext.Closed += new  ToolStripDropDownClosedEventHandler(quickTextClosed);

            groupbuttons = new GroupButtonAdaptor(toolStrip, wsp.GroupDefine);

            toolstrip = toolStrip;
            NewFile();
        }

        #endregion

    }
}
