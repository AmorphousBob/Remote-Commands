using Rocket.API;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using UnityEngine;
using SDG.Unturned;
using System.Collections.Generic;
using Rocket.Unturned.Items;



namespace RemoteCommands
{
    public class RemoteState : RocketPlugin
    {
        private DateTime lastCalled = DateTime.Now;
        public static List<InteractableItem> ItemsList = new List<InteractableItem>();
        public static List<ItemMagazineAsset> MagList = new List<ItemMagazineAsset>();

        public class TimedState
        {
            public UnturnedPlayer player;
            public string state;
            public DateTime ftime;
            public TimedState(UnturnedPlayer player, string state, DateTime ftime)
            {

            }
        }
        List<TimedState> Timed = new List<TimedState>();
        List<TimedState> ToDelete = new List<TimedState>();

        protected override void Load()
        {
            //On plugin start/load
            foreach (SteamPlayer sp in Provider.clients)
            {
                UnturnedPlayer player = UnturnedPlayer.FromSteamPlayer(sp);
                player.VanishMode = false;
                player.GodMode = false;
            }

        }

        protected override void Unload()
        {
            //On plugin stop/unload
        }

        public void FixedUpdate()
        {
            //Every server tick/update
            if ((DateTime.Now - this.lastCalled).TotalSeconds > 1)
            {
                /*               this.lastCalled = DateTime.Now;
                               foreach(TimedState t in Timed)
                               {
                                   if (t.state == "god" && t.player.GodMode != true)
                                       Timed.Remove(t);

                                   if (t.state == "vanish" && t.player.VanishMode != true)
                                       Timed.Remove(t);

                                   if (DateTime.Now >= t.ftime)
                                   {
                                       if (t.state == "vanish")
                                       {
                                           t.player.VanishMode = false;
                                           UnturnedChat.Say(t.player, "Vanish has been removed");
                                           Timed.Remove(t);
                                       }
                                       if (t.state == "god")
                                       {
                                           t.player.GodMode = false;
                                           UnturnedChat.Say(t.player, "God has been removed");
                                           Timed.Remove(t);
                                       }
                                   }

                               }*/
            }
        }

        /*      [RocketCommand("RemoteState", "Remotely change a player's god state and vanish state", "<player> <state> [time]", AllowedCaller.Both)]
              [RocketCommandAlias("rs")]
              public void ExecuteCommandRemoteState(IRocketPlayer caller, string[] parameters)
              {
                      //Runs on ingame execution of command
                      if (parameters.Length < 1)
                  {
                      UnturnedChat.Say(caller, "Invalid Arguments", Color.red);
                      return;
                  }
                  if (parameters.Length == 0)
                  {
                      UnturnedChat.Say(caller, "Invalid Arguments", Color.red);
                      return;
                  }
                  UnturnedPlayer unturnedPlayer2 = UnturnedPlayer.FromCSteamID(new CSteamID(Convert.ToUInt64(parameters[0])));
                  if (unturnedPlayer2 == null)
                  {
                      UnturnedChat.Say(caller, "Player not found", Color.red);
                      return;
                  }
                  if (caller is ConsolePlayer)
                 {
                      int numbertest = 0;
                      if (parameters.Length == 3)
                      {
                          if (int.TryParse(parameters[2], out numbertest))
                          {
                              double number = Convert.ToDouble(numbertest);
                              DateTime finishTime = DateTime.Now.AddMinutes(number);
                              string state1 = null;
                              string state = parameters[1].ToLower();
                              if (numbertest <= 120 && numbertest > 0)
                              {
                                  if (state == "god" || state == "g")
                                  {
                                      state1 = "god";
                                      UnturnedChat.Say(unturnedPlayer2, unturnedPlayer2 + state1 + finishTime);
                                      foreach (TimedState t in Timed)
                                      {

                                          if (t.player.CSteamID.m_SteamID == unturnedPlayer2.CSteamID.m_SteamID)
                                          {
                                              if (t.state == "god")
                                                  ToDelete.Add(t);
                                          }
                                      }
                                      UnturnedChat.Say(unturnedPlayer2, unturnedPlayer2 + state1 + finishTime);
                                      unturnedPlayer2.GodMode = true;
                                      Timed.Add(new TimedState(unturnedPlayer2, state1, finishTime));
                                      UnturnedChat.Say(caller, "Succesfully gave" + " " + unturnedPlayer2.DisplayName + " " + "god for" + " " + numbertest + " minutes");
                                      foreach (TimedState t in ToDelete)
                                      {
                                          Timed.Remove(t);
                                      }
                                      ToDelete.Clear();
                                      return;
                                  }
                                  if (parameters[1].ToLower() == "vanish" || parameters[1].ToLower() == "v")
                                  {
                                      if (state == "vanish" || state == "v")
                                      {
                                          state1 = "vanish";
                                          foreach (TimedState t in Timed)
                                          {

                                              if (t.player.CSteamID.m_SteamID == unturnedPlayer2.CSteamID.m_SteamID)
                                              {
                                                  if (t.state == "vanish")
                                                  {
                                                      ToDelete.Add(t);
                                                  }
                                              }
                                          }
                                          unturnedPlayer2.GodMode = true;
                                          Timed.Add(new TimedState(unturnedPlayer2, state1, finishTime));
                                          UnturnedChat.Say(caller, "Succesfully gave" + " " + unturnedPlayer2.DisplayName + " " + "god for" + " " + numbertest + " minutes");
                                          foreach (TimedState t in ToDelete)
                                          {
                                              Timed.Remove(t);
                                          }
                                          ToDelete.Clear();
                                          return;
                                      }
                                  }
                                  if (numbertest > 120 || numbertest < 0)
                                  {
                                      UnturnedChat.Say(caller, "Invalid Number, must be between 0-120");
                                      return;
                                  }
                              }
                          }
                      }
                          if (parameters.Length == 2)
                          {
                              if (parameters[1].ToLower() == "god")
                              {
                                  if (unturnedPlayer2.GodMode == false)
                                  {
                                      unturnedPlayer2.GodMode = true;
                                      UnturnedChat.Say(caller, "Turned on God for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                                  else
                                  {
                                      unturnedPlayer2.GodMode = false;
                                      UnturnedChat.Say(caller, "Turned off God for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                              }
                              if (parameters[1].ToLower() == "g")
                              {
                                  if (unturnedPlayer2.GodMode == false)
                                  {
                                      unturnedPlayer2.GodMode = true;
                                      UnturnedChat.Say(caller, "Turned on God for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                                  else
                                  {
                                      unturnedPlayer2.GodMode = false;
                                      UnturnedChat.Say(caller, "Turned off God for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                              }
                              if (parameters[1].ToLower() == "vanish")
                              {
                                  if (unturnedPlayer2.VanishMode == false)
                                  {
                                      unturnedPlayer2.VanishMode = true;
                                      UnturnedChat.Say(caller, "Turned on Vanish for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                                  else
                                  {
                                      unturnedPlayer2.VanishMode = false;
                                      UnturnedChat.Say(caller, "Turned off Vanish for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                              }
                              if (parameters[1].ToLower() == "v")
                              {
                                  if (unturnedPlayer2.VanishMode == false)
                                  {
                                      unturnedPlayer2.VanishMode = true;
                                      UnturnedChat.Say(caller, "Turned on Vanish for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                                  else
                                  {
                                      unturnedPlayer2.VanishMode = false;
                                      UnturnedChat.Say(caller, "Turned off Vanish for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                              }
                          }
                      }

                      if (caller is UnturnedPlayer)
                      {
                          UnturnedPlayer Unturnedplayer1 = (UnturnedPlayer)caller;


                          string perm = null;
                          perm = "god";
                          if (unturnedPlayer2.HasPermission(perm))
                          {
                              perm = "item";
                              if (Unturnedplayer1.HasPermission(perm))
                              {
                                  UnturnedChat.Say(caller, "Owner or Co-Owner confirmed: Bypassing");
                              }
                              else
                              {
                                  UnturnedChat.Say(caller, "Cannot Change State of someone with higher or equal rank", Color.red);
                                  return;
                              }
                          }

                      int numbertest = 0;
                      if (parameters.Length == 3)
                      {
                          if (int.TryParse(parameters[2], out numbertest))
                          {
                              double number = Convert.ToDouble(numbertest);
                              DateTime finishTime = DateTime.Now.AddMinutes(number);
                              string state1 = null;
                              string state = parameters[1].ToLower();
                              if (numbertest <= 120 && numbertest > 0)
                              {
                                  if (state == "god" || state == "g")
                                  {
                                      state1 = "god";
                                      foreach (TimedState t in Timed)
                                      {
                                          if (t.player.CSteamID.m_SteamID == unturnedPlayer2.CSteamID.m_SteamID)
                                          {
                                              if (t.state == "god")
                                              {
                                                  ToDelete.Add(t);
                                              }
                                          }
                                      }
                                      unturnedPlayer2.GodMode = true;
                                      Timed.Add(new TimedState(unturnedPlayer2, state1, finishTime));
                                      UnturnedChat.Say(caller, "Succesfully gave" + " " + unturnedPlayer2.DisplayName + " " + "god for" + " " + numbertest + " minutes");
                                      foreach (TimedState t in ToDelete)
                                      {
                                          Timed.Remove(t);
                                      }
                                      ToDelete.Clear();
                                      return;
                                  }
                                  if (parameters[1].ToLower() == "vanish" || parameters[1].ToLower() == "v")
                                  {
                                      if (state == "vanish" || state == "v")
                                      {
                                          state1 = "vanish";
                                          foreach (TimedState t in Timed)
                                          {
                                              if (t.player.CSteamID.m_SteamID == unturnedPlayer2.CSteamID.m_SteamID)
                                              {
                                                  if (t.state == "vanish")
                                                  {
                                                      ToDelete.Add(t);
                                                  }
                                              }
                                          }
                                          unturnedPlayer2.GodMode = true;
                                          Timed.Add(new TimedState(unturnedPlayer2, state1, finishTime));
                                          UnturnedChat.Say(caller, "Succesfully gave" + " " + unturnedPlayer2.DisplayName + " " + "god for" + " " + numbertest + " minutes");
                                          foreach (TimedState t in ToDelete)
                                          {
                                              Timed.Remove(t);
                                          }
                                          ToDelete.Clear();
                                          return;
                                      }
                                  }
                                  if (numbertest > 120 || numbertest < 0)
                                  {
                                      UnturnedChat.Say(caller, "Invalid Number, must be between 0-120");
                                      return;
                                  }
                              }
                          }
                          if (parameters.Length == 2)
                          {
                              if (parameters[1].ToLower() == "god")
                              {
                                  if (unturnedPlayer2.GodMode == false)
                                  {
                                      unturnedPlayer2.GodMode = true;
                                      UnturnedChat.Say(caller, "Turned on God for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                                  else
                                  {
                                      unturnedPlayer2.GodMode = false;
                                      UnturnedChat.Say(caller, "Turned off God for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                              }
                              if (parameters[1].ToLower() == "g")
                              {
                                  if (unturnedPlayer2.GodMode == false)
                                  {
                                      unturnedPlayer2.GodMode = true;
                                      UnturnedChat.Say(caller, "Turned on God for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                                  else
                                  {
                                      unturnedPlayer2.GodMode = false;
                                      UnturnedChat.Say(caller, "Turned off God for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                              }
                              if (parameters[1].ToLower() == "vanish")
                              {
                                  if (unturnedPlayer2.VanishMode == false)
                                  {
                                      unturnedPlayer2.VanishMode = true;
                                      UnturnedChat.Say(caller, "Turned on Vanish for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                                  else
                                  {
                                      unturnedPlayer2.VanishMode = false;
                                      UnturnedChat.Say(caller, "Turned off Vanish for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                              }
                              if (parameters[1].ToLower() == "v")
                              {
                                  if (unturnedPlayer2.VanishMode == false)
                                  {
                                      unturnedPlayer2.VanishMode = true;
                                      UnturnedChat.Say(caller, "Turned on Vanish for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                                  else
                                  {
                                      unturnedPlayer2.VanishMode = false;
                                      UnturnedChat.Say(caller, "Turned off Vanish for " + unturnedPlayer2.DisplayName);
                                      return;
                                  }
                              }
                          }
                      }
                  }
              }
              */
        [RocketCommand("God", "Changes God State", "<player> [time]", AllowedCaller.Both)]
        public void ExecuteCommandgod(IRocketPlayer caller, string[] parameters)
        {
            //Runs on ingame execution of command
            bool silent = false;
            if (parameters.Length == 2 && parameters[1].ToLower() == "silent")
                silent = true;

            if (caller is UnturnedPlayer)
            {
                UnturnedPlayer Unturnedplayer1 = (UnturnedPlayer)caller;
                if (parameters.Length == 0)
                {
                    if (Unturnedplayer1.GodMode == false)
                    {
                        UnturnedChat.Say(caller, "Enabled God", Color.red);
                        Unturnedplayer1.GodMode = true;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, "Disabled God", Color.red);
                        Unturnedplayer1.GodMode = false;
                    }
                    return;
                }
            }
            if (caller is UnturnedPlayer)
            {
                UnturnedPlayer Unturnedplayer1 = (UnturnedPlayer)caller;
                if (parameters.Length == 0)
                {
                    if (Unturnedplayer1.GodMode == false)
                    {
                        UnturnedChat.Say(caller, "Enabled God", Color.red);
                        Unturnedplayer1.GodMode = true;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, "Disabled God", Color.red);
                        Unturnedplayer1.GodMode = false;
                    }
                    return;
                }
            }
            if (caller is ConsolePlayer && parameters.Length < 1)
            {
                UnturnedChat.Say(caller, "Must have a target");
                return;
            }
            if (parameters.Length >= 1)
            {
                if (caller is UnturnedPlayer)
                {
                    //Checking if player has permission to grant god mode to other players
                    bool PermissionCheck = false;
                    PermissionCheck = caller.HasPermission("god.*");
                    UnturnedPlayer Unturnedplayer1 = (UnturnedPlayer)caller;
                    if (PermissionCheck == false)
                    {
                        if (Unturnedplayer1.GodMode == false)
                        {
                            UnturnedChat.Say(caller, "Enabled God", Color.red);
                            Unturnedplayer1.GodMode = true;
                        }
                        else
                        {
                            UnturnedChat.Say(caller, "Disabled God", Color.red);
                            Unturnedplayer1.GodMode = false;
                        }
                        return;
                    }
                }
                UnturnedPlayer unturnedPlayer2 = UnturnedPlayer.FromName(parameters[0]);
                //checking for second player, if exists or if they dont additionally stopping them from using their own name
                if (unturnedPlayer2 == null)
                {
                    UnturnedChat.Say(caller, "Player not found", Color.red);
                    return;
                }
                if (caller is UnturnedPlayer)
                {
                    UnturnedPlayer Unturnedplayer1 = (UnturnedPlayer)caller;
                    if (Unturnedplayer1.CSteamID == unturnedPlayer2.CSteamID)
                    {
                        UnturnedChat.Say(caller, "Invalid Arguments", Color.red);
                        return;
                    }
                    //Owner bypass for changing someone elses state, and block for people changing other staff states
                    string perm = null;
                    perm = "god";
                    if (unturnedPlayer2.HasPermission(perm))
                    {
                        perm = "item";
                        if (Unturnedplayer1.HasPermission(perm))
                        {
                            UnturnedChat.Say(caller, "Owner or Co-Owner confirmed: Bypassing");
                        }
                        else
                        {
                            UnturnedChat.Say(caller, "Cannot Change State of someone with higher or equal rank", Color.red);
                            return;
                        }
                    }
                }
                /*    int numbertest = 0;
                    if (parameters.Length == 2)
                    {
                        if (int.TryParse(parameters[1], out numbertest))
                        {
                            if (numbertest > 120 || numbertest < 0)
                            {
                                UnturnedChat.Say(caller, "Invalid Number, must be between 0-120");
                                return;
                            }
                            if (numbertest <= 120 && numbertest > 0)
                            {
                                double number = Convert.ToDouble(numbertest);
                                DateTime finishTime = DateTime.Now.AddMinutes(number);
                                string state1 = null;
                                    state1 = "god";
                                    foreach (TimedState t in Timed)
                                    {
                                        if (t.player.CSteamID.m_SteamID == unturnedPlayer2.CSteamID.m_SteamID)
                                        {
                                            if (t.state == "god")
                                            {
                                                ToDelete.Add(t);
                                            }
                                        }
                                    }
                                    unturnedPlayer2.GodMode = true;
                                    Timed.Add(new TimedState(unturnedPlayer2, state1, finishTime));
                                    UnturnedChat.Say(caller, "Succesfully gave" + " " + unturnedPlayer2.DisplayName + " " + "god for" + " " + numbertest + " minutes");
                                    foreach (TimedState t in ToDelete)
                                    {
                                        Timed.Remove(t);
                                    }
                                    ToDelete.Clear();
                                    return;

                            }
                        }

                    }
                    */

                if (parameters.Length > 1 && parameters[1].ToLower() != "silent")
                {
                    UnturnedChat.Say(caller, "Invalid Arguments", Color.red);
                    return;
                }
                if (parameters.Length >= 1 && parameters.Length < 3)
                {
                    //Changing the state of the player
                    if (unturnedPlayer2.GodMode == false)
                    {
                        unturnedPlayer2.GodMode = true;
                        UnturnedChat.Say(caller, "Turned on god for " + unturnedPlayer2.DisplayName);
                       if (silent == false)
                            UnturnedChat.Say(unturnedPlayer2, "God Mode has been enabled for you");
                        return;
                    }
                    else
                    {
                        unturnedPlayer2.GodMode = false;
                        UnturnedChat.Say(caller, "Turned off god for " + unturnedPlayer2.DisplayName);
                      if (silent == false)
                            UnturnedChat.Say(unturnedPlayer2, "God Mode has been disabled for you");
                        return;
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, "Invalid Arguments", Color.red);
                    return;
                }
            }
        }

        [RocketCommand("vanish", "Changes Vanish State", "<player> [time]", AllowedCaller.Both)]
        public void ExecuteCommandvanish(IRocketPlayer caller, string[] parameters)
        {
            //Runs on ingame execution of command
            //same comments just with vanish
            bool silent = false;

            if (parameters.Length == 2 && parameters[1].ToLower() == "silent")
                silent = true;
            if (caller is UnturnedPlayer)
            {
                UnturnedPlayer Unturnedplayer1 = (UnturnedPlayer)caller;
                if (parameters.Length == 0)
                {
                    if (Unturnedplayer1.VanishMode == false)
                    {
                        UnturnedChat.Say(caller, "Enabled Vanish", Color.red);
                        Unturnedplayer1.VanishMode = true;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, "Disabled Vanish", Color.red);
                        Unturnedplayer1.VanishMode = false;
                    }
                    return;
                }
            }
            if (caller is ConsolePlayer && parameters.Length == 0)
            {
                UnturnedChat.Say(caller, "Must have a target");
                return;
            }
            if (parameters.Length >= 1)
            {
                if (caller is UnturnedPlayer)
                {
                    bool PermissionCheck = false;
                    PermissionCheck = caller.HasPermission("god.*");
                    UnturnedPlayer Unturnedplayer1 = (UnturnedPlayer)caller;
                    if (PermissionCheck == false)
                    {
                        if (Unturnedplayer1.VanishMode == false)
                        {
                            UnturnedChat.Say(caller, "Enabled Vanish", Color.red);
                            Unturnedplayer1.VanishMode = true;
                        }
                        else
                        {
                            UnturnedChat.Say(caller, "Disabled Vanish", Color.red);
                            Unturnedplayer1.VanishMode = false;
                        }
                        return;
                    }
                }
                UnturnedPlayer unturnedPlayer2 = UnturnedPlayer.FromName(parameters[0]);
                if (unturnedPlayer2 == null)
                {
                    UnturnedChat.Say(caller, "Player not found", Color.red);
                    return;
                }
                if (caller is UnturnedPlayer)
                {
                    UnturnedPlayer Unturnedplayer1 = (UnturnedPlayer)caller;
                    if (Unturnedplayer1.CSteamID == unturnedPlayer2.CSteamID)
                    {
                        UnturnedChat.Say(caller, "Invalid Arguments", Color.red);
                        return;
                    }
                    string perm = null;
                    perm = "vanish";
                    if (unturnedPlayer2.HasPermission(perm))
                    {
                        perm = "item";
                        if (Unturnedplayer1.HasPermission(perm))
                        {
                            UnturnedChat.Say(caller, "Bypassing Restriction");
                        }
                        else
                        {
                            UnturnedChat.Say(caller, "Cannot Change State of someone with higher or equal rank", Color.red);
                            return;
                        }
                    }
                }
                /*      int numbertest = 0;
                      if (parameters.Length == 2)
                      {
                          if (int.TryParse(parameters[1], out numbertest))
                          {
                              if (numbertest > 120 || numbertest < 0)
                              {
                                  UnturnedChat.Say(caller, "Invalid Number, must be between 0-120");
                                  return;
                              }
                              if (numbertest <= 120 && numbertest > 0)
                              {
                                  double number = Convert.ToDouble(numbertest);
                                  DateTime finishTime = DateTime.Now.AddMinutes(number);
                                  string state1 = null;
                                  state1 = "vanish";
                                  foreach (TimedState t in Timed)
                                  {
                                      if (t.player.CSteamID.m_SteamID == unturnedPlayer2.CSteamID.m_SteamID)
                                      {
                                          if (t.state == "vanish")
                                          {
                                              ToDelete.Add(t);
                                          }
                                      }
                                  }
                                  unturnedPlayer2.VanishMode = true;
                                  Timed.Add(new TimedState(unturnedPlayer2, state1, finishTime));
                                  UnturnedChat.Say(caller, "Succesfully gave" + " " + unturnedPlayer2.DisplayName + " " + "vanish for" + " " + numbertest + " minutes");
                                  foreach (TimedState t in ToDelete)
                                  {
                                      Timed.Remove(t);
                                  }
                                  ToDelete.Clear();
                                  return;

                              }
                          }

                      }
                      */
                if (parameters.Length > 1 && parameters[1].ToLower() != "silent")
                {
                    UnturnedChat.Say(caller, "Invalid Arguments", Color.red);
                    return;
                }
                if (parameters.Length >= 1 && parameters.Length < 3)
                {
                    if (unturnedPlayer2.VanishMode == false)
                    {
                        unturnedPlayer2.VanishMode = true;
                        UnturnedChat.Say(caller, "Turned on vanish for " + unturnedPlayer2.DisplayName);
                        if(silent == false)
                        UnturnedChat.Say(unturnedPlayer2, "Vanish has been enabled for you");
                        return;
                    }
                    else
                    {
                        unturnedPlayer2.VanishMode = false;
                        UnturnedChat.Say(caller, "Turned off vanish for " + unturnedPlayer2.DisplayName);
                        if (silent == false)
                        UnturnedChat.Say(unturnedPlayer2, "Vanish has been disabled for you");
                        return;
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, "Invalid Arguments", Color.red);
                    return;
                }

            }
        }

        [RocketCommand("locate", "States the coordinates of Players", "[Player]", AllowedCaller.Player)]
        public void ExecuteCommandlocate(IRocketPlayer caller, string[] parameters)
        {
          /*  if (caller is Console)
            {
                UnturnedPlayer UnturnedPlayer2 = UnturnedPlayer.FromName(parameters[0]);
                float x = 0;
                float y = 0;
                float z = 0;
                if (parameters.Length == 1)
                {
                    x = UnturnedPlayer2.Position.x;
                    y = UnturnedPlayer2.Position.y;
                    z = UnturnedPlayer2.Position.z;
                    UnturnedChat.Say(caller, "The position of " + UnturnedPlayer2.DisplayName + " is: x: " + x + " y: " + y + " z: " + z);
                    return;
                }
                else
                {
                    UnturnedChat.Say(caller, "Invalid Arguments");
                    return;
                }
            }*/
            if (caller is UnturnedPlayer)
            {
                UnturnedPlayer UnturnedPlayer1 = (UnturnedPlayer)caller;
                Vector3 pos = UnturnedPlayer1.Position;
                float x = 0;
                float y = 0;
                float z = 0;
                if (parameters.Length == 0)
                {
                    x = UnturnedPlayer1.Position.x;
                    y = UnturnedPlayer1.Position.y;
                    z = UnturnedPlayer1.Position.z;
                    UnturnedChat.Say(caller, "Your position is: x: " + x + " y: " + y + " z: " + z);
                    return;
                }
                if (parameters.Length == 1)
                {
                    UnturnedPlayer UnturnedPlayer2 = UnturnedPlayer.FromName(parameters[0]);
                    if (UnturnedPlayer2 == null)
                    {
                        UnturnedChat.Say(caller, "Player not found", Color.red);
                        return;
                    }
                    x = UnturnedPlayer2.Position.x;
                    y = UnturnedPlayer2.Position.y;
                    z = UnturnedPlayer2.Position.z;
                    UnturnedChat.Say(caller, "The position of " + UnturnedPlayer2.DisplayName + " is: x: " + x + " y: " + y + " z: " + z);
                    return;
                }
                else
                {
                    UnturnedChat.Say(caller, "Invalid Arguments", Color.red);
                    return;
                }
            }
        }
        [RocketCommand("metaweapon", "Spawns a weapon with edited meta data [Warning: Only use if you know how to]", "", AllowedCaller.Player)]
        [RocketCommandAlias("mw")]
        public void ExecuteCommandmetaweapon(IRocketPlayer caller, string[] parameters)
        {
            SDG.Unturned.ItemAsset currentEquiped;
            UnturnedPlayer player = (UnturnedPlayer)caller;
            byte page1 = player.Player.equipment.equippedPage;
            ItemJar currentWeapon = null;
           currentWeapon = player.Player.inventory.getItem(page1, 0);

            //Checking
          /*  if (parameters.Length == 0)
            {
                UnturnedChat.Say(caller, "Invalid Arguments", Color.red);
                return;
            }*/
            currentEquiped = player.Player.equipment.asset;
            if (currentEquiped == null)
            {
                UnturnedChat.Say(caller, "No Gun Equipped", Color.red);
                return;
            }
            if (currentEquiped.type != SDG.Unturned.EItemType.GUN)
            {
                UnturnedChat.Say(caller, "No Gun Equipped", Color.red);
                return;
            }

            byte[] Correct = currentWeapon.item.state;
            if (parameters.Length > 0)
            {
                if (parameters[0].ToLower() == "player")
                {
                    Item weapon = currentWeapon.item;
                    ItemJar CurrentWeapon = new ItemJar(weapon);
                    Item weapon2 = CurrentWeapon.item;
                    UnturnedPlayer unturnedPlayer2 = UnturnedPlayer.FromName(parameters[1]);
                    if (parameters[1] == "*")
                    {
                        foreach (SteamPlayer sp in Provider.clients)
                        {
                            UnturnedPlayer listplayer = UnturnedPlayer.FromSteamPlayer(sp);
                            listplayer.Inventory.tryAddItem(weapon2, true, true);
                        }
                        player.Inventory.removeItem(page1, 0);
                        return;
                    }
                    if (unturnedPlayer2 == null)
                    {
                        UnturnedChat.Say(caller, "Player could not be found.", Color.red);
                        return;
                    }
                    if (unturnedPlayer2 != null)
                    {
                        unturnedPlayer2.Inventory.tryAddItem(weapon2, true);
                        player.Inventory.removeItem(page1, 0);
                        return;
                    }
                }

                if (parameters.Length > 0 && parameters[0].ToLower() != "honeybadger" && parameters[0].ToLower() != "heartbreaker" && parameters[0].ToLower() != "maplestrike")
                {



                    bool changedmagazine = false;
                    bool changedbarrel = false;
                    bool changedtactical = false;
                    bool changedsight = false;
                    bool changedgrip = false;
                    bool changedrate = false;
                    bool opammo = false;



                    byte sight1 = Correct[0];
                    byte sight2 = Correct[1];
                    byte tactical1 = Correct[2];
                    byte tactical2 = Correct[3];
                    byte grip1 = Correct[4];
                    byte grip2 = Correct[5];
                    byte barrel1 = Correct[6];
                    byte barrel2 = Correct[7];
                    byte magazine1 = Correct[8];
                    byte magazine2 = Correct[9];
                    byte ammo = Correct[10];
                    byte firemode = Correct[11];
                    byte something = Correct[12];
                    byte sightdurability = Correct[13];
                    byte tacticaldurability = Correct[14];
                    byte gripdurability = Correct[15];
                    byte barreldurability = Correct[16];
                    byte magazinedurability = Correct[17];

                    byte[] Changed = new byte[18];

                    if (parameters.Length > 0)
                    {
                        if (parameters[0].ToLower() == "ammo:edit")
                        {
                           int ammoamount = 0;
                           if (int.TryParse(parameters[1], out ammoamount) == true)
                                {
                                byte ammoamount1 = Convert.ToByte(ammoamount);
                                ammo = ammoamount1;

                                Changed[0] = sight1;
                                Changed[1] = sight2;
                                Changed[2] = tactical1;
                                Changed[3] = tactical2;
                                Changed[4] = grip1;
                                Changed[5] = grip2;
                                Changed[6] = barrel1;
                                Changed[7] = barrel2;
                                Changed[8] = magazine1;
                                Changed[9] = magazine2;
                                Changed[10] = ammo;
                                Changed[11] = firemode;
                                Changed[12] = something;
                                Changed[13] = sightdurability;
                                Changed[14] = tacticaldurability;
                                Changed[15] = gripdurability;
                                Changed[16] = barreldurability;
                                Changed[17] = magazinedurability;

                                currentWeapon.item.metadata = Changed;
                                return;
                            }   
                        }
                    }

                    foreach (string s in parameters)
                    {
                        //Grips
                        if (s == "143" || s.ToLower() == "bipod" && changedgrip == false)
                        {
                            //bipod
                            changedgrip = true;
                            grip1 = 143;
                            grip2 = 0;
                            gripdurability = 100;
                        }
                        if (s == "145" || s.ToLower() == "horizontal" || s.ToLower() == "horizontal grip" && changedgrip == false)
                        {
                            //horizontal
                            changedgrip = true;
                            grip1 = 145;
                            grip2 = 0;
                            gripdurability = 100;
                        }
                        if (s == "8" || s.ToLower() == "vertical" || s.ToLower() == "vertical grip" && changedgrip == false)
                        {
                            //vertical grip
                            changedgrip = true;
                            grip1 = 8;
                            grip2 = 0;
                            gripdurability = 100;
                        }
                        //Tactical
                        if (s == "1007" || s.ToLower() == "adaptive" || s.ToLower() == "adaptive chambering" && changedtactical == false)
                        {
                            //adaptive
                            changedtactical = true;
                            tactical1 = 239;
                            tactical2 = 3;
                            tacticaldurability = 100;
                        }
                        if (s == "1008" || s.ToLower() == "rangefinder" && changedtactical == false)
                        {
                            //rangefinder
                            changedtactical = true;
                            tactical1 = 240;
                            tactical2 = 3;
                            tacticaldurability = 100;
                        }
                        if (s == "151" || s.ToLower() == "tactical laser" || s.ToLower() == "laser" || s.ToLower() == "tactical" && changedtactical == false)
                        {
                            //tactical laser
                            changedtactical = true;
                            tactical1 = 151;
                            tactical2 = 0;
                            tacticaldurability = 100;
                        }
                        if (s == "152" || s.ToLower() == "tactical light" && changedtactical == false)
                        {
                            //tactical light
                            changedtactical = true;
                            tactical1 = 152;
                            tactical2 = 0;
                            tacticaldurability = 100;

                        }
                        if (s == "1438" || s.ToLower() == "bayonet" && changedtactical == false)
                        {
                            //Bayonet
                            changedtactical = true;
                            tactical1 = 158;
                            tactical2 = 5;
                            tacticaldurability = 100;
                        }
                        //Barrels
                        if (s == "149" || s.ToLower() == "military barrel" || s.ToLower() == "barrel" && changedbarrel == false)
                        {
                            //Mili barrel
                            changedbarrel = true;
                            barrel1 = 149;
                            barrel2 = 0;
                            barreldurability = 100;
                        }
                        if (s == "7" || s.ToLower() == "military suppressor" || s.ToLower() == "suppressor" || s.ToLower() == "sup" && changedbarrel == false)
                        {
                            //Mili suppreser
                            changedbarrel = true;
                            barrel1 = 7;
                            barrel2 = 0;
                            barreldurability = 100;
                        }
                        if (s == "150" || s.ToLower() == "military muzzle" || s.ToLower() == "muzzle" && changedbarrel == false)
                        {
                            //Mili Muzzle
                            changedbarrel = true;
                            barrel1 = 150;
                            barrel2 = 0;
                            barreldurability = 100;
                        }
                        if (s == "144" || s.ToLower() == "ranger suppressor" || s.ToLower() == "r sup" || s.ToLower() == "ranger sup" && changedbarrel == false)
                        {
                            //Ranger Suppreser
                            changedbarrel = true;
                            barrel1 = 144;
                            barrel2 = 0;
                            barreldurability = 100;
                        }
                        if (s == "1191" || s.ToLower() == "ranger barrel" && changedbarrel == false)
                        {
                            //Ranger Barrel
                            changedbarrel = true;
                            barrel1 = 167;
                            barrel2 = 4;
                            barreldurability = 100;
                        }
                        if (s == "1190" || s.ToLower() == "ranger muzzle" && changedbarrel == false)
                        {
                            //Ranger Muzzle
                            changedbarrel = true;
                            barrel1 = 166;
                            barrel2 = 4;
                            barreldurability = 100;
                        }
                        if (s == "117" || s.ToLower() == "honeybadger" || s.ToLower() == "honey" || s.ToLower() == "honeybadger barrel" && changedbarrel == false)
                        {
                            //Honeybadger barrel
                            changedbarrel = true;
                            barrel1 = 117;
                            barrel2 = 0;
                            barreldurability = 100;
                        }
                        if (s == "1002" || s.ToLower() == "matamorez" || s.ToLower() == "matamorez barrel" && changedbarrel == false)
                        {
                            //matamorez barrel
                            changedbarrel = true;
                            barrel1 = 234;
                            barrel2 = 3;
                            barreldurability = 100;
                        }
                        if (s == "350" || s.ToLower() == "crossbow" || s.ToLower() == "crossbow barrel" && changedbarrel == false)
                        {
                            //Crowbow barrel
                            changedbarrel = true;
                            barrel1 = 94;
                            barrel2 = 1;
                            barreldurability = 100;
                        }
                        if (s == "354" || s.ToLower() == "bow barrel" || s.ToLower() == "bow" && changedbarrel == false)
                        {
                            //bow barrel
                            changedbarrel = true;
                            barrel1 = 98;
                            barrel2 = 1;
                            barreldurability = 100;
                        }
                        if (s == "1444" || s.ToLower() == "#name" && changedbarrel == false)
                        {
                            //Something Barrel - Makeshift sup? #name
                            changedbarrel = true;
                            barrel1 = 164;
                            barrel2 = 5;
                            barreldurability = 100;
                        }
                        if (s == "477" || s.ToLower() == "makeshift" || s.ToLower() == "makeshift muffler" || s.ToLower() == "makeshiftmuffler" && changedbarrel == false)
                        {
                            //Makeshift Muffler
                            changedbarrel = true;
                            barrel1 = 221;
                            barrel2 = 1;
                            barreldurability = 100;
                        }
                        if (s == "1167" || s.ToLower() == "nailgun barrel" || s.ToLower() == "nailgun" && changedbarrel == false)
                        {
                            //Nail Gun
                            changedbarrel = true;
                            barrel1 = 143;
                            barrel2 = 4;
                            barreldurability = 100;
                        }
                        if (s == "1338" || s.ToLower() == "paintball barrel" && changedbarrel == false)
                        {
                            //Paintball gun
                            changedbarrel = true;
                            barrel1 = 58;
                            barrel2 = 5;
                            barreldurability = 100;
                        }
                        /*Barrel template  if (s == "" || s.ToLower() == "" && changedbarrel == false)
                                           {
                                               //Name
                                               changedbarrel = true;
                                               barrel1 = ;
                                               barrel2 = ;
                                               barreldurability = 100;
                                           }*/
                        //Magazines
                        if (s == "1365" || s.ToLower() == "hellsfury" || s.ToLower() == "hell" || s.ToLower() == "hells" || s.ToLower() == "hellsfury drum" && changedmagazine == false)
                        {
                            //hellsfury drum
                            changedmagazine = true;
                            magazine1 = 85;
                            magazine2 = 5;
                            magazinedurability = 100;
                            ammo = 250;
                        }
                        if (s == "1443" || s.ToLower() == "mk2 drum" && changedmagazine == false)
                        {
                            //shadowstalker mk2 drum
                            changedmagazine = true;
                            magazine1 = 163;
                            magazine2 = 5;
                            magazinedurability = 100;
                            ammo = 20;
                        }
                        if (s == "1302" || s.ToLower() == "cannon" || s.ToLower() == "tank" || s.ToLower() == "tank cannon" && changedmagazine == false)
                        {
                            //Tank cannon mag / Missile
                            changedmagazine = true;
                            magazine1 = 22;
                            magazine2 = 5;
                            magazinedurability = 100;
                            ammo = 1;
                        }
                        if (s == "17" || s.ToLower() == "drum" || s.ToLower() == "military drum" && changedmagazine == false)
                        {
                            //Military Drum
                            changedmagazine = true;
                            magazine1 = 17;
                            magazine2 = 0;
                            magazinedurability = 100;
                            ammo = 100;
                        }
                        if (s == "125" || s.ToLower() == "rangerdrum" || s.ToLower() == "ranger drum" && changedmagazine == false)
                        {
                            //Ranger Drum
                            changedmagazine = true;
                            magazine1 = 125;
                            magazine2 = 0;
                            magazinedurability = 100;
                            ammo = 75;
                        }
                        if (s == "133" || s.ToLower() == "dragonfang box" || s.ToLower() == "dragonfang" || s.ToLower() == "dfang" && changedmagazine == false)
                        {
                            //DragonFang Box
                            changedmagazine = true;
                            magazine1 = 133;
                            magazine2 = 0;
                            magazinedurability = 100;
                            ammo = 150;
                        }
                        if (s == "127" || s.ToLower() == "nykorev box" || s.ToLower() == "nykorev" || s.ToLower() == "nyk" && changedmagazine == false)
                        {
                            //Nykorev Box
                            changedmagazine = true;
                            magazine1 = 127;
                            magazine2 = 0;
                            magazinedurability = 100;
                            ammo = 200;
                        }
                        if (s == "6" || s.ToLower() == "normal mag" || s.ToLower() == "military magazine" && changedmagazine == false)
                        {
                            //Normal Mili Mag
                            changedmagazine = true;
                            magazine1 = 6;
                            magazine2 = 0;
                            magazinedurability = 100;
                            ammo = 30;
                        }
                        if (s == "1395" || s.ToLower() == "hmg box" || s.ToLower() == "hmg" && changedmagazine == false)
                        {
                            //HMG box
                            changedmagazine = true;
                            magazine1 = 115;
                            magazine2 = 5;
                            magazinedurability = 100;
                            ammo = 50;
                        }
                        if (s == "1166" || s.ToLower() == "nailgun mag" || s.ToLower() == "nailgun magazine" && changedmagazine == false)
                        {
                            //Nail Gun 
                            changedmagazine = true;
                            magazine1 = 142;
                            magazine2 = 4;
                            magazinedurability = 100;
                            ammo = 20;
                        }
                        if (s == "1381" || s.ToLower() == "callingcard" || s.ToLower() == "callingcard magazine" && changedmagazine == false)
                        {
                            //Calling Card Magazine
                            changedmagazine = true;
                            magazine1 = 101;
                            magazine2 = 5;
                            magazinedurability = 100;
                            ammo = 71;
                        }
                        if (s == "520" || s.ToLower() == "rocket" && changedmagazine == false)
                        {
                            //rocket
                            changedmagazine = true;
                            magazine1 = 8;
                            magazine2 = 2;
                            magazinedurability = 100;
                            ammo = 1;
                        }
                        if (s == "113" || s.ToLower() == "12 gauge" || s.ToLower() == "12g" && changedmagazine == false)
                        {
                            //12 Gauge shotgun shells
                            changedmagazine = true;
                            magazine1 = 113;
                            magazine2 = 0;
                            magazinedurability = 100;
                            ammo = 8;
                        }
                        if (s == "381" || s.ToLower() == "20g" || s.ToLower() == "20 gauge" && changedmagazine == false)
                        {
                            //Masterkey Ammo  / 20 gauge
                            changedmagazine = true;
                            magazine1 = 125;
                            magazine2 = 1;
                            magazinedurability = 100;
                            ammo = 2;
                        }
                        if (s == "1368" || s.ToLower() == "vonya magazine" || s.ToLower() == "vonya" && changedmagazine == false)
                        {
                            //Vonya Mag
                            changedmagazine = true;
                            magazine1 = 88;
                            magazine2 = 5;
                            magazinedurability = 100;
                            ammo = 7;
                        }
                        if (s == "347" || s.ToLower() == "arrow" && changedmagazine == false)
                        {
                            //Arrow
                            changedmagazine = true;
                            magazine1 = 91;
                            magazine2 = 1;
                            magazinedurability = 100;
                            ammo = 1;
                        }
                        if (s == "1209" || s.ToLower() == "explosive arrow"  && changedmagazine == false)
                        {
                            //Explosive Arrow
                            changedmagazine = true;
                            magazine1 = 185;
                            magazine2 = 4;
                            magazinedurability = 100;
                            ammo = 1;
                        }
                        if (s == "1200" || s.ToLower() == "fragmentation" || s.ToLower() == "military fragmentation magazine" && changedmagazine == false)
                        {
                            //Fragmentation
                            changedmagazine = true;
                            magazine1 = 176;
                            magazine2 = 4;
                            magazinedurability = 100;
                            ammo = 20;
                        }
                        if (s == "1177" || s.ToLower() == "tracer" || s.ToLower() == "military tracer magazine" && changedmagazine == false)
                        {
                            //Tracer
                            changedmagazine = true;
                            magazine1 = 153;
                            magazine2 = 4;
                            magazinedurability = 100;
                            ammo = 20;
                        }
                        /*    Magazine template               if (s == "" || s.ToLower() == "" || s.ToLower() == "" && changedmagazine == false)
                                                                   {
                                                                       //item
                                                                       changedmagazine = true;
                                                                       magazine1 = ;
                                                                       magazine2 = ;
                                                                       magazinedurability = 100;
                                                                       ammo = ;
                                                                   }*/
                        //Sights

                        if (s == "1442" || s.ToLower() == "mk2 scope" && changedsight == false)
                        {
                            //shadowstalker mk2 sight
                            changedsight = true;
                            sight1 = 162;
                            sight2 = 5;
                        }
                        if (s == "296" || s.ToLower() == "16x scope" || s.ToLower() == "16x" && changedsight == false)
                        {
                            //16x sight
                            changedsight = true;
                            sight1 = 40;
                            sight2 = 1;
                        }
                        if (s == "146" || s.ToLower() == "dot" || s.ToLower() == "dot sight" && changedsight == false)
                        {
                            //Dot sight
                            changedsight = true;
                            sight1 = 146;
                            sight2 = 0;
                        }
                        if (s == "147" || s.ToLower() == "halo" || s.ToLower() == "halo sight" && changedsight == false)
                        {
                            //halo Sight
                            changedsight = true;
                            sight1 = 147;
                            sight2 = 0;
                        }
                        if (s == "153" || s.ToLower() == "7x" || s.ToLower() == "7x scope" && changedsight == false)
                        {
                            //7x Scope
                            changedsight = true;
                            sight1 = 153;
                            sight2 = 0;
                        }
                        if (s == "21" || s.ToLower() == "8x" || s.ToLower() == "8x scope" && changedsight == false)
                        {
                            //8x Scope
                            changedsight = true;
                            sight1 = 21;
                            sight2 = 0;
                        }
                        if (s == "148" || s.ToLower() == "chevron" || s.ToLower() == "chevron scope" && changedsight == false)
                        {
                            //Chevron
                            changedsight = true;
                            sight1 = 148;
                            sight2 = 0;
                        }
                        if (s == "22" || s.ToLower() == "cross" || s.ToLower() == "cross scope" && changedsight == false)
                        {
                            //Cross Scope
                            changedsight = true;
                            sight1 = 22;
                            sight2 = 0;
                        }

                        /* Sight Template   if (s == "" || s.ToLower() == "" && changedsight == false)
                                            {
                                                //item
                                                changedsight = true;
                                                sight1 = ;
                                                sight2 = ;
                                            }*/
                        //FireRate
                        if (s.ToLower() == "automatic" && changedrate == false)
                        {
                            changedrate = true;
                            firemode = 2;
                        }
                        if (s.ToLower() == "semi" && changedrate == false)
                        {
                            changedrate = true;
                            firemode = 1;
                        }
                        if (s.ToLower() == "burst" && changedrate == false)
                        {
                            changedrate = true;
                            firemode = 3;
                        }
                        if (s.ToLower() == "safety" && changedrate == false)
                        {
                            changedrate = true;
                            firemode = 0;
                        }
                        if (s.ToLower() == "no-fire" && changedrate == false)
                        {
                            changedrate = true;
                            firemode = 12;
                        }
                        if (s.ToLower() == "ammo:op")
                            opammo = true;
                        if (s.ToLower() == "null")
                        {
                            sight1 = 0;
                            sight2 = 0;
                            tactical1 = 0;
                            tactical2 = 0;
                            grip1 = 0;
                            grip2 = 0;
                            barrel1 = 0;
                            barrel2 = 0;
                            magazine1 = 0;
                            magazine2 = 0;
                            ammo = 0;
                            firemode = 0;
                            something = 1;
                            sightdurability = 0;
                            tacticaldurability = 0;
                            gripdurability = 0;
                            barreldurability = 0;
                            magazinedurability = 0;

                            Changed[0] = sight1;
                            Changed[1] = sight2;
                            Changed[2] = tactical1;
                            Changed[3] = tactical2;
                            Changed[4] = grip1;
                            Changed[5] = grip2;
                            Changed[6] = barrel1;
                            Changed[7] = barrel2;
                            Changed[8] = magazine1;
                            Changed[9] = magazine2;
                            Changed[10] = ammo;
                            Changed[11] = firemode;
                            Changed[12] = something;
                            Changed[13] = sightdurability;
                            Changed[14] = tacticaldurability;
                            Changed[15] = gripdurability;
                            Changed[16] = barreldurability;
                            Changed[17] = magazinedurability;

                            currentWeapon.item.metadata = Changed;
                            return;
                        }
                        
                    }
                    if (opammo == true)
                    {
                        ammo = 250;
                    }

                    Changed[0] = sight1;
                    Changed[1] = sight2;
                    Changed[2] = tactical1;
                    Changed[3] = tactical2;
                    Changed[4] = grip1;
                    Changed[5] = grip2;
                    Changed[6] = barrel1;
                    Changed[7] = barrel2;
                    Changed[8] = magazine1;
                    Changed[9] = magazine2;
                    Changed[10] = ammo;
                    Changed[11] = firemode;
                    Changed[12] = something;
                    Changed[13] = sightdurability;
                    Changed[14] = tacticaldurability;
                    Changed[15] = gripdurability;
                    Changed[16] = barreldurability;
                    Changed[17] = magazinedurability;

                    currentWeapon.item.metadata = Changed;


                }
            }
            //Data dat = new Data();
            //dat.writeSingle("Range", 9999999999999999);
            //dat.writeSingle("Zombie_Damage", 99999999999999);
            //dat.writeSingle("Player_Damage", 99999999999999);
            //dat.writeSingle("Animal_Damage", 99999999999999);
            //dat.writeSingle("Barricade_Damage", 99999999999999);
            //dat.writeSingle("Structure_Damage", 99999999999999);
            //dat.writeSingle("Vehicle_Damage", 99999999999999);
            //dat.writeSingle("Resource_Damage", 99999999999999);
            //dat.writeSingle("Speed", 99999999999999);
            //dat.writeUInt16("Explosion", (ushort)65535);
            //dat.writeUInt16("Impact", (ushort)65535);
            //dat.writeUInt16("Tracer", 45);
            //dat.writeBoolean("Explosive", true);
            //dat.writeBoolean("Delete_Empty", false);
            //AssetBundle Magazine = AssetBundle.LoadFromFile("Magazine");
            //Localization.read();
            /*           ItemAsset uItem = null;
                       PlayerInventory inventory = player.Player.inventory;
                       for (byte page = 0; page < 8; page++)
                       {
                           byte amountOfItems = inventory.getItemCount(page);
                           for (int index = amountOfItems - 1; index >= 0; index--)
                           {
                               try
                               {
                                   uItem = UnturnedItems.GetItemAssetById(inventory.getItem(page, (byte)index).item.id);
                               }
                               catch (Exception)
                               {
                                   //Logger.LogError("Error trying to get item at Page: " + page + " Index: " + index);
                               }

                               if (uItem != null)
                               {
                                   if (uItem.type == EItemType.MAGAZINE)
                                   {

                                   }
                               }

                               uItem = null;
                           }
                       }
                       */



            /*          Vector3 pos = player.Position;
                      ItemManager.getItemsInRadius(pos, 10, new List<RegionCoordinate>(), ItemsList);
                      foreach(InteractableItem i in ItemsList)
                      {

                          (ItemAsset)Assets.find(, i);

                      }
                      */
            //ItemMagazineAsset mag = new ItemMagazineAsset(new Bundle("Magazine"), dat, new Local(dat), 17);
            //ItemGunAsset gun = new ItemGunAsset(new Bundle (), dat, Localization.read("/Player/Useable/PlayerUseableGun.dat"), 17);
            //mag.name = "Fuck her right in the pussy.";
            //ItemMagazineAsset gunn = new ItemMagazineAsset(new Bundle(), dat, Localization.read(""), 17);
            //Item magItem = new Item(17, true);
            //player.Inventory.tryAddItem(magItem, true);
            //magItem.state = mag.getState();

            /*       byte[] Heartbreaker = new byte[18] { 146, 0, 239, 3, 8, 0, 7, 0, 85, 5, 250, 2, 1, 100, 100, 100, 255, 100 }; // Tracer ammunition bytes are 153, 4 in position 8, 9 - Military frag is 176,4

                    if (parameters.Length == 1 && parameters[0].ToLower() == "heartbreaker")
                    {
                        Item check = new Item(1037, 1, 100, Heartbreaker);//255 is highest value a byte can go to                   
                        player.GiveItem(check);
                        return;
                        //player.Inventory.tryAddItem(magItem, 0, 0, 0, 1);
                    }
                    byte[] HoneyBadger = new byte[18] { 146, 0, 239, 3, 8, 0, 7, 0, 85, 5, 250, 2, 1, 100, 100, 100, 255, 100 };
                    if (parameters.Length == 1 && parameters[0].ToLower() == "honeybadger")
                    {
                        Item check = new Item(116, 1, 100, HoneyBadger);
                        player.GiveItem(check);
                        return;
                    }
                    byte[] Maplestrike = new byte[18] { 146, 0, 239, 3, 143, 0, 149, 0, 85, 5, 250, 2, 1, 100, 100, 100, 255, 100 };
                    if (parameters.Length == 1 && parameters[0].ToLower() == "maplestrike")
                    {
                        Item spawn = new Item(363, 1, 100, Maplestrike);
                        player.GiveItem(spawn);
                        return;
                    }*/

            byte[] c = currentWeapon.item.state;
            if (parameters.Length == 0)
            {

                    //player.Player.equipment.applyMythicVisual();
                    //player.Player.equipment.equip(3, 1, 1, 17);
                    //byte[] finder = magItem.state;

                    foreach (byte d in c)
                {
                    Console.WriteLine(d);
                }
               // return;
                //player.Player.inventory.tryAddItem(magItem, true);
                //byte[] f = new byte[12];
                //Item test = new Item(1037, 1, 100, f);
                //player.GiveItem(test);
                return;
            }
        }
    }
}
//public class GunMetaData
//{
//    public byte barrel1;
//    public byte barrel2;
//    public byte mag1;
//    public byte mag2;
//    public byte ammo;
//    public byte tactical1;
//    public byte tactical2;
//    public byte grip1;
//    public byte grip2;
//    public byte sight1;
//    public byte sight2;
//    public GunMetaData(byte sight1, byte sight2, byte barrel1, byte barrel2, byte tactical1, byte tactical2, byte mag1)
//    {
//    }
//}