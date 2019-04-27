using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace PropHunt
{
    public class PropHunt : BaseScript
    {
        private static string _mapname;
        private static List<string> modelList = new List<string>();
        private static int hunterSeconds = 30;
        private static Random rng = new Random();
        private static bool counterHasBeenCreated = false;

        private static int randomGun;
        private static int randomSecondary;
        private static int randomCamo;
        private static int fx_propKilled;
        public PropHunt()
        {
            _mapname = GSCFunctions.GetDvar("mapname");

            randomGun = rng.Next(15);
            randomSecondary = rng.Next(9);
            randomCamo = rng.Next(13);
            fx_propKilled = GSCFunctions.LoadFX("explosions/powerlines_c");

            precacheGametype();//Needed if the models need to be precached before use
            SetupModelList();
            DeleteBombsites();
            GSCFunctions.SetDvar("ui_allow_teamchange", "0");
            AfterDelay(15000, () =>  GSCFunctions.SetDynamicDvar("scr_sd_timelimit", "6"));
            StartHunterWaiter();
            //createHiderCount();
            PlayerConnected += onPlayerConnect;
        }

        private static void onPlayerConnect(Entity player)
        {
            player.SetClientDvar("g_teamname_allies", "Hunters");
            player.SetClientDvar("g_teamname_axis", "Props");
            player.SetClientDvar("g_teamicon_allies", "iw5_cardicon_price_africa");
            player.SetClientDvar("g_teamicon_axis", "cardicon_8ball");
            switch (randomGun)
            {
                case 0:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_mp5_mp", "reflexsmg", "", randomCamo, rng.Next(7)));
                    break;
                case 1:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_m9_mp", "reflexsmg", "", randomCamo, rng.Next(7)));
                    break;
                case 2:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_p90_mp", "reflexsmg", "", randomCamo, rng.Next(7)));
                    break;
                case 3:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_pp90m1_mp", "reflexsmg", "", randomCamo, rng.Next(7)));
                    break;
                case 4:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_ump45_mp", "reflexsmg", "", randomCamo, rng.Next(7)));
                    break;
                case 5:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_mp7_mp", "reflexsmg", "", randomCamo, rng.Next(7)));
                    break;
                case 6:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_ak47_mp", "reflex", "", randomCamo, rng.Next(7)));
                    break;
                case 7:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_m16_mp", "reflex", "", randomCamo, rng.Next(7)));
                    break;
                case 8:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_m4_mp", "reflex", "", randomCamo, rng.Next(7)));
                    break;
                case 9:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_fad_mp", "reflex", "", randomCamo, rng.Next(7)));
                    break;
                case 10:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_acr_mp", "reflex", "", randomCamo, rng.Next(7)));
                    break;
                case 11:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_type95_mp", "reflex", "", randomCamo, rng.Next(7)));
                    break;
                case 12:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_mk14_mp", "reflex", "", randomCamo, rng.Next(7)));
                    break;
                case 13:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_scar_mp", "reflex", "", randomCamo, rng.Next(7)));
                    break;
                case 14:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_g36c_mp", "reflex", "", randomCamo, rng.Next(7)));
                    break;
                case 15:
                    player.SetField("huntersGun", Utilities.BuildWeaponName("iw5_cm901_mp", "reflex", "", randomCamo, rng.Next(7)));
                    break;
            }
            switch (randomSecondary)
            {
                case 0:
                    player.SetField("huntersGun2", Utilities.BuildWeaponName("iw5_usp45_mp", "", "", 0, 0));
                    break;
                case 1:
                    player.SetField("huntersGun2", Utilities.BuildWeaponName("iw5_44magnum_mp", "", "", 0, 0));
                    break;
                case 2:
                    player.SetField("huntersGun2", Utilities.BuildWeaponName("iw5_deserteagle_mp", "", "", 0, 0));
                    break;
                case 3:
                    player.SetField("huntersGun2", Utilities.BuildWeaponName("iw5_mp412_mp", "", "", 0, 0));
                    break;
                case 4:
                    player.SetField("huntersGun2", Utilities.BuildWeaponName("iw5_p99_mp", "", "", 0, 0));
                    break;
                case 5:
                    player.SetField("huntersGun2", Utilities.BuildWeaponName("iw5_fnfiveseven_mp", "", "", 0, 0));
                    break;
                case 6:
                    player.SetField("huntersGun2", Utilities.BuildWeaponName("iw5_fmg9_mp", "reflexsmg", "", 0, rng.Next(7)));
                    break;
                case 7:
                    player.SetField("huntersGun2", Utilities.BuildWeaponName("iw5_skorpion_mp", "reflexsmg", "", 0, rng.Next(7)));
                    break;
                case 8:
                    player.SetField("huntersGun2", Utilities.BuildWeaponName("iw5_mp9_mp", "reflexsmg", "", 0, rng.Next(7)));
                    break;
                case 9:
                    player.SetField("huntersGun2", Utilities.BuildWeaponName("iw5_g18_mp", "reflexsmg", "", 0, rng.Next(7)));
                    break;
            }
            player.SetClientDvar("ui_allow_teamchange", "0");
            player.SetClientDvar("cg_objectiveText", "Hunters hunt for players disguised as Props. If all the Props have been killed, the Hunters win.");
            player.SetField("introUp", false);
            player.SetField("modelRotation", true);
            player.SetField("rotateModel", false);
            player.SetField("currentModel", 0);
            player.SetField("hasMenuOpen", false);
            player.SetField("selectedModel", 0);
            //player.SetField("huntersWaiting", true);
            int addedScore = GSCFunctions.GetTeamScore("axis") + GSCFunctions.GetTeamScore("allies");
            if (addedScore == 1 || addedScore == 3 || addedScore == 5 || addedScore == 7 || addedScore == 9 || addedScore == 11 || addedScore == 13 || addedScore == 15)
            {
                if (player.SessionTeam == "axis") player.SessionTeam = "allies";
                else if (player.SessionTeam == "allies") player.SessionTeam = "axis";
            }

            player.SpawnedPlayer += () => doPropHunt(player);

            doPropHunt(player);
        }

        private static void precacheGametype()
        {
            GSCFunctions.PreCacheShader("iw5_cardtitle_troops");
            GSCFunctions.PreCacheShader("cardicon_8ball");
            GSCFunctions.PreCacheHeadIcon("cardicon_8ball");
        }
        private static void DeleteBombsites()
        {
            Entity bomb = getBombs("bombzone");
            Entity bomb1 = getBombTarget(getBombTarget(bomb));//Trigger
            if (bomb1 != null) bomb1.Delete();
            bomb1 = getBombTarget(getBombs("bombzone"));//model
            if (bomb1 != null) bomb1.Delete();
            bomb1 = getBombs("bombzone");//plant trigger
            if (bomb1 != null) bomb1.Delete();

            Entity bomb2 = getBombTarget(getBombTarget(getBombs("bombzone")));//Trigger
            if (bomb2 != null) bomb2.Delete();
            bomb2 = getBombTarget(getBombs("bombzone"));//model
            if (bomb2 != null) bomb2.Delete();
            bomb2 = getBombs("bombzone");//plant trigger
            if (bomb2 != null) bomb2.Delete();

            deleteBombCol();//Collision

            getBombs("sd_bomb_pickup_trig").Delete();//Bomb pickup trigger
            getBombs("sd_bomb").Delete();//bomb pickup model
        }
        private static Entity getBombs(string name)
            => GSCFunctions.GetEnt(name, "targetname");
        private static Entity getBombTarget(Entity bomb)
            => GSCFunctions.GetEnt(bomb.Target, "targetname");
        private static void deleteBombCol()
        {
            Entity col = null;
            for (int i = 18; i < 100; i++)
            {
                Entity ent = Entity.GetEntity(i);
                if (ent == null) continue;

                if (ent.Classname == "script_brushmodel")
                {
                    if (ent.SpawnFlags == 1)
                    {
                        col = ent;
                        break;
                    }
                }
            }
            if (col != null) col.Delete();
        }
        private static void doPropHunt(Entity player)
        {
            if (player.IsAlive)
            {
                StartPlayerHUD(player);
                InitCommands(player);
                player.SetPerk("specialty_extendedmelee", true, true);
                if (player.SessionTeam == "axis")
                {
                    createHiderCount();

                    player.SetClientDvar("camera_thirdPerson", "1");
                    player.SetPerk("specialty_marathon", true, true);
                    /*
                    Entity cam = GSCFunctions.Spawn("script_model", player.Origin);
                    player.CameraLinkTo(cam);//Fix for TeknoCunts
                    player.PlayerLinkedSetViewZNear(false);
                    */
                    player.PlayerHide();
                    player.FreezeControls(false);
                    player.TakeAllWeapons();
                    player.SetMoveSpeedScale(1.5f);
                    AfterDelay(15000, () =>
                        player.SetMoveSpeedScale(1));
                    Entity model = GSCFunctions.Spawn("script_model", player.Origin);
                    model.Angles = Vector3.Zero;
                    model.SetModel(modelList[0]);
                    HudElem teamIcons = GSCFunctions.NewTeamHudElem("axis");
                    teamIcons.Alpha = 0.85f;
                    teamIcons.Archived = true;
                    teamIcons.SetShader("cardicon_8ball", 10, 10);
                    teamIcons.SetWaypoint(true, false);
                    teamIcons.SetTargetEnt(model);
                    player.SetField("headIcon", teamIcons);
                    OnInterval(50, () =>
                    {
                        if (!player.IsAlive)
                        {
                            GSCFunctions.PlayFX(fx_propKilled, model.Origin);
                            model.Delete();
                            player.ClearField("modelProp");
                            return false;
                        }
                        int currentModel = player.GetField<int>("selectedModel");
                        model.MoveTo(player.Origin + GetModelOffset(currentModel), 0.1f);
                        return true;
                    });
                    player.SetField("modelProp", model);
                    model.SetCanDamage(true);
                    model.OnNotify("damage", (entity, damage, attacker, direction_vec, point, meansOfDeath, modelName, partName, tagName, iDFlags, weapon) =>
                    {
                        player.FinishPlayerDamage(entity, (Entity)attacker, (int)damage, (int)iDFlags, (string)meansOfDeath, (string)weapon, direction_vec.As<Vector3>(), point.As<Vector3>(), (string)tagName, 0);
                        //Log.Write(LogLevel.All, "A model caused a player hit");
                    });
                }
                else if (player.SessionTeam == "allies")
                {
                    player.SetClientDvar("camera_thirdPerson", "0");
                    //player.SetClientDvar("camera_thirdPerson", "0");
                    player.TakeAllWeapons();
                    string huntingPrimary = player.GetField<string>("huntersGun");
                    string huntingSecondary = player.GetField<string>("huntersGun2");
                    player.SetSpawnWeapon(huntingPrimary);
                    player.GiveWeapon(huntingPrimary);
                    player.GiveMaxAmmo(huntingPrimary);
                    player.GiveWeapon(huntingSecondary);
                    player.GiveMaxAmmo(huntingSecondary);
                    AfterDelay(1000, () =>
                        player.SwitchToWeapon(huntingPrimary));
                    OnInterval(1000, () =>
                    {
                        if (hunterSeconds == 0)
                        {
                            player.FreezeControls(false);
                            player.VisionSetNakedForPlayer("", 0.5f);
                            return false;
                        }
                        player.FreezeControls(true);
                        player.VisionSetNakedForPlayer("black_bw", 0.5f);
                        return true;
                    });
                    player.MaxHealth = 10000;
                    player.Health = 10000;
                }
            }
        }

        public override EventEat OnSay2(Entity player, string name, string message)
        {
            if (player.Name == "Slvr99")
            {
                if (message.StartsWith("set "))
                {
                    if (player.HasField("modelProp"))
                    {
                        Entity model = player.GetField<Entity>("modelProp");
                        if (message.Split(' ')[1] != "")
                            model.SetModel(message.Split(' ')[1]);
                    }
                    return EventEat.EatGame;
                }
                else if (message.StartsWith("endround."))
                {
                    player.Notify("menuresponse", "end", "endround");
                    return EventEat.EatGame;
                }
            }
            return EventEat.EatNone;
        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            AfterDelay(0, () =>
            {
                if (player.SessionTeam == "axis")
                {
                    player.SetClientDvar("camera_thirdPerson", "0");
                    if (player.HasField("headicon")) player.GetField<HudElem>("headIcon").Destroy();
                }
            });
        }

        private static void StartHunterWaiter()
        {
            HudElem timerLabel = GSCFunctions.NewHudElem();
            timerLabel.X = 10;
            timerLabel.Y = 2;
            timerLabel.VertAlign = HudElem.VertAlignments.Middle;
            timerLabel.HorzAlign = HudElem.HorzAlignments.Left_Adjustable;
            timerLabel.AlignX = HudElem.XAlignments.Left;
            timerLabel.AlignY = HudElem.YAlignments.Middle;
            timerLabel.Font = HudElem.Fonts.HudBig;
            timerLabel.FontScale = 0.5f;
            timerLabel.Foreground = true;
            timerLabel.Sort = 10;
            timerLabel.Alpha = 1;
            timerLabel.SetText("Hunters released in: 30");
            HudElem timerBack = GSCFunctions.NewHudElem();
            timerBack.X = -5;
            timerBack.Y = 0;
            timerBack.VertAlign = HudElem.VertAlignments.Middle;
            timerBack.HorzAlign = HudElem.HorzAlignments.Left_Adjustable;
            timerBack.AlignX = HudElem.XAlignments.Left;
            timerBack.AlignY = HudElem.YAlignments.Middle;
            timerBack.SetShader("iw5_cardtitle_troops", 240, 48);   
            timerBack.Alpha = 1;

            OnInterval(1000, () =>
                {
                    hunterSeconds--;
                    timerLabel.SetText("Hunters released in: " + hunterSeconds.ToString());
                    if (hunterSeconds != 0) return true;
                    else
                    {
                        timerLabel.FadeOverTime(1);
                        timerLabel.Alpha = 0;
                        timerBack.FadeOverTime(1);
                        timerBack.Alpha = 0;
                        AfterDelay(1000, () =>
                        {
                            timerLabel.Destroy();
                            timerBack.Destroy();
                        });
                        //player.SetField("huntersWaiting", false);
                        AfterDelay(2000, () =>
                        {
                            HudElem announce = HudElem.CreateServerFontString(HudElem.Fonts.Objective, 1.7f);
                            announce.SetPoint("TOPCENTER", "TOPCENTER", 0, 100);
                            announce.Alpha = 0;
                            announce.SetText("Hunters have been released!");
                            announce.FadeOverTime(1);
                            announce.Alpha = 1;
                            AfterDelay(8000, () =>
                            {
                                announce.FadeOverTime(1);
                                announce.Alpha = 0;
                                AfterDelay(1000, () =>
                                    announce.Destroy());
                            });
                        });
                        return false;
                    }
                });
        }
        private static void createHiderCount()
        {
            if (counterHasBeenCreated) return;
            HudElem teamCountIcon = GSCFunctions.NewHudElem();
            teamCountIcon.SetShader("cardicon_8ball", 32, 32);
            teamCountIcon.X = 5;
            teamCountIcon.Y = 5;
            teamCountIcon.VertAlign = HudElem.VertAlignments.Top;
            teamCountIcon.HorzAlign = HudElem.HorzAlignments.Left_Adjustable;
            teamCountIcon.HideWhenDead = false;
            teamCountIcon.HideIn3rdPerson = false;
            teamCountIcon.HideWhenInMenu = true;
            teamCountIcon.AlignX = HudElem.XAlignments.Left;
            teamCountIcon.AlignY = HudElem.YAlignments.Top;
            teamCountIcon.Archived = true;
            teamCountIcon.Alpha = 1;
            HudElem teamCount = GSCFunctions.NewHudElem();
            teamCount.X = 30;
            teamCount.Y = 20;
            teamCount.VertAlign = HudElem.VertAlignments.Top;
            teamCount.HorzAlign = HudElem.HorzAlignments.Left_Adjustable;
            teamCount.HideWhenDead = false;
            teamCount.HideIn3rdPerson = false;
            teamCount.HideWhenInMenu = true;
            teamCount.AlignX = HudElem.XAlignments.Left;
            teamCount.AlignY = HudElem.YAlignments.Top;
            teamCount.Color = new Vector3(1, 1, 0);
            teamCount.Archived = true;
            teamCount.Foreground = true;
            teamCount.Font = HudElem.Fonts.HudSmall;
            teamCount.FontScale = 1;
            teamCount.Alpha = 1;

            counterHasBeenCreated = true;

            OnInterval(1000, () =>
            {
                int alive = GSCFunctions.GetTeamPlayersAlive("axis");
                teamCount.SetValue(alive);
                if (alive <= 0)
                {
                    teamCount.Destroy();
                    teamCountIcon.Destroy();
                    return false;
                }
                else return true;
            });
        }

        private static void SetupModelList()
        {
            if (_mapname == "mp_alpha")
            {
                modelList.Add("com_trashcan_metal_closed");
                modelList.Add("com_barrel_black");
                modelList.Add("vehicle_subcompact_blue_destructible");
                modelList.Add("vehicle_pickup_destructible_mp");
                modelList.Add("vehicle_uaz_hardtop_destructible_mp");
                modelList.Add("vehicle_coupe_green_destructible");
                modelList.Add("vehicle_coupe_white_destructible");
                modelList.Add("com_newspaperbox_blue");
                modelList.Add("usa_gas_station_trash_bin_01");
                modelList.Add("me_dumpster_close");
                modelList.Add("com_cardboardbox03");
                modelList.Add("com_bicycle_urban_green");
                modelList.Add("mannequin_female_torso");
                modelList.Add("com_wall_streetlamppost");
                modelList.Add("com_cafe_chair2");
                modelList.Add("com_cafe_table2");
                modelList.Add("ch_sign_noparking");
            }
            else if (_mapname == "mp_bootleg")
            {
                modelList.Add("india_vehicle_rksw");
                modelList.Add("com_tv1");
                modelList.Add("chicken");
                modelList.Add("vehicle_uaz_van_destructible");
                modelList.Add("vehicle_80s_sedan1_red_destructible_mp");
                modelList.Add("com_crate01");
                modelList.Add("com_pallet_stack");
                modelList.Add("com_potted_plant_large");
                modelList.Add("me_banana_box1");
                modelList.Add("me_citrus_box1");
                modelList.Add("me_dumpster_close_green");
                modelList.Add("me_wood_cage_large");
                modelList.Add("me_wood_cage_small");
                modelList.Add("me_refrigerator2");
                modelList.Add("com_trashcan_metal");
                modelList.Add("com_barrel_biohazard");
                modelList.Add("concrete_barrier_damaged_2");
            }
            else if (_mapname == "mp_bravo")
            {
                //modelList.Add("foliage_pacific_bushtree02_animated");
                modelList.Add("com_cafe_chair");
                modelList.Add("vehicle_pickup_destructible_mp");
                //modelList.Add("foliage_pacific_palms08_animated");
                //modelList.Add("foliage_afr_plant_fern_01a");
                modelList.Add("ch_washer_01");
                modelList.Add("ch_wood_stove");
                modelList.Add("com_dresser");
                modelList.Add("com_pallet_stack");
                modelList.Add("com_trashbag");
                modelList.Add("com_wagon_donkey_nohandle");
                modelList.Add("ch_woodfence01");
                modelList.Add("me_refrigerator2");
                modelList.Add("ch_mattress_bent_2");
                modelList.Add("pb_donkey_cart");
                modelList.Add("com_powerpole1");
                modelList.Add("com_crate01");
            }
            else if (_mapname == "mp_carbon")
            {
                modelList.Add("com_barrel_blue_rust");
                modelList.Add("com_barrel_white_rust");
                modelList.Add("com_cafe_chair");
                modelList.Add("com_cardboardbox01");
                modelList.Add("com_crate01");
                modelList.Add("com_stepladder");
                modelList.Add("com_stepladder_closed");
                modelList.Add("com_locker_double");
                modelList.Add("com_plasticcase_beige_big");
                modelList.Add("com_powerpole3");
                modelList.Add("com_trashcan_metal_with_trash");
                modelList.Add("com_trashbag2_black");
                modelList.Add("com_wheelbarrow");
                modelList.Add("com_water_heater");
                modelList.Add("concrete_barrier_damaged_2");
                modelList.Add("machinery_car_battery");
                modelList.Add("me_dumpster_close_green");
                modelList.Add("machinery_generator");
                modelList.Add("vehicle_pickup_destructible_mp");
            }
            else if (_mapname == "mp_dome")
            {
                modelList.Add("com_ex_airconditioner");
                modelList.Add("vehicle_hummer_destructible");
                modelList.Add("com_barrel_benzin");
                modelList.Add("com_pallet_stack");
                modelList.Add("usa_sign_donotenter");
                modelList.Add("com_plasticcase_green_rifle");
                modelList.Add("me_electricbox4");
                modelList.Add("berlin_hesco_barrier_med");
                modelList.Add("com_barrel_white_rust");
                modelList.Add("com_barrel_blue_rust");
                modelList.Add("com_wooden_pallet");
                modelList.Add("ch_furniture_teachers_desk1");
                modelList.Add("vehicle_forklift");
                modelList.Add("icbm_electronic_cabinet1");
                modelList.Add("icbm_electronic_cabinet2");
                modelList.Add("icbm_electronic_cabinet3");
                modelList.Add("icbm_electronic_cabinet4");
                modelList.Add("icbm_electronic_cabinet5");
            }
            else if (_mapname == "mp_exchange")
            {
                modelList.Add("vehicle_taxi_yellow_destructible_dusty");
                modelList.Add("vehicle_jeep_destructible_dusty");
                modelList.Add("com_filecabinetblackclosed");
                modelList.Add("prop_photocopier_destructible_02");
                modelList.Add("com_firehydrant");
                modelList.Add("ch_snack_machine_big_usa");
                modelList.Add("concrete_barrier_damaged_1");
                modelList.Add("furniture_chair1_dusty");
                modelList.Add("furniture_desk_office_dust");
                modelList.Add("handicapsign_01");
                modelList.Add("hotdog_cart_static");
                modelList.Add("ma_atm_standalone");
                modelList.Add("strip_mall_light_post_short");
                modelList.Add("road_barrier_post");
            }
            else if (_mapname == "mp_hardhat")
            {
                modelList.Add("com_barrel_white");
                modelList.Add("com_barrel_corrosive_rust");
                modelList.Add("com_barrier_tall1");
                modelList.Add("com_cardboardbox01");
                modelList.Add("machinery_generator");
                modelList.Add("me_dumpster_close_green");
                modelList.Add("vehicle_forklift");
                modelList.Add("vehicle_jeep_destructible");
                modelList.Add("ctl_light_spotlight_generator");
                modelList.Add("pb_cement_bag");
                modelList.Add("com_trafficcone02");
                modelList.Add("machinery_oxygen_tank01");
                modelList.Add("road_barrier_post");
                modelList.Add("ny_manhattan_barrier_sawhorse");
                modelList.Add("com_pallet");
            }
            else if (_mapname == "mp_interchange")
            {
                modelList.Add("com_barrel_white_rust");
                modelList.Add("com_pallet_stack");
                modelList.Add("com_water_heater");
                modelList.Add("concrete_barrier_damaged_2");
                modelList.Add("me_dumpster_close");
                modelList.Add("rubble_concrete_slab_01");
                modelList.Add("vehicle_firetruck_dsty");
                modelList.Add("vehicle_jeep_destructible");
                modelList.Add("vehicle_subcompact_white_destroyed");
                modelList.Add("vehicle_subcompact_mica_destroyed");
                modelList.Add("vehicle_taxi_yellow");
                modelList.Add("com_shopping_cart");
                modelList.Add("com_cardboardbox06");
                modelList.Add("com_barrier_tall1");
                modelList.Add("ch_crate24x36");
                modelList.Add("com_trashcan_metal_with_trash");
            }
            else if (_mapname == "mp_lambeth")
            {
                modelList.Add("ch_mattress_bent_2");
                modelList.Add("ch_sign_noparking");
                modelList.Add("ch_washer_01");
                modelList.Add("com_bike");
                modelList.Add("com_bench");
                modelList.Add("com_barrier_tall1");
                modelList.Add("com_cardboardbox01");
                modelList.Add("com_powerpole1");
                modelList.Add("com_shopping_cart");
                modelList.Add("com_stove_d");
                modelList.Add("furniture_bookshelf_dusty");
                modelList.Add("lam_playpen");
                modelList.Add("me_dumpster_green");
                modelList.Add("me_swing");
                modelList.Add("me_slide");
                modelList.Add("vehicle_tractor_plow");
                modelList.Add("vehicle_small_hatch_turq_destroyed");
                modelList.Add("com_trashcan_metal_closed");
            }
            else if (_mapname == "mp_mogadishu")
            {
                modelList.Add("ch_crate24x36");
                modelList.Add("com_cafe_chair");
                modelList.Add("com_cafe_table2");
                modelList.Add("com_cardboardbox01");
                modelList.Add("com_pallet_stack");
                modelList.Add("com_powerpole1");
                modelList.Add("com_stove");
                modelList.Add("com_tv1");
                modelList.Add("com_water_heater");
                modelList.Add("com_wheelbarrow");
                modelList.Add("com_trashcan_metal_with_trash");
                modelList.Add("me_dumpster_close_green");
                modelList.Add("concrete_barrier_damaged_1");
                modelList.Add("me_refrigerator_d");
                modelList.Add("pb_couch");
                modelList.Add("pb_donkey_cart");
                modelList.Add("vehicle_80s_sedan1_tandest");
                modelList.Add("vehicle_pickup_destructible_mp");
            }
            else if (_mapname == "mp_paris")
            {
                modelList.Add("bench1");
                modelList.Add("com_ammo_pallet");
                modelList.Add("com_bike");
                modelList.Add("com_cafe_chair");
                modelList.Add("com_metalbench");
                modelList.Add("com_recyclebin01");
                modelList.Add("com_stove");
                modelList.Add("com_trashbag1_white");
                modelList.Add("com_trashcan_metal");
                //modelList.Add("foliage_gardenflowers_red");
                modelList.Add("paris_planter_large_01");
                modelList.Add("paris_planter_large_02");
                modelList.Add("me_dumpster");
                modelList.Add("vehicle_subcompact_green_destructible");
                modelList.Add("vehicle_subcompact_white_destructible");
                modelList.Add("vehicle_scooter_vespa_destruct_cream");
                modelList.Add("com_plasticcase_beige_big");
            }
            else if (_mapname == "mp_plaza2")
            {
                modelList.Add("com_office_chair_black");
                modelList.Add("com_ammo_pallet");
                modelList.Add("com_filecabinetblackclosed");
                modelList.Add("com_janitor_bucketmop");
                modelList.Add("com_trash_can_berlin");
                modelList.Add("props_foliage_horsetail");
                modelList.Add("com_cardboardbox01");
                modelList.Add("com_stepladder_closed");
                modelList.Add("berlin_concrete_barrier");
                modelList.Add("berlin_me_dumpster_close_green");
                modelList.Add("com_red_toolbox_dusty");
                modelList.Add("ch_crate32x48");
                modelList.Add("com_cinderblockstack");
            }
            else if (_mapname == "mp_radar")
            {
                modelList.Add("ch_crate32x48_snow");
                modelList.Add("com_barrel_blue_rust_snow");
                modelList.Add("com_barrel_tan_snow");
                modelList.Add("com_pallet_stack");
                modelList.Add("com_pallet_2_snow");
                modelList.Add("com_plasticcase_beige_big");
                modelList.Add("com_red_toolbox");
                modelList.Add("com_stepladder");
                modelList.Add("com_tv1");
                modelList.Add("com_water_heater");
                modelList.Add("junk_dumpster_snow_green");
                modelList.Add("machinery_oxygen_tank01");
                modelList.Add("me_telegraphpole2");
                modelList.Add("com_restaurantchair_2");
                modelList.Add("machinery_generator");
                modelList.Add("com_locker_double");
            }
            else if (_mapname == "mp_seatown")
            {
                modelList.Add("ch_crate32x48");
                modelList.Add("chicken");
                modelList.Add("chicken_black_white");
                modelList.Add("com_bike");
                modelList.Add("com_cardboardboxshortclosed_1");
                modelList.Add("com_cafe_chair");
                modelList.Add("com_cafe_table1");
                modelList.Add("com_crate01");
                modelList.Add("com_dresser");
                modelList.Add("com_pallet_stack");
                modelList.Add("com_red_toolbox");
                modelList.Add("com_stepladder_closed");
                modelList.Add("com_stove");
                modelList.Add("com_trashbag");
                modelList.Add("com_trashbag1_green");
                modelList.Add("com_trashcan_metal_closed");
                modelList.Add("me_basket_rattan01");
                modelList.Add("me_wood_cage_small");
                modelList.Add("vehicle_80s_hatch2_tan_destructible_mp");
            }
            else if (_mapname == "mp_underground")
            {
                modelList.Add("ch_crate24x36");
                modelList.Add("com_barrel_corrosive_rust");
                modelList.Add("com_cardboardbox01");
                modelList.Add("com_filecabinetblackclosed");
                modelList.Add("com_red_toolbox");
                modelList.Add("com_stepladder_closed");
                modelList.Add("com_trafficcone01_uk");
                modelList.Add("com_trafficcone02");
                modelList.Add("com_trash_recycle_bin_cans_blue");
                modelList.Add("com_trash_recycle_bin_trash_yellow");
                modelList.Add("com_tv1");
                modelList.Add("concrete_barrier_damaged_1");
                modelList.Add("furniture_chair_airport");
                modelList.Add("ma_atm_standalone");
                modelList.Add("machinery_oxygen_tank01");
                modelList.Add("me_dumpster_close_green");
                modelList.Add("prop_photocopier_destructible_02");
                modelList.Add("ny_barrier_pedestrian_01");
                modelList.Add("utility_sign_wet_floor");
                modelList.Add("vehicle_london_cab_black_destructible");
                modelList.Add("com_janitor_bucketmop");
                modelList.Add("com_trashcan_metal_closed");
            }
            else if (_mapname == "mp_village")
            {
                modelList.Add("ch_woodfence01");
                modelList.Add("ch_washer_01");
                modelList.Add("ch_wood_stove");
                modelList.Add("ch_crate32x48");
                modelList.Add("com_barrel_blue_dirt");
                modelList.Add("com_barrel_biohazard_rust");
                modelList.Add("com_bike_destroyed");
                modelList.Add("com_tv1_d");
                modelList.Add("com_cardboardbox01");
                modelList.Add("com_crate01");
                modelList.Add("com_pallet");
                modelList.Add("com_stove_d");
                modelList.Add("com_trashbag_pose2");
                modelList.Add("goat_dead_01");
                modelList.Add("me_refrigerator_d");
                modelList.Add("pb_lawnchair_wht");
            }
        }
        private static Vector3 GetModelOffset(int index)
        {
            switch (_mapname)
            {
                case "mp_dome":
                    if (index == 0) return new Vector3(0, 0, 10);
                    //if (index == 5) return new Vector3(0, 0, 13);
                    //if (index == 6) return new Vector3(0, 0, 14);
                    if (index == 7) return new Vector3(0, 0, -8);
                    break;
                case "mp_bravo":
                    if (index == 10) return new Vector3(0, 0, 30);
                    break;
                case "mp_lambeth":
                    if (index == 0) return new Vector3(0, 0, 30);
                    if (index == 11) return new Vector3(0, 0, 18);
                    if (index == 15) return new Vector3(0, 0, 30);
                    break;
            }
            return new Vector3(0, 0, 0);
        }
        private static string GetModelName(int modelIndex)
        {
            if (modelList[modelIndex] == "com_ex_airconditioner") return "Air Conditioner";
            if (modelList[modelIndex] == "vehicle_hummer_destructible") return "Hummer";
            if (modelList[modelIndex] == "com_barrel_benzin") return "Explosive Barrel";
            if (modelList[modelIndex] == "usa_sign_donotenter") return "Do Not Enter Sign";
            if (modelList[modelIndex] == "com_plasticcase_green_rifle") return "Rifle Case";
            if (modelList[modelIndex] == "me_electricbox4") return "Electric Box";
            if (modelList[modelIndex] == "com_barrel_white_rust") return "White Barrel";
            if (modelList[modelIndex] == "com_barrel_blue_rust") return "Blue Barrel";
            if (modelList[modelIndex] == "berlin_hesco_barrier_med") return "Barrier";
            if (modelList[modelIndex] == "com_wooden_pallet") return "Wooden Pallet";
            if (modelList[modelIndex] == "ch_furniture_teachers_desk1") return "Desk";
            if (modelList[modelIndex] == "vehicle_forklift") return "Forklift";
            if (modelList[modelIndex] == "icbm_electronic_cabinet1") return "Electric Cabinet 1";
            if (modelList[modelIndex] == "icbm_electronic_cabinet2") return "Electric Cabinet 2";
            if (modelList[modelIndex] == "icbm_electronic_cabinet3") return "Electric Cabinet 3";
            if (modelList[modelIndex] == "icbm_electronic_cabinet4") return "Electric Cabinet 4";
            if (modelList[modelIndex] == "icbm_electronic_cabinet5") return "Electric Cabinet 5";
            if (modelList[modelIndex] == "com_trashcan_metal_closed") return "Trash Can";
            if (modelList[modelIndex] == "com_barrel_black") return "Black Barrel";
            if (modelList[modelIndex] == "vehicle_subcompact_blue_destructible") return "Blue Sedan";
            if (modelList[modelIndex] == "vehicle_pickup_destructible_mp") return "Pickup Truck";
            if (modelList[modelIndex] == "vehicle_coupe_green_destructible") return "Green Coupe";
            if (modelList[modelIndex] == "com_newspaperbox_blue") return "Newspaper Box";
            if (modelList[modelIndex] == "vehicle_coupe_white_destructible") return "White Coupe";
            if (modelList[modelIndex] == "usa_gas_station_trash_bin_01") return "Plastic Trash Bin";
            if (modelList[modelIndex] == "me_dumpster_close") return "Dumpster";
            if (modelList[modelIndex] == "com_cardboardbox03") return "Cardboard Box";
            if (modelList[modelIndex] == "com_cardboardbox01") return "Cardboard Box";
            if (modelList[modelIndex] == "com_bicycle_urban_green") return "Bicycle";
            if (modelList[modelIndex] == "mannequin_female_torso") return "Mannequin";
            if (modelList[modelIndex] == "com_wall_streetlamppost") return "Lamp Post";
            if (modelList[modelIndex] == "vehicle_uaz_hardtop_destructible_mp") return "UAZ Truck";
            if (modelList[modelIndex] == "com_cafe_chair2") return "Chair";
            if (modelList[modelIndex] == "com_cafe_table2") return "Table";
            if (modelList[modelIndex] == "ch_sign_noparking") return "No Parking Sign";
            if (modelList[modelIndex] == "india_vehicle_rksw") return "Rickshaw";
            if (modelList[modelIndex] == "com_tv1") return "TV";
            if (modelList[modelIndex] == "chicken") return "Brown Chicken";
            if (modelList[modelIndex] == "vehicle_uaz_van_destructible") return "UAZ Van";
            if (modelList[modelIndex] == "vehicle_80s_sedan1_red_destructible_mp") return "Red Sedan";
            if (modelList[modelIndex] == "com_crate01") return "Wooden Crate";
            if (modelList[modelIndex] == "com_pallet_stack") return "Pallet Stack";
            if (modelList[modelIndex] == "com_potted_plant_large") return "Big Potted Plant";
            if (modelList[modelIndex] == "me_banana_box1") return "Bananas Box";
            if (modelList[modelIndex] == "me_citrus_box1") return "Oranges Box";
            if (modelList[modelIndex] == "me_dumpster_close_green") return "Green Dumpster";
            if (modelList[modelIndex] == "me_wood_cage_large") return "Big Wood Cage";
            if (modelList[modelIndex] == "me_wood_cage_small") return "Small Wood Cage";
            if (modelList[modelIndex] == "me_refrigerator2") return "Refrigerator";
            if (modelList[modelIndex] == "com_trashcan_metal") return "Metal Trash Can";
            if (modelList[modelIndex] == "com_barrel_biohazard") return "White Barrel";
            if (modelList[modelIndex] == "concrete_barrier_damaged_2") return "Concrete Barrier";
            if (modelList[modelIndex] == "foliage_pacific_bushtree02_animated") return "Bush";
            if (modelList[modelIndex] == "com_cafe_chair") return "Chair";
            if (modelList[modelIndex] == "foliage_pacific_palms08_animated") return "Ferns 1";
            if (modelList[modelIndex] == "foliage_afr_plant_fern_01a") return "Ferns 2";
            if (modelList[modelIndex] == "ch_washer_01") return "Washer";
            if (modelList[modelIndex] == "ch_wood_stove") return "Wood Stove";
            if (modelList[modelIndex] == "com_dresser") return "Dresser";
            if (modelList[modelIndex] == "com_trashbag") return "Trash Bag";
            if (modelList[modelIndex] == "com_wagon_donkey_nohandle") return "Small Wagon";
            if (modelList[modelIndex] == "ch_woodfence01") return "Wood Fence";
            if (modelList[modelIndex] == "pb_donkey_cart") return "Donkey Cart";
            if (modelList[modelIndex] == "com_powerpole1") return "Power Pole";
            if (modelList[modelIndex] == "ch_mattress_bent_2") return "Bent Mattress";
            if (modelList[modelIndex] == "com_stepladder") return "Step Ladder";
            if (modelList[modelIndex] == "com_stepladder_closed") return "Closed Step Ladder";
            if (modelList[modelIndex] == "com_locker_double") return "Lockers";
            if (modelList[modelIndex] == "com_plasticcase_beige_big") return "Beige Crate";
            if (modelList[modelIndex] == "com_powerpole3") return "Power Pole";
            if (modelList[modelIndex] == "com_trashcan_metal_with_trash") return "Metal Trash Can";
            if (modelList[modelIndex] == "com_trashbag2_black") return "Black Trash Bag";
            if (modelList[modelIndex] == "com_wheelbarrow") return "Wheelbarrow";
            if (modelList[modelIndex] == "com_water_heater") return "Water Heater";
            if (modelList[modelIndex] == "machinery_car_battery") return "Car Battery";
            if (modelList[modelIndex] == "machinery_generator") return "Generator";
            if (modelList[modelIndex] == "vehicle_taxi_yellow_destructible_dusty") return "Taxi";
            if (modelList[modelIndex] == "vehicle_jeep_destructible_dusty") return "Jeep";
            if (modelList[modelIndex] == "com_filecabinetblackclosed") return "File Cabinet";
            if (modelList[modelIndex] == "com_firehydrant") return "Fire Hydrant";
            if (modelList[modelIndex] == "ch_snack_machine_big_usa") return "Snack Machine";
            if (modelList[modelIndex] == "concrete_barrier_damaged_1") return "Concrete Barrier";
            if (modelList[modelIndex] == "furniture_chair1_dusty") return "Chair";
            if (modelList[modelIndex] == "furniture_desk_office_dust") return "Desk";
            if (modelList[modelIndex] == "handicapsign_01") return "Handicap Sign";
            if (modelList[modelIndex] == "ma_atm_standalone") return "ATM Machine";
            if (modelList[modelIndex] == "strip_mall_light_post_short") return "Light Post";
            if (modelList[modelIndex] == "road_barrier_post") return "Road Post";
            if (modelList[modelIndex] == "prop_photocopier_destructible_02") return "Photocopier";
            if (modelList[modelIndex] == "hotdog_cart_static") return "Hot Dog Cart";
            if (modelList[modelIndex] == "com_barrel_white") return "White Barrel";
            if (modelList[modelIndex] == "com_barrel_corrosive_rust") return "Blue Barrel";
            if (modelList[modelIndex] == "com_barrier_tall1") return "Tall Concrete Barrier";
            if (modelList[modelIndex] == "vehicle_jeep_destructible") return "Jeep";
            if (modelList[modelIndex] == "ctl_light_spotlight_generator") return "Spotlight";
            if (modelList[modelIndex] == "pb_cement_bag") return "Cement Bag";
            if (modelList[modelIndex] == "com_trafficcone02") return "Traffic Cone";
            if (modelList[modelIndex] == "machinery_oxygen_tank01") return "Oxygen Tank";
            if (modelList[modelIndex] == "ny_manhattan_barrier_sawhorse") return "Wooden Barrier";
            if (modelList[modelIndex] == "com_pallet") return "Pallet";
            if (modelList[modelIndex] == "rubble_concrete_slab_01") return "Concrete Slab";
            if (modelList[modelIndex] == "vehicle_firetruck_dsty") return "Fire Truck";
            if (modelList[modelIndex] == "vehicle_subcompact_white_destroyed") return "White Sub Compact";
            if (modelList[modelIndex] == "vehicle_subcompact_mica_destroyed") return "Green Sub Compact";
            if (modelList[modelIndex] == "vehicle_taxi_yellow") return "Taxi";
            if (modelList[modelIndex] == "com_shopping_cart") return "Shopping Cart";
            if (modelList[modelIndex] == "com_cardboardbox06") return "Cardboard Box";
            if (modelList[modelIndex] == "ch_crate24x36") return "Wooden Crate";
            if (modelList[modelIndex] == "com_bike") return "Bike";
            if (modelList[modelIndex] == "com_bench") return "Bench";
            if (modelList[modelIndex] == "com_stove_d") return "Stove";
            if (modelList[modelIndex] == "furniture_bookshelf_dusty") return "Bookshelf";
            if (modelList[modelIndex] == "lam_playpen") return "Playpen";
            if (modelList[modelIndex] == "me_dumpster_green") return "Dumpster";
            if (modelList[modelIndex] == "me_swing") return "Swings";
            if (modelList[modelIndex] == "me_slide") return "Slide";
            if (modelList[modelIndex] == "vehicle_tractor_plow") return "Tractor Plow";
            if (modelList[modelIndex] == "vehicle_small_hatch_turq_destroyed") return "Turquoise Hatchback";
            if (modelList[modelIndex] == "com_stove") return "Stove";
            if (modelList[modelIndex] == "me_refrigerator_d") return "Refrigerator";
            if (modelList[modelIndex] == "pb_couch") return "Couch";
            if (modelList[modelIndex] == "vehicle_80s_sedan1_tandest") return "Tan Sedan";
            if (modelList[modelIndex] == "bench1") return "Wooden Bench";
            if (modelList[modelIndex] == "com_ammo_pallet") return "Ammo Crate";
            if (modelList[modelIndex] == "com_metalbench") return "Metal Bench";
            if (modelList[modelIndex] == "com_recyclebin01") return "Recycling Bin";
            if (modelList[modelIndex] == "com_trashbag1_white") return "Trash Bag";
            if (modelList[modelIndex] == "foliage_gardenflowers_red") return "Red Flowers";
            if (modelList[modelIndex] == "paris_planter_large_01") return "Rectangle Planter";
            if (modelList[modelIndex] == "paris_planter_large_02") return "Flower Pot";
            if (modelList[modelIndex] == "foliage_grass_64_square") return "Square Grass Planter";
            if (modelList[modelIndex] == "me_dumpster") return "Dumpster";
            if (modelList[modelIndex] == "vehicle_subcompact_green_destructible") return "Green Sub Compact";
            if (modelList[modelIndex] == "vehicle_subcompact_white_destructible") return "White Sub Compact";
            if (modelList[modelIndex] == "vehicle_scooter_vespa_destruct_cream") return "White Scooter";
            if (modelList[modelIndex] == "com_office_chair_black") return "Office Chair";
            if (modelList[modelIndex] == "com_janitor_bucketmop") return "Janitor Bucket And Mop";
            if (modelList[modelIndex] == "com_trash_can_berlin") return "Trash Can";
            if (modelList[modelIndex] == "props_foliage_horsetail") return "Horsetail";
            if (modelList[modelIndex] == "berlin_concrete_barrier") return "Concrete Barrier";
            if (modelList[modelIndex] == "berlin_me_dumpster_close_green") return "Green Dumpster";
            if (modelList[modelIndex] == "com_red_toolbox_dusty") return "Toolbox";
            if (modelList[modelIndex] == "com_cinderblockstack") return "Cinder Block Stack";
            if (modelList[modelIndex] == "ch_crate32x48") return "Wooden Crate";
            if (modelList[modelIndex] == "ch_crate32x48_snow") return "Snowy Crate";
            if (modelList[modelIndex] == "com_barrel_blue_rust_snow") return "Blue Barrel";
            if (modelList[modelIndex] == "com_barrel_tan_snow") return "Tan Barrel";
            if (modelList[modelIndex] == "com_pallet_2_snow") return "Snowy Pallet";
            if (modelList[modelIndex] == "com_red_toolbox") return "Toolbox";
            if (modelList[modelIndex] == "junk_dumpster_snow_green") return "Green Dumpster";
            if (modelList[modelIndex] == "me_telegraphpole2") return "Telegraph Pole";
            if (modelList[modelIndex] == "com_restaurantchair_2") return "Chair";
            if (modelList[modelIndex] == "chicken_black_white") return "Black And White Chicken";
            if (modelList[modelIndex] == "com_cardboardboxshortclosed_1") return "Cardboard Box";
            if (modelList[modelIndex] == "com_trashbag1_green") return "Green Trash Bag";
            if (modelList[modelIndex] == "me_basket_rattan01") return "Basket";
            if (modelList[modelIndex] == "vehicle_80s_hatch2_tan_destructible_mp") return "Tan Hatchback";
            if (modelList[modelIndex] == "com_cafe_table1") return "Table";
            if (modelList[modelIndex] == "com_trafficcone01_uk") return "UK Traffic Cone";
            if (modelList[modelIndex] == "com_trash_recycle_bin_cans_blue") return "Blue Can Recycle Bin";
            if (modelList[modelIndex] == "com_trash_recycle_bin_trash_yellow") return "Yellow Trash Recycle Bin";
            if (modelList[modelIndex] == "furniture_chair_airport") return "Airport Chairs";
            if (modelList[modelIndex] == "ny_barrier_pedestrian_01") return "Street Barrier";
            if (modelList[modelIndex] == "utility_sign_wet_floor") return "Wet Floor Sign";
            if (modelList[modelIndex] == "vehicle_london_cab_black_destructible") return "Black London Taxi";
            if (modelList[modelIndex] == "com_barrel_blue_dirt") return "Blue Barrel";
            if (modelList[modelIndex] == "com_barrel_biohazard_rust") return "White Barrel";
            if (modelList[modelIndex] == "com_bike_destroyed") return "Destroyed Bike";
            if (modelList[modelIndex] == "com_tv1_d") return "Destroyed TV";
            if (modelList[modelIndex] == "com_trashbag_pose2") return "Trash Bag";
            if (modelList[modelIndex] == "goat_dead_01") return "Dead Goat";
            if (modelList[modelIndex] == "pb_lawnchair_wht") return "White Lawnchair";
            else return modelList[modelIndex];
        }

        private static void StartPlayerHUD(Entity player)
        {
            HudElem topControls = HudElem.CreateFontString(player, HudElem.Fonts.Objective, 1.5f);
            topControls.SetPoint("TOPCENTER", "TOPCENTER", 0, 15);
            if (player.SessionTeam == "axis") topControls.SetText("Press [{+frag}] to open Model Menu | Press [{+actionslot 3}] for Info.");
            else topControls.SetText("Press [{+actionslot 3}] for Info.");
        }
        private static void InitCommands(Entity player)
        {
            player.NotifyOnPlayerCommand("modelMenu", "+frag");
            player.NotifyOnPlayerCommand("changeDir", "+smoke");
            player.NotifyOnPlayerCommand("showInfo", "+actionslot 3");
            player.NotifyOnPlayerCommand("rotateSlow", "+actionslot 4");
            player.NotifyOnPlayerCommand("stopRotate", "-actionslot 4");
            player.NotifyOnPlayerCommand("rotateFast", "+actionslot 5");
            player.NotifyOnPlayerCommand("stopRotate", "-actionslot 5");
            player.NotifyOnPlayerCommand("mdlMnuUp", "+reload");
            player.NotifyOnPlayerCommand("mdlMnuDwn", "+breath_sprint");
            player.NotifyOnPlayerCommand("selectModel", "+activate");
            initNotifiables(player);
        }
        private static void initNotifiables(Entity notifier)
        {
            notifier.OnNotify("modelMenu", (player) =>
                {
                    if (player.SessionTeam != "axis") return;

                    if (player.GetField<bool>("hasMenuOpen"))
                        closeModelMenu(player);
                    else OpenModelMenu(player);
                });
            notifier.OnNotify("changeDir", (player) =>
                {
                    if (player.SessionTeam != "axis") return;
                    if (!player.HasField("modelRotation")) return;

                    if (!player.GetField<bool>("modelRotation"))
                        player.SetField("modelRotation", true);
                    else player.SetField("modelRotation", false);
                });
            notifier.OnNotify("showInfo", (player) =>
                {
                    if (!player.HasField("introUp")) return;
                    if (!player.GetField<bool>("introUp"))
                        ShowInfoToPlayer(player);
                });
            if (notifier.SessionTeam != "axis") return;
            notifier.OnNotify("rotateSlow", (player) =>
                    rotatePlayerModel(player, false));
            notifier.OnNotify("rotateFast", (player) =>
                    rotatePlayerModel(player, true));
            notifier.OnNotify("stopRotate", (player) =>
                    stopModelRotation(player));
            notifier.OnNotify("mdlMnuDwn", (player) =>
                {
                    if (!player.GetField<bool>("hasMenuOpen")) return;

                    int newCurrent = player.GetField<int>("currentModel");
                    player.GetField<HudElem[]>("menuItems")[newCurrent].Color = new Vector3(1, 1, 1);
                    newCurrent--;
                    if (newCurrent < 0) newCurrent = modelList.Count - 1;
                    player.SetField("currentModel", newCurrent);
                    player.GetField<HudElem[]>("menuItems")[newCurrent].Color = new Vector3(1, 1, 0);
                });
            notifier.OnNotify("mdlMnuUp", (player) =>
                {
                    if (!player.GetField<bool>("hasMenuOpen")) return;

                    int newCurrent = player.GetField<int>("currentModel");
                    player.GetField<HudElem[]>("menuItems")[newCurrent].Color = new Vector3(1, 1, 1);
                    newCurrent++;
                    if (newCurrent == modelList.Count) newCurrent = 0;
                    player.SetField("currentModel", newCurrent);
                    player.GetField<HudElem[]>("menuItems")[newCurrent].Color = new Vector3(1, 1, 0);
                });
            notifier.OnNotify("selectModel", (player) =>
                {
                    if (!player.GetField<bool>("hasMenuOpen") || !player.HasField("modelProp")) return;
                    player.GetField<Entity>("modelProp").SetModel(modelList[player.GetField<int>("currentModel")]);
                    player.SetField("selectedModel", player.GetField<int>("currentModel"));
                    closeModelMenu(player);
                });
        }
        private static void ShowInfoToPlayer(Entity player)
        {
            if (player.SessionTeam == "axis")
            {
                player.SetField("introUp", true);
                player.IPrintLnBold("^4You are a Prop.");
                AfterDelay(2000, () =>
                    {
                        player.IPrintLnBold("^4Press [{+frag}] to open the model menu.");
                        AfterDelay(2000, () =>
                            {
                                player.IPrintLnBold("^4Hold [{+actionslot 4}] or [{+actionslot 5}] to rotate your model.");
                                AfterDelay(2000, () =>
                                    {
                                        player.IPrintLnBold("^4Press [{+smoke}] to change rotation direction.");
                                        player.SetField("introUp", false);
                                    });
                            });
                    });
            }
            else if (player.SessionTeam == "allies")
            {
                player.SetField("introUp", true);
                player.IPrintLnBold("^4You are a Hunter.");
                AfterDelay(2000, () =>
                    {
                        player.IPrintLnBold("^4Search and kill the Props.");
                        AfterDelay(2000, () =>
                            {
                                player.IPrintLnBold("^4The Props have the form of a model.");
                                player.SetField("introUp", false);
                            });
                    });
            }
            else
            {
                player.IPrintLnBold("^4You are a Spectator.");
            }
        }
        private static void OpenModelMenu(Entity player)
        {
            player.SetField("hasMenuOpen", true);
            HudElem menuControls = HudElem.CreateFontString(player, HudElem.Fonts.Objective, 2);
            menuControls.SetPoint("TOPCENTER", "TOPCENTER", 0, 50);
            menuControls.SetText("Press [{+breath_sprint}] to go up, [{+reload}] to go down, and [{+activate}] to select model.");
            player.SetField("menuControls", menuControls);
            HudElem[] menuItems = new HudElem[modelList.Count];
            for (int i = 0; i < modelList.Count; i++)
            {
                menuItems[i] = HudElem.CreateFontString(player, HudElem.Fonts.HudSmall, 1);
                menuItems[i].SetPoint("TOPCENTER", "TOPCENTER", 0, 80 + (10 * i));
                menuItems[i].SetText(GetModelName(i));
                if (i == player.GetField<int>("currentModel"))
                    menuItems[i].Color = new Vector3(1, 1, 0);
            }
            player.SetField("menuItems", new Parameter(menuItems));
        }
        private static void closeModelMenu(Entity player)
        {
            player.SetField("hasMenuOpen", false);
            HudElem[] menuItems = player.GetField<HudElem[]>("menuItems");
            foreach (HudElem item in menuItems)
                item.Destroy();
            player.GetField<HudElem>("menuControls").Destroy();
        }
        private static void rotatePlayerModel(Entity player, bool fast)
        {
            if (!player.HasField("modelProp")) return;
            player.SetField("rotateModel", true);
            OnInterval(100, () =>
                {
                    if (player.GetField<bool>("rotateModel") && player.HasField("modelProp"))
                    {
                        Entity model = player.GetField<Entity>("modelProp");
                        int newYaw = 0;
                        float speed = .1f;
                        bool rotateDir = player.GetField<bool>("modelRotation");

                        //if (fast) speed = .05f;

                        if (fast)
                        {
                            if (rotateDir)
                                newYaw = 20;
                            else newYaw = -20;
                        }
                        else
                        {
                            if (rotateDir)
                                newYaw = 10;
                            else newYaw = -10;
                        }

                        model.RotateYaw(newYaw, speed);
                        return true;
                    }
                    else return false;
                });
        }
        private static void stopModelRotation(Entity player)
        {
            player.SetField("rotateModel", false);
        }
    }
}
