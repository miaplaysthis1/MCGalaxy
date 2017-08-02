﻿/*
Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCGalaxy)
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using MCGalaxy.Gui.Popups;

namespace MCGalaxy.Gui {
    
    public partial class PropertyWindow : Form {
        
        void LoadChatProps() {
            chat_ParseColor(ServerConfig.DefaultColor, chat_btnDefault);
            chat_ParseColor(ServerConfig.IRCColor, chat_btnIRC);
            chat_ParseColor(ServerConfig.HelpSyntaxColor, chat_btnSyntax);
            chat_ParseColor(ServerConfig.HelpDescriptionColor, chat_btnDesc);
            
            chat_txtConsole.Text = ServerConfig.ConsoleName;
            chat_cbTabRank.Checked = ServerConfig.TablistRankSorted;
            chat_cbTabLevel.Checked = !ServerConfig.TablistGlobal;
            chat_cbTabBots.Checked = ServerConfig.TablistBots;
            
            chat_txtShutdown.Text = ServerConfig.DefaultShutdownMessage;
            chat_chkCheap.Checked = ServerConfig.ShowInvincibleMessage;
            chat_txtCheap.Enabled = chat_chkCheap.Checked;
            chat_txtCheap.Text = ServerConfig.InvincibleMessage;
            chat_txtBan.Text = ServerConfig.DefaultBanMessage;
            chat_txtPromote.Text = ServerConfig.DefaultPromoteMessage;
            chat_txtDemote.Text = ServerConfig.DefaultDemoteMessage;
        }
        
        void ApplyChatProps() {
            ServerConfig.DefaultColor = Colors.Parse(chat_btnDefault.Text);
            ServerConfig.IRCColor = Colors.Parse(chat_btnIRC.Text);
            ServerConfig.HelpSyntaxColor = Colors.Parse(chat_btnSyntax.Text);
            ServerConfig.HelpDescriptionColor = Colors.Parse(chat_btnDesc.Text);
            
            ServerConfig.ConsoleName = chat_txtConsole.Text;
            ServerConfig.TablistRankSorted = chat_cbTabRank.Checked;
            ServerConfig.TablistGlobal = !chat_cbTabLevel.Checked;
            ServerConfig.TablistBots = chat_cbTabBots.Checked;
            
            ServerConfig.DefaultShutdownMessage = chat_txtShutdown.Text;
            ServerConfig.ShowInvincibleMessage = chat_chkCheap.Checked;
            ServerConfig.InvincibleMessage = chat_txtCheap.Text;
            ServerConfig.DefaultBanMessage = chat_txtBan.Text;
            ServerConfig.DefaultPromoteMessage = chat_txtPromote.Text;
            ServerConfig.DefaultDemoteMessage = chat_txtDemote.Text;
        }
        

        void chat_chkCheap_CheckedChanged(object sender, EventArgs e) {
            chat_txtCheap.Enabled = chat_chkCheap.Checked;
        }

        void chat_cmbDefault_Click(object sender, EventArgs e) {
            chat_ShowColorDialog(chat_btnDefault, "Default color");
        }

        void chat_btnIRC_Click(object sender, EventArgs e) {
            chat_ShowColorDialog(chat_btnIRC, "IRC text color");
        }
        
        void chat_btnSyntax_Click(object sender, EventArgs e) {
            chat_ShowColorDialog(chat_btnSyntax, "Help syntax color");
        }

        void chat_btnDesc_Click(object sender, EventArgs e) {
            chat_ShowColorDialog(chat_btnDesc, "Help description color");
        }
        
        
        void chat_ParseColor(string value, Button target) {
            char code = value[1];
            target.Text = Colors.Name(value);
            
            Color textCol;
            target.BackColor = ColorSelector.LookupColor(code, out textCol);
            target.ForeColor = textCol;
        }
        
        void chat_ShowColorDialog(Button target, string title) {
            string parsed = Colors.Parse(target.Text);
            char col = parsed.Length == 0 ? 'f' : parsed[1];
            
            using (ColorSelector sel = new ColorSelector(title, col)) {
                DialogResult result = sel.ShowDialog();
                if (result == DialogResult.Cancel) return;
                
                target.Text = Colors.Name(sel.ColorCode);     
                Color textCol;
                target.BackColor = ColorSelector.LookupColor(sel.ColorCode, out textCol);
                target.ForeColor = textCol;
            }
        }
    }
}
