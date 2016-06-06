/*
    Copyright 2011 MCForge
        
    Dual-licensed under the    Educational Community License, Version 2.0 and
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
using MCGalaxy.Drawing.Ops;

namespace MCGalaxy.Commands.Building {
    public sealed class CmdOutline : Command {
        public override string name { get { return "outline"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Building; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdOutline() { }

        public override void Use(Player p, string message) {
            string[] args = message.Split(' ');
            if (args.Length != 2) { Help(p); return; }
            CatchPos cpos = default(CatchPos);           
            
            cpos.type = DrawCmd.GetBlock(p, args[0], out cpos.newType);
            if (cpos.type == Block.Zero) return;
            cpos.newType = DrawCmd.GetBlock(p, args[1], out cpos.newExtType);
            if (cpos.newType == Block.Zero) return;

            p.blockchangeObject = cpos;
            Player.Message(p, "Place two blocks to determine the edges.");
            p.ClearBlockchange();
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }
        
        void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type, byte extType) {
            RevertAndClearState(p, x, y, z);
            CatchPos bp = (CatchPos)p.blockchangeObject;
            bp.x = x; bp.y = y; bp.z = z; p.blockchangeObject = bp;
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange2);
        }
        
        void Blockchange2(Player p, ushort x, ushort y, ushort z, byte type, byte extType) {
            RevertAndClearState(p, x, y, z);
            CatchPos cpos = (CatchPos)p.blockchangeObject;
            
            OutlineDrawOp op = new OutlineDrawOp();
            op.Type = cpos.type; op.ExtType = cpos.extType;
            op.NewType = cpos.newType; op.NewExtType = cpos.newExtType;
            if (!DrawOp.DoDrawOp(op, null, p, x, y, z, cpos.x, cpos.y, cpos.z )) 
                return;
            if (p.staticCommands) 
                p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }
        struct CatchPos { public byte type, extType, newType, newExtType; public ushort x, y, z; }

        public override void Help(Player p) {
            Player.Message(p, "/outline [type] [type2] - Outlines [type] with [type2]");
        }
    }
}
