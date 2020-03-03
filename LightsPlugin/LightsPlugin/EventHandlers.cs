using System;
using EXILED;
using System.Linq;

namespace Lights {
    public class EventHandlers {
        public Plugin plugin;

        public EventHandlers( Plugin plugin ) {
            this.plugin = plugin;
        }

        #region Commands

        public void OnCommand( ref RACommandEvent ev ) {
            try {
                if ( ev.Command.Contains("REQUEST_DATA PLAYER_LIST SILENT") ) return;
                string [] args = ev.Command.Split(' ');
                ReferenceHub sender = ev.Sender.SenderId == "SERVER CONSOLE" || ev.Sender.SenderId == "GAME CONSOLE" ? Plugin.GetPlayer(PlayerManager.localPlayer) : Plugin.GetPlayer(ev.Sender.SenderId);

                if ( args [ 0 ].ToLower() == "lights_reload" ) {
                    ev.Allow = false;
                    if ( !sender.CheckPermission("lights.*") || !sender.CheckPermission("lights.reload") ) {
                        ev.Sender.RAMessage(plugin.AccessDenied);
                        return;
                    }
                    plugin.reloadConfig();
                    ev.Sender.RAMessage("<color=red>Configuration variables reloaded.</color>");
                    return;
                }

                #region Command: Lights
                if ( args [ 0 ].ToLower() == plugin.CmdName || args [ 0 ].ToLower() == plugin.CmdAlias ) {
                    ev.Allow = false;
                    if ( !sender.CheckPermission("lights.*") || !sender.CheckPermission("lights.light") ) {
                        ev.Sender.RAMessage(plugin.AccessDenied);
                        return;
                    }
                    if ( args.Length < 2 ) {
                        ev.Sender.RAMessage(plugin.HelpOne.Replace("%cmd", args [ 0 ]));
                        ev.Sender.RAMessage(plugin.HelpTwo);
                        return;
                    } else if ( args.Length >= 2 ) {
                        if ( !args [ 1 ].All(char.IsDigit) ) {
                            ev.Sender.RAMessage(plugin.HelpOne.Replace("%cmd", args [ 0 ]));
                            ev.Sender.RAMessage(plugin.HelpTwo);
                            return;
                        }
                        bool OnlyHCZ = false;
                        if ( args.Length >= 3 ) {
                            string [] _t = plugin.TrueArguments.Split(',');
                            if(_t.Contains(args[2].ToLower())) OnlyHCZ = true;
                            else ev.Sender.RAMessage(plugin.NotRecognized.Replace("%arg", args [ 2 ]));
                        }
                        ev.Sender.RAMessage(plugin.Success.Replace("%s", args [ 1 ]).Replace("%value" , OnlyHCZ + ""));
                        
                        Generator079.generators [ 0 ].RpcCustomOverchargeForOurBeautifulModCreators(int.Parse(args [ 1 ]), OnlyHCZ);
                        return;
                    }
                    return;
                }
                #endregion

                //if ( args [ 0 ].ToLower() == "hptest" ) {
                //    ev.Allow = false;
                //    foreach ( ReferenceHub hub in Plugin.GetHubs() ) {
                //        hub.playerStats.health = 50;
                //        hub.playerStats.maxArtificialHealth = 200;
                //    }
                //    return;
                //}
                #region Command: HpAll
                #endregion
                return;
            } catch ( Exception e ) {
                Log.Error("Command error: " + e.StackTrace);
            }
        }
        #endregion

        public bool checkPermission( RACommandEvent ev , ReferenceHub sender , string perm) {
            if ( !sender.CheckPermission("lights.*") || !sender.CheckPermission("lights.l") ) {
                ev.Sender.RAMessage(plugin.AccessDenied);
                return false;
            }
            return true;
        }
    }
}