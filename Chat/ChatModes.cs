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

namespace MCGalaxy {   
    public static class ChatModes {
        
        public static bool Handle(Player p, string text) {
            if (text.Length >= 2 && text[0] == '@' && text[1] == '@') {
                text = text.Remove(0, 2);
                if (text.Length < 1) { Player.Message(p, "No message entered"); return true; }
                
                Player.Message(p, "[<] Console: &f" + text);
                string name = p == null ? "(console)" : p.name;
                Server.s.Log("[>] " + name + ": " + text);
                return true;
            }
            
            if (text[0] == '@' || (p != null && p.whisper)) {
                if (text[0] == '@') text = text.Remove(0, 1).Trim();

                if (p == null || p.whisperTo == "") {
                    int pos = text.IndexOf(' ');
                    if ( pos != -1 ) {
                        string to = text.Substring(0, pos);
                        string msg = text.Substring(pos + 1);
                        HandleWhisper(p, to, msg);
                    } else {
                        Player.Message(p, "No message entered");
                    }
                } else {
                    HandleWhisper(p, p.whisperTo, text);
                }
                return true;
            }
            
            if (text[0] == '#' || (p != null && p.opchat)) {
                if (text[0] == '#') text = text.Remove(0, 1).Trim();
                MessageOps(p, text);
                return true;
            }
            if (text[0] == '+' || (p != null && p.adminchat)) {
                if (text[0] == '+') text = text.Remove(0, 1).Trim();
                MessageAdmins(p, text);
                return true;
            }
            return false;
        }
        
        public static void MessageOps(Player p, string message) {
            string displayName = p == null ? "(console)" : p.ColoredName;
            string name = p == null ? "(console)" : p.name;
            Chat.MessageOps("To Ops &f-" + displayName + "&f- " + message);
            if (p != null && p.Rank < Server.opchatperm )
                p.SendMessage("To Ops &f-" + displayName + "&f- " + message);
            
            Server.s.Log("(OPs): " + name + ": " + message);
            Server.IRC.Say(displayName + "%S: " + message, true);
        }

        public static void MessageAdmins(Player p, string message) {
            string displayName = p == null ? "(console)" : p.ColoredName;
            string name = p == null ? "(console)" : p.name;
            Chat.MessageAdmins("To Admins &f-" + displayName + "&f- " + message);
            if (p != null && p.Rank < Server.adminchatperm)
                p.SendMessage("To Admins &f-" + displayName + "&f- " + message);
            
            Server.s.Log("(Admins): " + name + ": " + message);
            Server.IRC.Say(displayName + "%S: " + message, true);
        }
        
        static void HandleWhisper(Player p, string target, string message) {
            Player who = PlayerInfo.FindMatches(p, target);
            if (who == null) return;
            if (who == p) { Player.Message(p, "Trying to talk to yourself, huh?"); return; }
            
            if (who.ignoreAll) {
                DoFakePM(p, who, message); return;
            }          
            if (p != null && who.listignored.Contains(p.name)) {
                DoFakePM(p, who, message); return;
            }
            DoPM(p, who, message);
        }
        
        static void DoFakePM(Player p, Player who, string message) {
            string name = p == null ? "(console)" : p.name;
            Server.s.Log(name + " @" + who.name + ": " + message);
            Player.Message(p, "[<] " + who.ColoredName + ": &f" + message);
        }
        
        static void DoPM(Player p, Player who, string message) {
            string name = p == null ? "(console)" : p.name;
            string fullName = p == null ? "%S(console)" : p.ColoredName;
            
            Server.s.Log(name + " @" + who.name + ": " + message);
            Player.Message(p, "[<] " + who.ColoredName + ": &f" + message);
            Player.Message(who, "&9[>] " + fullName + ": &f" + message);
        }
    }
}